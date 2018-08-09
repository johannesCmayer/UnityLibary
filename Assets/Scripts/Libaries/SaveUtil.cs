using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

public static class SaveUtil
{
    private static JsonSerializerSettings jSettings;
    private static List<string> usedNames = new List<string>();

    static SaveUtil()
    {
        var settings = new JsonSerializerSettings();
        settings.PreserveReferencesHandling = PreserveReferencesHandling.All;
        jSettings = settings;
    }

    #region FolderLocations

    public static string SaveFolder
    {
        get
        {
            return CreateDirectoryIfNotExistent(Application.persistentDataPath + "/user_save_data/");
        }
    }

    public static string InternalSaveFolder
    {
        get
        {
            return CreateDirectoryIfNotExistent(Application.streamingAssetsPath + "/internal_save_data/");
        }
    }

    public static string GeneratedTexturesFolder
    {
        get
        {
            return CreateDirectoryIfNotExistent(SaveFolder + "generated_textures/");
        }
    }

    public static string ConfigFolder
    {
        get
        {
            return CreateDirectoryIfNotExistent(SaveFolder + "config/");
        }
    }

    public static string InternalConfigFolder
    {
        get
        {
            return CreateDirectoryIfNotExistent(InternalSaveFolder + "config/");
        }
    }

    
    public static string TempDataFolder
    {
        get
        {
            return CreateDirectoryIfNotExistent(SaveFolder + "temp/");
        }
    }

    #endregion
    #region GenericSaveFunctions

    public static void SaveObject(object @object, string name, string folderPath, string extension = ".json")
    {

        var data = JsonConvert.SerializeObject(@object, Formatting.Indented, jSettings);
        using (var w = new StreamWriter(folderPath + name + extension))
            w.Write(data);
    }

    public static T LoadObjectOrDefault<T>(string name, string folderPath, string extension = ".json") where T : new()
    {
        try
        {
            var loadPath = folderPath + name + extension;
            T c = default(T);
            if (!File.Exists(loadPath))
            {
                c = new T();
                SaveObject(c, name, folderPath, extension);
            }
            else
            {
                var r = new StreamReader(loadPath);
                c = JsonConvert.DeserializeObject<T>(r.ReadToEnd(), jSettings);
                r.Close();
            }
            if (c == null)
            {
                Debug.LogError($"{typeof(T).Name} did not load correctly, creating and saving default!");
                c = new T();
                SaveObject(c, name, folderPath, extension);
            }
            if (c == null)
                throw new System.Exception($"Could not Load nor create default object of type {typeof(T).Name}!");
            return c;
        }
        catch (JsonSerializationException)
        {
            Debug.LogError($"Could not load file '{name}{extension}' in '{folderPath}'. It might be missing or corrupt.");
            return default(T);
        }
    }

    public static void DeleteObject(string name, string folderPath, string extension = ".json")
    {
        File.Delete(folderPath + name + extension);
    }

    public static void SaveObject(object obj, string name)
    {
        SaveObject(obj, name, SaveFolder);
    }
    public static T LoadObjectOrDefault<T>(string name) where T : new()
    {
        return LoadObjectOrDefault<T>(name, SaveFolder);
    }

    #endregion
    #region ConfigSaveFunctions

    public static void SaveConfig(object config, string configName)
    {
        SaveObject(config, configName, ConfigFolder);
    }
    public static T LoadConfigOrDefault<T>(string configName) where T : new()
    {
        return LoadObjectOrDefault<T>(configName, ConfigFolder);
    }
    public static void SaveConfigInternal(object config, string configName)
    {
        SaveObject(config, configName, InternalConfigFolder);
    }
    public static T LoadConfigOrDefaultFromInternal<T>(string configName) where T : new()
    {
        return LoadObjectOrDefault<T>(configName, InternalConfigFolder);
    }

    public static void SaveConfig<T>(object config)
    {
        SaveObject(config, typeof(T).Name, ConfigFolder);
    }
    public static T LoadConfigOrDefault<T>() where T : new()
    {
        return LoadObjectOrDefault<T>(typeof(T).Name, ConfigFolder);
    }
    public static void SaveConfigInternal<T>(object config)
    {
        SaveObject(config, typeof(T).Name, InternalConfigFolder);
    }
    public static T LoadConfigOrDefaultFromInternal<T>() where T : new()
    {
        return LoadObjectOrDefault<T>(typeof(T).Name, InternalConfigFolder);
    }

    #endregion    

    public static void SaveTexture(string name, Texture2D texture)
    {
        string filename = GeneratedTexturesFolder + name + ".png";
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(filename, bytes);
    }
    public static Texture2D LoadTexture(string name)
    {
        var imageBytes = File.ReadAllBytes(GeneratedTexturesFolder + name + ".png");
        var texture = new Texture2D(512, 512);
        texture.LoadImage(imageBytes, false);
        return texture;
    }
    public static Sprite LoadTextureAsSprite(string name)
    {
        var texture = LoadTexture(name);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
    }
    public static void DeleteTexture(string name)
    {
        DeleteObject(name, GeneratedTexturesFolder, ".png");
    }

    public static string GetUniqueSaveName()
    {
        var name = System.DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff");
        if (usedNames.Contains(name))
        {
            name += "-";
            do
            {
                name += Random.Range(0, 9).ToString();
            } while (usedNames.Contains(name));
        }
        usedNames.Add(name);
        return name;
    }

    public static void DeleteAllFilesInDir(string path)
    {
        var files = Directory.GetFiles(path);
        for (int i = 0; i < files.Length; i++)
        {
            File.Delete(files[i]);
        }
    }

    public static void DeleteTempFiles()
    {
        DeleteAllFilesInDir(TempDataFolder);
    }

    public static char MakeCharFilenameSave(char @char)
    {
        if (System.Char.IsLetterOrDigit(@char) ||
            @char == ' ' || @char == '_' || @char == '-')
        {
            return @char;
        }
        return '\0';
    }

    private static void CheckTrainerNameForUnderscore(string brainName)
    {
        if (brainName.Contains("_"))
            throw new System.Exception($"Brain names are not allowed to contain underscores (_)! {brainName} is not valid!");
    }

    private static string CreateDirectoryIfNotExistent(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }
}

public class SaveFile<T> where T : new()
{
    public T dataObject { get; private set; }
    public readonly string fileDir;
    public readonly string fileName;
    public readonly string fileExtension;

    public SaveFile(string saveDir, string fileName, string fileExtension = ".json")
    {
        this.fileDir = saveDir;
        this.fileName = fileName;
        this.fileExtension = fileExtension;
    }

    public SaveFile(T data, string fileDir, string fileName, string fileExtension = ".json")
    {
        this.dataObject = data;
        this.fileDir = fileDir;
        this.fileName = fileName;
        this.fileExtension = fileExtension;
    }

    public void Save()
    {
        SaveUtil.SaveObject(dataObject, fileName, fileDir, fileExtension);
    }

    public T Load()
    {
        var loadData = SaveUtil.LoadObjectOrDefault<T>(fileName, fileDir, fileExtension);
        dataObject = loadData;
        return loadData;
    }

    public void Delete()
    {
        SaveUtil.DeleteObject(fileName, fileDir, fileExtension);
        dataObject = default(T);
    }
}
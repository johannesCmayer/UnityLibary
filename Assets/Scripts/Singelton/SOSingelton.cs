using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SOSingelton<T> : ScriptableObject where T : ScriptableObject
{
    private static T _Instance;
    public static T Instance
    {
        get
        {
            return _Instance;
        }
    }

    void OnEnable()
    {        
        if (_Instance == null)
        {
            var instances = Resources.FindObjectsOfTypeAll<T>();
            if (instances.Length == 0)
                throw new System.Exception($"There is no instance of {typeof(T).Name} in the Project," +
                    $"but you are trying to acces it!");
            if (instances.Length > 1)
                throw new System.Exception($"There are multiple instances of {typeof(T).Name} in the Project," +
                    $"there needs to be only one!");
            _Instance = instances[0];
        }
    }
}

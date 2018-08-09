using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;

public abstract class MonoSingelton<T> : MonoBehaviour where T : MonoSingelton<T>, new()
{
    private static T _Instance;
    public static T Instance
    {
        get
        {            
            if (_Instance == null)
            {
                var instances = FindObjectsOfType<T>();
                if (instances.Length > 1)
                    throw new System.Exception($"There are multipe mono singletons of the " +
                        $"requestet type {typeof(T).Name} in the Scene.");
                if (instances.Length == 0)
                    return null;
                _Instance = instances[0];
                _Instance.Initialize();
            }
            return _Instance;
        }
        set
        {
            _Instance = value;
        }
    }

    protected virtual void Initialize()
    {
    }
}

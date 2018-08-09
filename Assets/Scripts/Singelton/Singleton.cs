using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class Singelton<T> where T : Singelton<T>, new()
{
    private static bool initialized = false;

    private static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new T();
                if (_Instance == null)
                    throw new System.Exception($"No {typeof(T).Name} could be created!");
            }
            if (!initialized)
            {
                _Instance.Initialize();
                initialized = true;
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

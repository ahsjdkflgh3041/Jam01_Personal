using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton_mj <T>: MonoBehaviour where T: Component
{
    private static T _instance;

    public static T I  // Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();
                }
            }

            return _instance;
        }
    }

    public virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as T;
            if (transform.parent == null){// root 아니면 실행X
                DontDestroyOnLoad(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
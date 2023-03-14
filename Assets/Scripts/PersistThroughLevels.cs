using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistThroughLevels : MonoBehaviour
{
    private static PersistThroughLevels instance = null;

    // this object will child all things we want to keep throughout levels like UI, etc.

    public static PersistThroughLevels Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
        instance = null;
    }
}

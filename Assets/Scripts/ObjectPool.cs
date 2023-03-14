using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{

    private static ObjectPool instance = null;

    [SerializeField] GameObject ballPrefab;
    [SerializeField] int poolSize = 10;

    GameObject[] poolArray;

    public static ObjectPool Instance
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

        PopulatePool();
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
        instance = null;
    }

    // store objects into object and set limit at start to be used
    void PopulatePool()
    {
        poolArray = new GameObject[poolSize];

        for (int I = 0; I < poolArray.Length; I++)
        {
            poolArray[I] = Instantiate(ballPrefab, transform);
            poolArray[I].SetActive(false);
        }
    }

    // activate the upmost deactivated object in array else exit loop early
    public void EnableObjectInPool()
    {
        for (int I = 0; I < poolArray.Length; I++)
        {
            if (poolArray[I].activeInHierarchy == false)
            {
                poolArray[I].SetActive(true);
                return;
            }
        }
    }

}

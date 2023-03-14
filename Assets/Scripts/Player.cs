using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private bool canShoot = false;

    private float attackTimer = 0.0f;
    private float waitTimer = 0.2f;

    [SerializeField]
    private GameObject bombAI;

    [HideInInspector]
    public Vector3 originalScale;
    [HideInInspector]
    public Vector3 originalPosition;

    private static Player instance = null;

    [HideInInspector]
    public float maximumX = 6.8f;
    [HideInInspector]
    public float minimumX = -6.8f;

    public bool SetPlayerShoot(bool CanShoot)
    {
        return canShoot = CanShoot;
    }

    public static Player Instance
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

    private void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
    }

    public void DestroyPlayer()
    {
        Destroy(gameObject);
        instance = null;
    }

    private void Update()
    {
        if (canShoot)
        {
            if (Input.GetMouseButton(0))
            {
                attackTimer += Time.deltaTime; // timer begins
              
                if (attackTimer >= waitTimer)
                {
                    if (ObjectPool.Instance != null)
                    {
                        ObjectPool.Instance.EnableObjectInPool();
                    }

                    attackTimer = 0;
                }
            }
        }
    }
}

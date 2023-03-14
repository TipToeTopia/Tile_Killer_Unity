using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSelfDestruct : MonoBehaviour
{
    [SerializeField]
    private float time = 3.0f;

    void Start()
    {
        Destroy(gameObject, time);
    }
}

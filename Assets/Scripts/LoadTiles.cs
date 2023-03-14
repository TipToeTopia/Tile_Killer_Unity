using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTiles : MonoBehaviour
{
    [Header("Grid For Level")]

    [SerializeField]
    private float xMinimum = -7.0f;
    [SerializeField]
    private float xMaximum = 6.0f;
    [SerializeField]
    private float yMinimum = -5.0f;
    [SerializeField]
    private float yMaximum = 1.0f;
    [SerializeField]
    private float gridxInterval = 1.5f;
    [SerializeField]
    private float gridyInterval = 1.0f;

    void Start()
    {
        // load tiles at the start

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadUpGrid(xMinimum, xMaximum, gridxInterval, yMinimum, yMaximum, gridyInterval);
        }
    }

}

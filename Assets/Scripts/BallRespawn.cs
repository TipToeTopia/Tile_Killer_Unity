using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallRespawn : MonoBehaviour
{
    private float despawnCoordinate;

    private void Start()
    {
        despawnCoordinate = GameManager.Instance.GetDespawnCoordinate();
    }

    void Update()
    {
        // respawn if under player

        if (this.transform.position.z < despawnCoordinate)
        {
            GameManager.Instance.RespawnBall();
            Destroy(gameObject);
            
        }
    }
}

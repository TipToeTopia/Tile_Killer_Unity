using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBase : MonoBehaviour
{
    protected float despawnY;

    protected int powerUpSpeed = 5;

    protected GameObject playerInstance;

    protected virtual void Start()
    {
        despawnY = GameManager.Instance.GetDespawnCoordinate();
        playerInstance = Player.Instance.gameObject;

    }

    protected virtual void Update()
    {
        this.transform.position += new Vector3(0.0f, 0.0f, -powerUpSpeed) * Time.deltaTime;

        if (this.transform.position.z < despawnY)
        {
            DestroyPowerUp();   
        }
    }

    protected virtual void OnTriggerEnter(Collider Other)
    {
        if (playerInstance == null)
        {
            DestroyPowerUp();

            return;
        }
    }

    protected void DestroyPowerUp()
    {
        GameManager.Instance.DecrementPowerUpList();
        Destroy(gameObject);
    }
}

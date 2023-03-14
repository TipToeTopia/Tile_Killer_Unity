using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPowerUp : MonoBehaviour
{
    [SerializeField]
    private int powerUpSpeed = 5;

    private GameObject playerInstance;

    [SerializeField]
    private GameObject tileKiller;

    private float despawnY;

    void Start()
    {
        playerInstance = Player.Instance.gameObject;

        despawnY = GameManager.Instance.GetDespawnCoordinate();
    }


    void Update()
    {
        // falling, if below player - destroy

        this.transform.position += new Vector3(0.0f, 0.0f, -powerUpSpeed) * Time.deltaTime;

        if (this.transform.position.z < despawnY)
        {
            GameManager.Instance.DecrementPowerUpList();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider Other)
    {
        // powerup if hits player

        if (playerInstance == null)
        {
            GameManager.Instance.DecrementPowerUpList();
            Destroy(gameObject);
        }
        else
        {
            if (Other.gameObject == playerInstance)
            {
                Instantiate(tileKiller, GameManager.Instance.GetOriginCoordinate().position, Quaternion.identity);
                GameManager.Instance.DecrementPowerUpList();
                Destroy(gameObject);
            }
        }
    }
}

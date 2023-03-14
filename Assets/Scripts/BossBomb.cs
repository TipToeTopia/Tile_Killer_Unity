using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : MonoBehaviour
{
    [SerializeField]
    private int bombSpeed = 5;

    private GameObject playerInstance;

    private float despawnAxis;

    void Start()
    {
        playerInstance = Player.Instance.gameObject;

        despawnAxis = GameManager.Instance.GetDespawnCoordinate();
    }

    void Update()
    {
        // drop bomb from boss position

        this.transform.position += new Vector3(0.0f, 0.0f, -bombSpeed) * Time.deltaTime;

        if (this.transform.position.z < despawnAxis)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider Other)
    {
        // if hit player, decrement lives

        if (playerInstance == null)
        {
            Destroy(gameObject);
        }
        else
        {
            if (Other.gameObject == playerInstance)
            {
                GameManager.Instance.DecrementLives();
                Destroy(gameObject);
            }
        }
    }
}

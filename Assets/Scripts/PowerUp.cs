using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private int powerUpSpeed = 5;

    private GameObject playerObject;

    private float despawnY;

    [SerializeField]
    private AudioClip playerEnlargementSound;

    private const float PLAYER_X_BOUNDARIES = 0.5f;

    void Start()
    {
        playerObject = Player.Instance.gameObject;

        despawnY = GameManager.Instance.GetDespawnCoordinate();

    }

    
    void Update()
    {
        // fall, if below player destroy

        this.transform.position += new Vector3(0.0f, 0.0f, -powerUpSpeed) * Time.deltaTime;

        if (this.transform.position.z < despawnY)
        {
            GameManager.Instance.DecrementPowerUpList();
            Destroy(gameObject);
        }    
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (playerObject == null)
        {
            GameManager.Instance.DecrementPowerUpList();
            Destroy(gameObject);
        }
        else
        {
            if (Other.gameObject == playerObject)
            {
                Player.Instance.maximumX -= PLAYER_X_BOUNDARIES;
                Player.Instance.minimumX += PLAYER_X_BOUNDARIES;

                playerObject.transform.localScale += new Vector3(1f, 0.0f, 0.0f);
                AudioManager.Instance.PlaySound(playerEnlargementSound);
                GameManager.Instance.DecrementPowerUpList();
                Destroy(gameObject);
            }
        }

        
    }
}

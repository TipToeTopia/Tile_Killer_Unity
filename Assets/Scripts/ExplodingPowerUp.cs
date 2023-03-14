using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingPowerUp : MonoBehaviour
{
    [SerializeField]
    private int powerUpSpeed = 5;
    private int explosionRadius = 1;

    private GameObject playerInstance;
    private GameObject ballInstance;

    [SerializeField]
    private Rigidbody explodingBall;

    private float despawnY;

    private int explodingProjectiles = 5;
    private int forceApplied = 200;

    private const string BALL_TAG = "Ball";

    private const int RADIAN_CONSTANT = 2;

    [SerializeField]
    private AudioClip explodingBallSound;

    void Start()
    {
        playerInstance = Player.Instance.gameObject;
        ballInstance = GameObject.FindGameObjectWithTag(BALL_TAG);

        despawnY = GameManager.Instance.GetDespawnCoordinate();
    }


    void Update()
    {
        this.transform.position += new Vector3(0.0f, 0.0f, -powerUpSpeed) * Time.deltaTime;

        if (this.transform.position.z < despawnY)
        {
            GameManager.Instance.DecrementPowerUpList();
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject == playerInstance)
        {
            if (ballInstance != null)
            {
                for (int I = 0; I < explodingProjectiles; I++)
                {
                    // create a ball around ball origin using the loop index for each ball around 360 degrees

                    float Radians = RADIAN_CONSTANT * Mathf.PI / explodingProjectiles * I;

                    float Vertical = Mathf.Sin(Radians);
                    float Horizontal = Mathf.Cos(Radians);

                    Vector3 SpawnDir = new Vector3(Horizontal, 0.0f, Vertical);

                    Vector3 SpawnPosition = ballInstance.transform.position + SpawnDir * explosionRadius;

                    Rigidbody ExplodingBall = Instantiate(explodingBall, SpawnPosition, Quaternion.identity);

                    if (ExplodingBall != null)
                    {
                        ExplodingBall.AddForce(SpawnDir * forceApplied);
                    }
                }
            }

            AudioManager.Instance.PlaySound(explodingBallSound);
            GameManager.Instance.DecrementPowerUpList();

            Destroy(gameObject);
        }
    }
}

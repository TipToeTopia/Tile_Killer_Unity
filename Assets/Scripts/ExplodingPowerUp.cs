using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingPowerUp : PowerUpBase
{
    private int explosionRadius = 1;

    private GameObject ballInstance;

    [SerializeField]
    private Rigidbody explodingBall;

    private int explodingProjectiles = 5;
    private int forceApplied = 200;

    private const string BALL_TAG = "Ball";

    private const int RADIAN_CONSTANT = 2;

    [SerializeField]
    private AudioClip explodingBallSound;

    protected override void Start()
    {
        base.Start();

        ballInstance = GameObject.FindGameObjectWithTag(BALL_TAG);
    }

    protected override void OnTriggerEnter(Collider Other)
    {
        base.OnTriggerEnter(Other);

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

            DestroyPowerUp();
        }
    }
}

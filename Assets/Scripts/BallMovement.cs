using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    private Vector3 initialLaunch = new Vector3(0.0f, 0.0f, 1.0f);

    [SerializeField]
    private Rigidbody rigidBody;

    [SerializeField]
    private float ballSpeed = 7.0f;

    private const float BALL_SPEED_MULTIPLIER = 0.2f;
    private const float BALL_AXIS_UPWARD = 1.0f;
    private const float BALL_DIRECTIONAL_MULTIPLIER = 2.5f;

    [SerializeField]
    private AudioClip reflectionSound;

    private bool hasPlayerLaunchedBall = false;

    private const int BALL_INITIAL_SPAWN_FROM_PLAYER = 1;

    private const string PLAYER_TAG = "Player";

    void Update()
    {
        // player launch, once pressed launch ball

        rigidBody.velocity = rigidBody.velocity.normalized * ballSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            hasPlayerLaunchedBall = true;
        }

        if (hasPlayerLaunchedBall)
        {

            rigidBody.velocity = initialLaunch * ballSpeed;

        }
        else
        {
            if (Player.Instance != null)
            {
                this.transform.position = Player.Instance.transform.position + new Vector3(0.0f, 0.0f, BALL_INITIAL_SPAWN_FROM_PLAYER);
            }
        }
    }

    private void OnCollisionEnter(Collision Collision)
    {
        ballSpeed += BALL_SPEED_MULTIPLIER;
        
        AudioManager.Instance.PlaySound(reflectionSound);

        // get the relative position of ball to paddle and set that as the new directional vector x

        if (Collision.gameObject.tag == PLAYER_TAG)
        {
            float BallPositionOnPaddle = BallRelativePositionToBall(transform.position, Collision.transform.position, Collision.collider.bounds.size.x);

            Vector3 NewDirection = new Vector3(BallPositionOnPaddle, transform.position.y, BALL_AXIS_UPWARD).normalized;

            initialLaunch = NewDirection; 
        }
        else
        {
            Vector3 ContactNormal = Collision.GetContact(0).normal;

            initialLaunch = Vector3.Reflect(initialLaunch, ContactNormal);
        }
    }

    // function used to find offset of ball collision with paddle
    private float BallRelativePositionToBall(Vector3 BallPosition, Vector3 PlayerPosition, float PlayerWidth)
    {
        return ((BallPosition.x - PlayerPosition.x) / PlayerWidth) * BALL_DIRECTIONAL_MULTIPLIER;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float PLAYER_SPEED = 100.0f;

    [SerializeField]
    private Rigidbody rigidBody;

    // world boundaries
    [HideInInspector]
    public float minimumNegativeX; 
    [HideInInspector]
    public float maximumPositiveX;

    private const string MOUSE_X = "Mouse X";

    void Update()
    {
        // mouse movement, and set clamps to be in line with power up scale increase

        minimumNegativeX = Player.Instance.minimumX;
        maximumPositiveX = Player.Instance.maximumX;

        Vector3 PlayerPosition = transform.position;

        if (!GameManager.Instance.isPaused)
        {
            PlayerPosition.x += Input.GetAxis(MOUSE_X) * PLAYER_SPEED * Time.deltaTime;
            PlayerPosition.x = Mathf.Clamp(PlayerPosition.x, minimumNegativeX, maximumPositiveX);
            transform.position = PlayerPosition;
        }

    }
}


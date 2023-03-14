using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnlargementPowerUp : PowerUpBase
{
    [SerializeField]
    private AudioClip playerEnlargementSound;

    private const float PLAYER_X_BOUNDARIES = 0.5f;

    private const float ENLARGEMENT_FACTOR = 1.0f;

    protected override void OnTriggerEnter(Collider Other)
    {
        base.OnTriggerEnter(Other);

        if (Other.gameObject == playerInstance)
        {
            Player.Instance.maximumX -= PLAYER_X_BOUNDARIES;
            Player.Instance.minimumX += PLAYER_X_BOUNDARIES;

            playerInstance.transform.localScale += new Vector3(ENLARGEMENT_FACTOR, 0.0f, 0.0f);

            AudioManager.Instance.PlaySound(playerEnlargementSound);

            DestroyPowerUp();
        }
    }
}

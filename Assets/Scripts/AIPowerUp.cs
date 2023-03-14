using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPowerUp : PowerUpBase
{
    [SerializeField]
    private GameObject tileKiller;

    protected override void OnTriggerEnter(Collider Other)
    {
        // powerup if hits player

        base.OnTriggerEnter(Other);

        if (Other.gameObject == playerInstance)
        {
            Instantiate(tileKiller, GameManager.Instance.GetOriginCoordinate().position, Quaternion.identity);

            DestroyPowerUp();

        }
    }
}

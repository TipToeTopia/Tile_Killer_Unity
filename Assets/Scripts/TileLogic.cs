using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject powerUp;

    [SerializeField]
    private GameObject powerUpsTwo;

    [SerializeField]
    private GameObject powerUpsThree;

    [SerializeField]
    private AudioClip powerUpDropSound;

    [HideInInspector]
    public static int pickupProbability = 10;

    private int powerUpsInGameLimit = 1;

    private const string EXPLODING_BALL_TAG = "Exploding_Ball";

    private const int STARTING_PROBABILITY_RANGE = 1;

    private const int POWER_UP_ONE = 1;
    private const int POWER_UP_TWO = 2;
    private const int POWER_UP_THREE = 3;

    private const float DESTRUCTION_DELAY = 0.01f;

    private void OnCollisionEnter(Collision Collision)
    {
        OnCollisionOrTrigger();
    }

    private void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.tag == EXPLODING_BALL_TAG)
        {
            OnCollisionOrTrigger();
            Destroy(Other.gameObject);
        }
    }

    private void OnCollisionOrTrigger()
    {
        int Rand = Random.Range(STARTING_PROBABILITY_RANGE, pickupProbability);

        if (Rand == POWER_UP_ONE)
        {
            SpawnPowerUp(powerUp); 
        }
        else if (Rand == POWER_UP_TWO)
        {
            SpawnPowerUp(powerUpsTwo);
        }
        else if (Rand == POWER_UP_THREE)
        {
            SpawnPowerUp(powerUpsThree);
        }

        if (this.gameObject != null)
        {
            GameManager.Instance.UpdateScore();
        }

        // slight buffer for ball reflection to be calculated indefinitely

        GameManager.Instance.tileList.Remove(this.gameObject);

        StartCoroutine(TileDestructionDelay());
    }

    private void SpawnPowerUp(GameObject PowerUp)
    {
        if (GameManager.Instance.activePowerUps.Count < powerUpsInGameLimit)
        {
            Instantiate(PowerUp, transform.position, Quaternion.identity);

            AudioManager.Instance.PlaySound(powerUpDropSound);
            GameManager.Instance.IncrementPowerUpList();
        }
    }

    IEnumerator TileDestructionDelay()
    {
        yield return new WaitForSeconds(DESTRUCTION_DELAY);
        Destroy(gameObject);

    }
}

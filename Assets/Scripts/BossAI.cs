using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    // Full boss AI implementation

    [SerializeField]
    private float bossSpeed = 0.5f;

    [SerializeField]
    private float pingPongSpeed = 1.0f;

    [SerializeField]
    private GameObject bombAI;

    [SerializeField]
    private GameObject minionAI;

    [SerializeField]
    private GameObject shieldVisual;

    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private AudioClip shieldActivatedSound;

    [SerializeField]
    private AudioClip bombDroppingSound;

    [SerializeField]
    private ParticleSystem bossDeathParticleEffect;

    private const float YOU_LOSE_COORDINATE = -7.4f;

    private bool isPingPonging = false;
    private bool goingRightDirection = true;
    private bool hasSpawnedAI = false;
    private bool beenImmune = false;
    private bool isImmune = false;

    private bool canSpawnAI = false;
    private bool canBeImmune = false;

    private const float MAXIMUM_RIGHT_LIMIT = 6.0f;
    private const float MAXIMUM_LEFT_LIMIT = -6.0f;

    private const float MAXIMUM_MINION_SPAWN_RIGHT_LIMIT = 6.0f;
    private const float MAXIMUM_MINION_SPAWN_LEFT_LIMIT = -6.0f;
    private const float MAXIMUM_MINION_SPAWN_UP_LIMIT = 1.0f;
    private const float MAXIMUM_MINION_SPAWN_DOWN_LIMIT = -3.0f;

    private const float Y_SPAWN = 0.5f;

    private const float PING_PONG_TIMER = 15.0f;
    private const float BOSS_IMMUNITY_TIMER = 7.0f;

    private float attackTimer = 0.0f;
    private float waitTimer = 1.0f;

    private int bossHealth = 100;
    private int smallBallDamage = 2;
    private int bigBallDamage = 20;
    private int minionSpawnAmount = 2;
    private int powerUpProbability = 400;

    private const int STARTING_PROBABILITY_RANGE = 1;

    private const int POWER_UP_ONE = 1;
    private const int POWER_UP_TWO = 2;

    private const int BOSS_DEATH_SCORE = 100;
    private const int BOSS_DEATH_INTEGER = 0;

    [Header("Set of scripted boss trigger events")]

    private const string PING_PONG_TRIGGER_TAG = "PingPongTrigger";
    private const string WALLS_TAG = "Walls";
    private const string MINION_AI_TRIGGER_TAG = "MinionAITrigger";
    private const string PLAYER_BOMB_TAG = "PlayerBomb";
    private const string BALL_TAG = "Ball";
    private const string IMMUNITY_TRIGGER_TAG = "Invinsible";

    void Update()
    {

        if (this.transform.position.z < YOU_LOSE_COORDINATE)
        {
            // game over

            if (GameManager.Instance != null)
            {
                GameManager.Instance.InstantDeath();
            }
        }

        // pingpong dropping bombs, else continue to go to destination

        if (isPingPonging == true)
        {
            PingPongBoss();

            attackTimer += Time.deltaTime; // timer begins

            if (attackTimer >= waitTimer)
            {
                AudioManager.Instance.PlaySound(bombDroppingSound);
                Instantiate(bombAI, this.transform.position, Quaternion.identity);
                attackTimer = 0;
            }
        }
        else
        {
            this.transform.position += new Vector3(0.0f, 0.0f, -bossSpeed) * Time.deltaTime;

            int Rand = Random.Range(STARTING_PROBABILITY_RANGE, powerUpProbability);

            // series of one time call boss abilities

            RandomPowerUps(Rand);
        }
    }

    private void OnTriggerEnter(Collider Other)
    {
        EventTriggers(Other);
    }

    private void OnCollisionEnter(Collision Collision)
    {
        if (Collision.gameObject.tag == WALLS_TAG)
        {
            Destroy(Collision.gameObject);
        }
        else if (Collision.gameObject.tag == BALL_TAG)
        {
            if (!isImmune)
            {
                DecreaseBossHealth(bigBallDamage);
            }
        }
    }

    IEnumerator PingPongTimer(float Time)
    {
        isPingPonging = true;
        yield return new WaitForSeconds(Time);
        isPingPonging = false;
    }

    IEnumerator BossImmunityTimer(float Time)
    {
        isImmune = true;
        shieldVisual.SetActive(true);
        yield return new WaitForSeconds(Time);
        shieldVisual.SetActive(false);
        isImmune = false;
    }

    private void PingPongBoss()
    {
        // pingpong from side to side

        if (goingRightDirection)
        {
            transform.Translate(new Vector3(1.0f, 0.0f, 0.0f) * pingPongSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(-1.0f, 0.0f, 0.0f) * pingPongSpeed * Time.deltaTime);
        }

        if (this.transform.position.x >= MAXIMUM_RIGHT_LIMIT)
        {
            goingRightDirection = false;
        }

        if (this.transform.position.x <= MAXIMUM_LEFT_LIMIT)
        {
            goingRightDirection = true;
        }
    }

    private void RandomPowerUps(int RandomNumber)
    {
        // power ups, either spawning minion AI or boss shield immunity

        if (RandomNumber == POWER_UP_ONE)
        {
            if (canSpawnAI)
            {
                if (!hasSpawnedAI)
                {
                    for (int I = 0; I < minionSpawnAmount; I++)
                    {
                        Instantiate(minionAI, new Vector3(Random.Range(MAXIMUM_MINION_SPAWN_LEFT_LIMIT, MAXIMUM_MINION_SPAWN_RIGHT_LIMIT), 
                        Y_SPAWN, Random.Range(MAXIMUM_MINION_SPAWN_UP_LIMIT, MAXIMUM_MINION_SPAWN_DOWN_LIMIT)), Quaternion.identity);
                    }

                    hasSpawnedAI = true;
                }
            }
        }
        else if (RandomNumber == POWER_UP_TWO)
        {
            if (canBeImmune)
            {
                if (!beenImmune)
                {
                    AudioManager.Instance.PlaySound(shieldActivatedSound);
                    StartCoroutine(BossImmunityTimer(BOSS_IMMUNITY_TIMER));
                    beenImmune = true;
                }
            }
        }
    }

    private void DecreaseBossHealth(int Amount)
    {
        bossHealth -= Amount;

        healthBar.value = bossHealth;

        if (bossHealth <= BOSS_DEATH_INTEGER)
        {
            Instantiate(bossDeathParticleEffect, this.transform.position, Quaternion.identity);
            GameManager.Instance.UpdateScore(BOSS_DEATH_SCORE);
            GameManager.Instance.YouWin();

            Destroy(gameObject);
        }
    }

    private void EventTriggers(Collider Other)
    {
        // list of scripted trigger events

        if (Other.gameObject.tag == PING_PONG_TRIGGER_TAG)
        {
            Destroy(Other.gameObject);
            StartCoroutine(PingPongTimer(PING_PONG_TIMER));

            canSpawnAI = false;
        }
        else if (Other.gameObject.tag == MINION_AI_TRIGGER_TAG)
        {
            canSpawnAI = true;
            Player.Instance.SetPlayerShoot(true);

        }
        else if (Other.gameObject.tag == IMMUNITY_TRIGGER_TAG)
        {
            canBeImmune = true;
        }
        else if (Other.gameObject.tag == PLAYER_BOMB_TAG)
        {
            if (!isImmune)
            {
                DecreaseBossHealth(smallBallDamage);
            }

            Other.gameObject.SetActive(false);
        }
    }
}


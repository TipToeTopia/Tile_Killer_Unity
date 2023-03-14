using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField]
    private int bombSpeed = 5;

    private const string MINION_AI_TRIGGER_TAG = "MinionAI";
    private const string BOSS_SHIELD_TRIGGER_TAG = "BossShield";

    [SerializeField]
    private ParticleSystem minionDeathParticleEffect;

    private const float TIMED_DEACTIVATION = 5.0f;

    private void OnEnable()
    {
        this.transform.position = Player.Instance.gameObject.transform.position;
        StartCoroutine(DelayedDestruction());
    }

    void Update()
    {

        this.transform.position += new Vector3(0.0f, 0.0f, bombSpeed) * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.tag == MINION_AI_TRIGGER_TAG)
        {
            Instantiate(minionDeathParticleEffect, this.transform.position, Quaternion.identity);

            Destroy(Other.gameObject);
            gameObject.SetActive(false);
        }
        else if (Other.gameObject.tag == BOSS_SHIELD_TRIGGER_TAG)
        {
            gameObject.SetActive(false);
        }
       
    }

    IEnumerator DelayedDestruction()
    {
        yield return new WaitForSeconds(TIMED_DEACTIVATION);

        gameObject.SetActive(false);
    }
}

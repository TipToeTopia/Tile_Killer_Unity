using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossTimeline : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector bossSequence;

    [SerializeField]
    private GameObject fakeBoss;

    const float BOSS_SEQUENCE_DURATION = 12.0f;

    private void Start()
    {
        PlaySequence();
    }

    // start cinematic sequence, at the end spawn gameplay boss and destroy cinematic boss

    public void PlaySequence()
    {
        bossSequence.Play();
        StartCoroutine(BossSequence());
    }

    private IEnumerator BossSequence()
    {
        yield return new WaitForSeconds(BOSS_SEQUENCE_DURATION);
        Destroy(fakeBoss);
        GameManager.Instance.SpawnBoss();
    }
}

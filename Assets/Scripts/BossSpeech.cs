using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpeech : MonoBehaviour
{
    private const float SPEECH_TIMER_ONE = 5.0f;
    private const float SPEECH_TIMER_TWO = 5.0f;

    [SerializeField]
    private GameObject speechBubble;

    void Start()
    {
        StartCoroutine(SpeechDelay());
    }

    // turn on, off boss speech

    private IEnumerator SpeechDelay()
    {
        yield return new WaitForSeconds(SPEECH_TIMER_ONE);
        speechBubble.SetActive(true);
        yield return new WaitForSeconds(SPEECH_TIMER_TWO);
        speechBubble.SetActive(false);
    }
}

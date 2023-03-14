using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // The audio manager will persist throughout the entire game, to play sound 
    // we use the PlaySound method and pass our audio clip through

    private static AudioManager instance = null;

    [SerializeField]
    private AudioSource effectsAudioSource;

    public static AudioManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void DestroyInstance()
    {
        Destroy(gameObject);
        instance = null;
    }

    public void PlaySound(AudioClip Clip)
    {
        effectsAudioSource.PlayOneShot(Clip);
    }
}

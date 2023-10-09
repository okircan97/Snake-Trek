using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public int poolSize = 20;
    private List<AudioSource> audioSources = new List<AudioSource>();


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            audioSources.Add(newSource);
        }
    }

    public void PlayClip(AudioClip clip, float volume = 1.0f, float pitch = 1.0f)
    {
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.volume = volume;
                source.pitch = pitch;
                source.Play();
                return;
            }
        }

        // If all sources are occupied and you still want to play another sound,
        // either grow the pool or ignore the request. Growing the pool is shown here:
        AudioSource newSource = gameObject.AddComponent<AudioSource>();
        newSource.clip = clip;
        newSource.volume = volume;
        newSource.pitch = pitch;
        newSource.Play();
        audioSources.Add(newSource);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Play the music
        audioSource.Play();
    }

    private void Update()
    {
        // Check if the music has finished playing and loop it
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}


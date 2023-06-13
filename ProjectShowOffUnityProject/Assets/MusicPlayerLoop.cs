using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayerLoop : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicStart;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        musicSource.PlayOneShot(musicStart);
        musicSource.PlayScheduled(AudioSettings.dspTime + musicStart.length);

    }

    // Update is called once per frame
    void Update()
    {

    }
}

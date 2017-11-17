using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public bool bgmOn = true;
    public AudioClip dayBgm, nightBgm;
    private AudioSource audioSource;


    void Start ()
    {
        audioSource = GetComponent<AudioSource>();	
	}
	
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.M))
        {
            PlayBgm(!bgmOn);
        }
	}

    public void PlayBgm(bool play)
    {
        if (play)
        {
            audioSource.Play();
            bgmOn = true;
        }
        else
        {
            audioSource.Stop();
            bgmOn = false;
        }
    }

    public void SwitchToDayBgm(bool isDay)
    {
        if (isDay)
        {
            audioSource.clip = dayBgm;
            if (bgmOn) audioSource.Play();
        }
        else
        {
            audioSource.clip = nightBgm;
            if (bgmOn) audioSource.Play();
        }
    }
}


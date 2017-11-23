using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to control the background music of the game.
/// </summary>
public class AudioController : MonoBehaviour
{
    public bool bgmOn = true;
    public AudioClip dayBgm, nightBgm;
    public AudioSource audioSource;

    private float bgmVolume = 1f;            // range 0-1.
    private float bgmVolumeModifier = 1.0f;  // range 0-1. where 1 is normal volume, 0.5 is half volume.


    void Update ()
    {
	    if (Input.GetButtonDown("ToggleMute"))
        {
            PlayBgm(!bgmOn);
        }

        // ModulateVolumeBasedOnEnemyDistance(CalculateClosestEnemy());
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

    public void SetBgmVolumeModifier(float mod)
    {
        Debug.Log("Setting bgm volume modifier: " + mod);
        bgmVolumeModifier = mod;
        audioSource.volume = bgmVolume * bgmVolumeModifier;
    }

    public void SetBgmVolume(float vol)
    {
        Debug.Log("Setting bgm volume: " + vol);
        bgmVolume = vol;
        audioSource.volume = bgmVolume * bgmVolumeModifier;
    }


    /* Use this code if modulating volume based off multiple enemies or when 3D sound can't be used

    public float maxVolume = 1f;            // maximum bgm volume. 
    public float minVolume = 0.1f;          // minimum bgm volume.
    public float maxRangeVolume = 20f;      // if any enemy is further than this, than minVolume is used.
    public float minRangeVolume = 1f;       // if any enemy is closer than this, than maxVolume is used.
    private GameObject[] wolves;
    private Transform player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        wolves = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private float CalculateClosestEnemy()
    {
        // if no wolves play at max volume
        if (wolves.Length == 0)
            return 0;

        // else find closest wolf 
        var closestDist = Mathf.Infinity;
        foreach (GameObject go in wolves)
        {
            var dist = Vector3.Distance(go.transform.position, player.position);
            if (dist < closestDist)
            {
                closestDist = dist;
            }
        }
        return closestDist;
    }

    private void ModulateVolumeBasedOnEnemyDistance(float closestDist)
    {
        var inversedPercent = (closestDist - minRangeVolume) / (maxRangeVolume - minRangeVolume);    // this is inversed (closer = quieter)
        var percent = Mathf.Clamp(1 - inversedPercent, 0, 1f);
        var volume = minVolume + (maxVolume - minVolume) * percent;
        SetBgmVolume(volume);
    }

    */
}


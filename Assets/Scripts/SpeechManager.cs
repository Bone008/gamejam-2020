using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SpeechManager : MonoBehaviour
{
    public static SpeechManager Instance { get; private set; }

    public AllLevelsData allData;
    private AudioSource source;

    void Awake()
    {
        if(Instance != null)
        {
            // We already have a persistent manager.
            Destroy(gameObject);
            return;
        }
        Instance = this;
        transform.SetParent(null, false);
        // Need to be kept alive to not cut off dialog across level reloads.
        DontDestroyOnLoad(gameObject);

        source = GetComponent<AudioSource>();
    }

    public void PlaySpecificClip(AudioClip clip)
    {
        if (clip != null)
        {
            // Override specific clips.
            if (source.isPlaying)
                source.Stop();
            source.PlayOneShot(clip);
        }
    }

    public void PlayOnDeath()
    {
        // Skip if already playing, not so important.
        if (source.isPlaying || allData.onDeathSpeeches.Length == 0) return;
        source.PlayOneShot(Util.PickRandomElement(allData.onDeathSpeeches));
    }

    public void PlayOnBrokenGrapple()
    {
        // Skip if already playing, not so important.
        if (source.isPlaying || allData.onBrokenGrappleSpeeches.Length == 0) return;
        source.PlayOneShot(Util.PickRandomElement(allData.onBrokenGrappleSpeeches));
    }

    public void PlayOnRandomVideo()
    {
        // Skip if already playing, not so important.
        if (source.isPlaying || allData.onRandomVideoSpeeches.Length == 0) return;
        source.PlayOneShot(Util.PickRandomElement(allData.onRandomVideoSpeeches));
    }

    public void StopSpeech()
    {
        source.Stop();
    }

}

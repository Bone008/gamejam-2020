using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{

    public GameObject panel;
    public VideoClip[] videos;
    public int[] lengths;
    private bool isPlaying = false;

    // Cat_eats_ham : 3
    // cat_in_jacket : 5
    // Kultivierte_Singles_Ü50 : 11
    // mario : 3
    // puppies : 8
    // sad_banana : 5
    // technologie : 6
    // trump : 5

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        if(!videos.Length.Equals(lengths.Length))
        {
            Debug.LogError("[PlayVideo] videos and lengths have to be same length.");
            this.enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // for testing purposes
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (!isPlaying)
            {
                Play();
            }
        }
    }

    IEnumerator CoFunc(int delay)
    {
        isPlaying = true;
        panel.SetActive(true);
        // manually change depending on video length
        yield return new WaitForSeconds(delay);
        panel.SetActive(false);
        isPlaying = false;
    }

    public void Play()
    {
        if (isPlaying) return;
        int rnd = (int)UnityEngine.Random.Range(0, videos.Length);
        panel.GetComponentInChildren<VideoPlayer>().clip = videos[rnd];
        StartCoroutine(CoFunc(lengths[rnd]));
        
    }
}

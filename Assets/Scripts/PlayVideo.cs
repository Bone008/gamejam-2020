using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public float videoProbability = 0f;
    public GameObject panel;
    public RectTransform arFrame;
    public VideoClip[] videos;
    public string[] videoNames;
    public int[] lengths;
    private bool isPlaying = false;
    private VideoPlayer videoPlayer;

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
        videoPlayer = panel.GetComponentInChildren<VideoPlayer>();

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
        if(isPlaying)
        {
            // Just floaty things.
            float progress = (float)(videoPlayer.time / videoPlayer.clip.length);
            panel.transform.localPosition = Mathf.Lerp(-50f, 50f, progress) * Vector2.right;
        }

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

        // Play speech after video has finished playing.
        SpeechManager.Instance.PlayOnRandomVideo();
    }

    public void Play()
    {
        if (isPlaying) return;
        int rnd = (int)UnityEngine.Random.Range(0, videos.Length);
        
        videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, videoNames[rnd]);
        VideoClip clip = videos[rnd];
        videoPlayer.clip = clip;
        Debug.Log(clip.width + "x" + clip.height);
        arFrame.sizeDelta = new Vector2(20, arFrame.rect.size.x / clip.width * clip.height + 20);
        StartCoroutine(CoFunc(lengths[rnd]));
    }

    public void PlayIfUnlucky()
    {
        if (Util.DoWeHaveBadLuck(videoProbability))
            Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TrollEnding : MonoBehaviour
{
    public VideoPlayer video;
    public AudioClip trollSpeech;
    public GameObject exit;
    public GameObject credits;

    private bool didTheTrollyTroll = false;

    void Update()
    {
        if(!didTheTrollyTroll && Input.GetButtonDown("Interact"))
        {
            didTheTrollyTroll = true;
            StartCoroutine(DoTheUltimateTroll());
        }
    }

    private IEnumerator DoTheUltimateTroll()
    {
        video.url = System.IO.Path.Combine(Application.streamingAssetsPath, "rickrolled.mp4");
        video.Play();
        yield return new WaitForSeconds(5f);
        video.SetDirectAudioVolume(0, 0.8f);
        yield return new WaitForSeconds(0.3f);
        video.SetDirectAudioVolume(0, 0.5f);
        yield return new WaitForSeconds(0.3f);
        video.SetDirectAudioVolume(0, 0.35f);
        yield return new WaitForSeconds(0.3f);
        video.SetDirectAudioVolume(0, 0.2f);

        SpeechManager.Instance.PlaySpecificClip(trollSpeech);
        yield return new WaitForSeconds(trollSpeech.length);


        video.SetDirectAudioVolume(0, 0.35f);
        yield return new WaitForSeconds(0.3f);
        video.SetDirectAudioVolume(0, 0.5f);
        yield return new WaitForSeconds(0.3f);
        video.SetDirectAudioVolume(0, 0.8f);

        exit.SetActive(true);
        credits.SetActive(true);
    }
}

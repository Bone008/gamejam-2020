using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Die : MonoBehaviour
{
    public float delayAfterAudio;
    public List<AudioClip> dieAudio;
    public AudioSource audioSource;

    void Start()
    {
    }

    IEnumerator OnTriggerEnter(Collider info)
    {
        if (info.gameObject.name == "Player")
        {
            int idx = (int)(Random.value * dieAudio.Count);
            audioSource.PlayOneShot(dieAudio[idx]);
            yield return new WaitForSeconds(dieAudio[idx].length); // Wait for the audio to have finished
            SpeechManager.Instance.PlayOnDeath();
            yield return new WaitForSeconds(delayAfterAudio);
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }
}

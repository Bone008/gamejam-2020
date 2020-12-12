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
            yield return new WaitForSeconds(dieAudio[idx].length + delayAfterAudio); // Wait for the audio to have finished
            Scene scene = SceneManager.GetActiveScene(); 
            SceneManager.LoadScene(scene.name);
        }
    }
}

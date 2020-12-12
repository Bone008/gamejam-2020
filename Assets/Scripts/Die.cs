using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Die : MonoBehaviour
{
    public float delayAfterAudio;
    public string activscene;

    public AudioSource audioSource;
    public AudioClip dieAudio;

    void Start()
    {
        activscene = SceneManager.GetActiveScene().name;
    }

    IEnumerator OnTriggerEnter(Collider info)
    {
        if (info.gameObject.name == "Player")
        {
            audioSource.PlayOneShot(dieAudio);
            yield return new WaitForSeconds(dieAudio.length + delayAfterAudio); // Wait for the audio to have finished
            SceneManager.LoadScene(activscene, LoadSceneMode.Single);
        }
    }
}

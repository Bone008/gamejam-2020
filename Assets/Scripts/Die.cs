using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Die : MonoBehaviour
{

  
    public float delayAfterAudio;

    IEnumerator OnTriggerEnter(Collider info)
    {
        if (info.gameObject.name == "Player")
        {
            Debug.Log("Collision Detected");
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length + delayAfterAudio); // Wait for the audio to have finished
            SceneManager.LoadScene("TestLevel01", LoadSceneMode.Single);
            
        }
    }
}

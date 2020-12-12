using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//[RequireComponent(typeof(AudioSource))]

public class TriggerDoor : MonoBehaviour

{
    IEnumerator OnTriggerEnter(Collider doorInformation)
    {
        if (doorInformation.gameObject.name == "Player")
        {
            Debug.Log("Collision Detected");
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length); // Wait for the audio to have finished
            SceneManager.LoadScene("TestLevel02", LoadSceneMode.Single); 
        }
    }
}

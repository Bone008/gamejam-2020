using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

[RequireComponent(typeof(AudioSource))]
public class DoorExit : MonoBehaviour
{
    public float delayAfterAudio;

    IEnumerator OnTriggerEnter(Collider doorInformation)
    {
        if (doorInformation.gameObject.name == "Player")
        {
            Debug.Log("Collision Detected");
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            yield return new WaitForSeconds(audio.clip.length);

            var levelManager = LevelManager.FindActiveInstance();
            levelManager.OnPlayerWinLevel(out float extraWaitTime);
            yield return new WaitForSeconds(extraWaitTime + delayAfterAudio);
            levelManager.LoadNextLevel();
        }
    }
}

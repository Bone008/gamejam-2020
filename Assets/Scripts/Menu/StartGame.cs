using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [Scene]
    public string firstSceneName;

    public void Activate()
    {
        SceneManager.LoadScene(firstSceneName, LoadSceneMode.Single);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFastForward : MonoBehaviour
{
    public float skipHoldDuration = 1.2f;
    private float skipTimer = 0f;
    private float prevTimer = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Time.timeScale = 5f;
        }
        else if (Input.GetKeyUp(KeyCode.F3))
        {
            Time.timeScale = 1f;
        }

        // Skip to next level.
        if (Input.GetKey(KeyCode.F10))
            skipTimer += Time.deltaTime;
        else
            skipTimer = 0f;
        if (skipTimer >= skipHoldDuration)
        {
            skipTimer = 0f;
            LevelManager.FindActiveInstance().LoadNextLevel();
        }

        // Go back to previous level.
        if (Input.GetKey(KeyCode.F9))
            prevTimer += Time.deltaTime;
        else
            prevTimer = 0f;
        if (prevTimer >= skipHoldDuration)
        {
            prevTimer = 0f;
            LevelManager.FindActiveInstance().LoadNextLevel(true);
        }
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}

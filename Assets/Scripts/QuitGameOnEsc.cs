using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameOnEsc : MonoBehaviour
{
    public float holdDuration = 1.5f;
    public GameObject indicatorObject;

    private float timer = 0f;

    void Update()
    {
        bool isPressed = Input.GetKey(KeyCode.Escape);
        indicatorObject?.SetActive(isPressed);
        if (isPressed)
            timer += Time.deltaTime;
        else
            timer = 0f;

        if(timer >= holdDuration)
        {
            LevelManager.ExitToMainMenu();
        }
    }
}

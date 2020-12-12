using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugFastForward : MonoBehaviour
{
    void Start()
    {
        
    }

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
    }
}

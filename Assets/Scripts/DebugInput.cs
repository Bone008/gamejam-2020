using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Used to help testing functionality with debug keys. The actions can be set from the editor.
/// </summary>
public class DebugInput : MonoBehaviour
{
    public KeyCode key;
    public UnityEvent actions;

    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            actions.Invoke();
        }
    }
}

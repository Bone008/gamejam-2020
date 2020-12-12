using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLever : MonoBehaviour
{
    public MechanismBase targetMechanism;
    public MechanismBase leverHandleMechanism;

    public bool isOn = false;

    void Start()
    {
        // Toggle once in the beginning if the lever should start in the on state.
        if (isOn)
        {
            isOn = false;
            Activate();
        }
    }

    public void Activate()
    {
        isOn = !isOn;
        targetMechanism?.SetTriggerActive(isOn);
        leverHandleMechanism.SetTriggerActive(isOn);
    }
}

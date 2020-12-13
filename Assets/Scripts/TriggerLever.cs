using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLever : MonoBehaviour
{
    public MechanismBase targetMechanism;
    public MechanismBase leverHandleMechanism;
    public AudioClip audioOn;
    public AudioClip audioOff;
    public bool isOn = false;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Toggle once (silently) in the beginning if the lever should start in the on state.
        if (isOn)
        {
            targetMechanism?.SetTriggerActive(isOn);
            leverHandleMechanism.SetTriggerActive(isOn);
        }
    }

    public void Activate()
    {
        isOn = !isOn;
        targetMechanism?.SetTriggerActive(isOn);
        leverHandleMechanism.SetTriggerActive(isOn);
        audioSource.PlayOneShot(isOn ? audioOn : audioOff);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerButton : MonoBehaviour
{
    public MechanismBase targetMechanism;
    public float activationDuration = 2f;
    public MechanismBase buttonFaceMechanism;

    private Coroutine resetCoroutine = null;

    public void Activate()
    {
        Debug.Log($"Activating button for {activationDuration:0.#} s!", this);
        targetMechanism?.SetTriggerActive(true);
        buttonFaceMechanism.SetTriggerActive(true);
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(DoReset());
    }

    private IEnumerator DoReset()
    {
        yield return new WaitForSeconds(activationDuration);
        Debug.Log("Deactivating button!", this);
        targetMechanism?.SetTriggerActive(false);
        buttonFaceMechanism.SetTriggerActive(false);
    }
}

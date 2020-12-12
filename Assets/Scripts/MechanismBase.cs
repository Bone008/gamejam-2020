using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class that represents any mechanism that can be toggled on/off.
/// Its effect may do different things, which is done in child classes.
/// </summary>
public abstract class MechanismBase : MonoBehaviour
{
    /// <summary>Do NOT change from other scripts directly! Only public so the initial value can be set from the Unity Editor.</summary>
    public bool isTriggerActive;

    // Use these as actions of buttons/levers to connect them to this mechanism.
    public void SetTriggerActive(bool value)
    {
        if (value == isTriggerActive)
            return;
        isTriggerActive = value;
        OnIsTriggeredChanged();
    }

    public void ToggleTriggerActive()
    {
        SetTriggerActive(!isTriggerActive);
    }

    /// <summary>Override to react to changes to isTriggered.</summary>
    protected virtual void OnIsTriggeredChanged() { }
}

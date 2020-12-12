using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows grouping multiple mechanisms into a common parent that can be linked to buttons.
/// All children of this game object are automatically toggled together with this mechanism.
/// </summary>
public class MechanismGroup : MechanismBase
{
    private MechanismBase[] children;

    void Awake()
    {
        children = GetComponentsInChildren<MechanismBase>();
    }

    protected override void OnIsTriggeredChanged()
    {
        foreach(var child in children)
        {
            child.SetTriggerActive(isTriggerActive);
        }
    }
}

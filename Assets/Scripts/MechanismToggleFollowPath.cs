using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanismToggleFollowPath : MechanismBase
{
    public FollowPath targetScript;

    protected override void OnIsTriggeredChanged()
    {
        targetScript.isFollowEnabled = isTriggerActive;
    }
}

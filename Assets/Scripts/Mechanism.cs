using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanism : MonoBehaviour
{
    public bool triggerActive;
    public float transitionDelay = 0f;
    public float transitionDuration = 1.0f;

    /// <summary>
    /// Progress of the transition in seconds.
    /// 0 <=> fully disabled; transitionDelay + transitionDuration <=> fully enabled.
    /// </summary>
    private float transitionProgress;

    public MechanismTransform mechTransform;

    void Start()
    {
        // Initially set progress to maximum/minimum depending on whether triggerActive is checked in the Editor.
        transitionProgress = (triggerActive ? GetTotalDuration() : 0f);
        SetTriggerActive(triggerActive);
    }

    void Update()
    {
        float transitionTarget = (triggerActive ? GetTotalDuration() : 0f);
        if(transitionProgress != transitionTarget)
        {
            transitionProgress = Mathf.MoveTowards(transitionProgress, transitionTarget, Time.deltaTime);
            // Rescale progress to a percentage between 0 and 1, accounting for initial delay.
            float percentage = Mathf.Max(0, (transitionProgress - transitionDelay) / transitionDuration);
            UpdateTransition(percentage);
        }
    }

    private float GetTotalDuration()
    {
        return transitionDelay + transitionDuration;
    }

    private void UpdateTransition(float percentage)
    {
        mechTransform.UpdateTransition(percentage);
    }

    public void SetTriggerActive(bool value)
    {
        triggerActive = value;
    }

    public void ToggleTriggerActive()
    {
        SetTriggerActive(!triggerActive);
    }
}

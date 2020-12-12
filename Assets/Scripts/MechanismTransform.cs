using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanismTransform : MechanismBase
{
    public float transitionDelay = 0f;
    public float transitionDuration = 1.0f;

    [Tooltip("Which transform should be moved. If null, the object which this script is attached to is used.")]
    public Transform targetTransform;
    public Vector3 deltaTranslation = Vector3.zero;
    public Vector3 deltaRotation = Vector3.zero;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    private Vector3 initialPosition;
    private Vector3 initialEulerAngles;

    /// <summary>
    /// Progress of the transition in seconds.
    /// 0 <=> fully disabled; transitionDelay + transitionDuration <=> fully enabled.
    /// </summary>
    private float transitionProgress;

    void Start()
    {
        if (targetTransform == null)
            targetTransform = transform;
        initialPosition = targetTransform.localPosition;
        initialEulerAngles = targetTransform.localEulerAngles;

        // Initially set progress to maximum/minimum depending on whether isTriggered is checked in the Editor.
        transitionProgress = (isTriggerActive ? GetTotalDuration() : 0f);
    }

    void Update()
    {
        float transitionTarget = (isTriggerActive ? GetTotalDuration() : 0f);
        if (transitionProgress != transitionTarget)
        {
            transitionProgress = Mathf.MoveTowards(transitionProgress, transitionTarget, Time.deltaTime);
            // Rescale progress to a percentage between 0 and 1, accounting for initial delay.
            float percentage = Mathf.Max(0, (transitionProgress - transitionDelay) / transitionDuration);
            UpdateTransition(percentage);
        }
    }

    /// <summary>Called whenever the mechanism is changing between on/off.</summary>
    /// <param name="percentage">Value between 0 and 1 indicating the transition progress. Last call will be with exactly the value 1 or 0.</param>
    private void UpdateTransition(float percentage)
    {
        float value = curve.Evaluate(percentage);
        targetTransform.localPosition = initialPosition + value * deltaTranslation;
        targetTransform.localEulerAngles = initialEulerAngles + value * deltaRotation;
    }

    private float GetTotalDuration()
    {
        return transitionDelay + transitionDuration;
    }

    private void OnDrawGizmosSelected()
    {
        if (deltaTranslation != Vector3.zero)
        {
            Gizmos.DrawLine(transform.position, transform.position + deltaTranslation);
            Gizmos.DrawWireCube(transform.position + deltaTranslation, 0.2f * Vector3.one);
        }
    }
}

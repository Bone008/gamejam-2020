using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanismTransform : MechanismBase
{
    public float transitionDelay = 0f;
    public float transitionDuration = 1.0f;

    [Tooltip("Which transform should be moved. If null, the object which this script is attached to is used.")]
    public Transform targetTransform;
    [Tooltip("Whether to move using transform or rigidbody. This has to be OFF for all platforms that attach the player!")]
    public bool moveUsingRigidbody = false;
    public Vector3 deltaTranslation = Vector3.zero;
    public Vector3 deltaRotation = Vector3.zero;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);

    private Rigidbody targetRigidbody;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    /// <summary>
    /// Progress of the transition in seconds.
    /// 0 <=> fully disabled; transitionDelay + transitionDuration <=> fully enabled.
    /// </summary>
    private float transitionProgress;

    void Awake()
    {
        if (targetTransform == null)
            targetTransform = transform;
        initialPosition = targetTransform.position;
        initialRotation = targetTransform.rotation;

        // Optionally move by rigidbody.
        if (moveUsingRigidbody)
        {
            targetRigidbody = targetTransform.GetComponent<Rigidbody>();
            if (targetRigidbody == null)
                targetRigidbody = targetTransform.gameObject.AddComponent<Rigidbody>();
            targetRigidbody.isKinematic = true;
        }

        // Initially set progress to maximum/minimum depending on whether isTriggered is checked in the Editor.
        transitionProgress = (isTriggerActive ? GetTotalDuration() : 0f);
    }

    void FixedUpdate()
    {
        if (moveUsingRigidbody) DoUpdate(Time.fixedDeltaTime);
    }
    void Update()
    {
        if (!moveUsingRigidbody) DoUpdate(Time.deltaTime);
    }

    private void DoUpdate(float deltaT)
    {
        float transitionTarget = (isTriggerActive ? GetTotalDuration() : 0f);
        if (transitionProgress != transitionTarget)
        {
            transitionProgress = Mathf.MoveTowards(transitionProgress, transitionTarget, deltaT);
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
        Vector3 newPos = initialPosition + value * (initialRotation * deltaTranslation);
        Quaternion newRot = Quaternion.Lerp(initialRotation, initialRotation * Quaternion.Euler(deltaRotation), value);

        if (moveUsingRigidbody)
        {
            targetRigidbody.MovePosition(newPos);
            targetRigidbody.MoveRotation(newRot);
        }
        else
        {
            targetTransform.position = newPos;
            targetTransform.rotation = newRot;
        }
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

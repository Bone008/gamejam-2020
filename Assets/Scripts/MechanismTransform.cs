using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechanismTransform : MonoBehaviour
{
    public Transform targetTransform;
    [System.ComponentModel.Description("test")]
    public Vector3 deltaTranslation = Vector3.zero;
    public Vector3 deltaRotation = Vector3.zero;

    private Vector3 initialPosition;
    private Vector3 initialEulerAngles;

    void Start()
    {
        initialPosition = targetTransform.localPosition;
        initialEulerAngles = targetTransform.localEulerAngles;
    }

    public void UpdateTransition(float percentage)
    {
        targetTransform.localPosition = initialPosition + percentage * deltaTranslation;
        targetTransform.localEulerAngles = initialEulerAngles + percentage * deltaRotation;
    }
}

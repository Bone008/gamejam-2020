using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachStuffOnEnter : MonoBehaviour
{
    public BoxCollider platformCollider;
    public float colliderPadding = 0.2f;
    private BoxCollider triggerCollider;
    private Dictionary<int, Transform> originalParentsById = new Dictionary<int, Transform>();

    void Start()
    {
        // Sanity check so we don't break physics.
        //float scaleMin = Mathf.Min(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        //float scaleMax = Mathf.Max(transform.lossyScale.x, transform.lossyScale.y, transform.lossyScale.z);
        //if (Mathf.Abs(scaleMax - scaleMin) > Mathf.Epsilon)
        if (transform.lossyScale != Vector3.one)
        {
            Debug.LogError("[AttachStuffOnEnter] Detected a rescaled platform anchor, this does bad things to physics and our calculations! "
                + "Please do not rescale the entire platform, only the 'Platform' game object!", this);
            gameObject.SetActive(false);
            return;
        }

        // Adjust our own trigger collider to the world-space bounding box of the platform,
        // while retaining our own Y component. Also add some padding because otherwise things
        // that the platform bumps into from the side are picked up too.
        triggerCollider = GetComponent<BoxCollider>();
        Vector3 triggerSize = platformCollider.bounds.size;
        triggerSize.x -= 2 * colliderPadding;
        triggerSize.y = triggerCollider.size.y;
        triggerSize.z -= 2 * colliderPadding;
        triggerCollider.size = triggerSize;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            if (other.transform.parent && other.transform.parent.TryGetComponent(out AttachStuffOnEnter otherAttacher))
            {
                // Artificially trigger exit of other collider that the object snatched up to reset its original parent.
                // This can still break if there is some OTHER script that re-parents stuff, but we don't have that atm soo ...
                otherAttacher.OnTriggerExit(other);
            }

            originalParentsById.Add(other.transform.GetInstanceID(), other.transform.parent);
            other.transform.SetParent(transform, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (originalParentsById.TryGetValue(other.transform.GetInstanceID(), out var origParent))
        {
            if (other.transform.parent != transform)
            {
                Debug.Log("[AttachStuffOnEnter] Someone else snatched our passenger away :O");
                originalParentsById.Remove(other.transform.GetInstanceID());
                return;
            }
            other.transform.SetParent(origParent, true);
            originalParentsById.Remove(other.transform.GetInstanceID());
        }
    }


    // Note: The following does not work and raises the error:
    // "Cannot set the parent of the GameObject 'Player' while activating or deactivating the parent GameObject 'PlatformAnchor'.
    // But it's only necessaryin case we want to disable platforms anyway in the future ...
    void OnDisable()
    {
        foreach (Transform potentialPassenger in transform)
        {
            if (originalParentsById.TryGetValue(potentialPassenger.GetInstanceID(), out var origParent))
            {
                Debug.Log("[AttachStuffOnEnter] Releasing passenger on platform disable.", potentialPassenger);
                potentialPassenger.SetParent(origParent, true);
            }
        }
        originalParentsById.Clear();
    }
}

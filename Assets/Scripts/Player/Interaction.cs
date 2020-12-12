using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public LayerMask interactableLayer;
    public float interactionRange;
    public float infoCardRange;

    public InfoCardController infoCard;

    private GameObject currentTarget = null;
    private bool wasInInteractionRange = false;
    private bool inInteractionRange = false;

    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, infoCardRange, interactableLayer, QueryTriggerInteraction.Collide))
        {
            float distSq = (hit.transform.position - transform.position).sqrMagnitude;
            inInteractionRange = distSq * distSq <= interactionRange * interactionRange;
            SetTarget(hit.collider.gameObject);
        }
        else
        {
            SetTarget(null);
        }

        if (currentTarget != null)
        {
            if(wasInInteractionRange != inInteractionRange)
            {
                infoCard.SetTarget(currentTarget, inInteractionRange);
                wasInInteractionRange = inInteractionRange;
            }
            if (inInteractionRange && Input.GetButtonDown("Interact"))
            {
                currentTarget.SendMessage("Activate");
                // Set target again to potentially update text.
                infoCard.SetTarget(currentTarget, inInteractionRange);
            }
        }
    }

    private void SetTarget(GameObject newTarget)
    {
        if (currentTarget == newTarget) return;
        currentTarget = newTarget;
        Debug.Log("Now targeting: " + currentTarget, this);

        infoCard.SetTarget(currentTarget, inInteractionRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public LayerMask interactableLayer;
    public float interactionRange;

    private GameObject currentTarget = null;

    void Update()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactionRange, interactableLayer, QueryTriggerInteraction.Collide))
        {
            SetTarget(hit.collider.gameObject);
        }
        else
        {
            SetTarget(null);
        }

        if(Input.GetButtonDown("Interact") && currentTarget != null)
        {
            currentTarget.SendMessage("Activate");
        }
    }

    private void SetTarget(GameObject newTarget)
    {
        if (currentTarget == newTarget) return;
        currentTarget = newTarget;
        Debug.Log("Now targeting: " + currentTarget, this);
        // TODO: Highlight what is being targeted somehow.
    }
}

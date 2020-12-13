using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoCardController : MonoBehaviour
{
    public float showTime = 0.4f;
    public RectTransform connectorLine;
    public RectTransform infoPanel;
    public Text textLine1;
    public Text textLine2;

    private bool isOpen = false;
    private Transform targetTransform = null;
    private Vector2 infoPanelInitialPos;
    private float shownPercentage = 0f;

    void Start()
    {
        infoPanelInitialPos = infoPanel.localPosition;
    }

    public void SetTarget(GameObject target, bool inInteractionRange)
    {
        if (target == null)
        {
            // Do not set targetTransform to null, so we can still follow
            // the object while we do the close animation.
            isOpen = false;
        }
        else
        {
            isOpen = true;
            targetTransform = target.transform;
            UpdateText(target, inInteractionRange);
        }
    }

    void LateUpdate()
    {
        shownPercentage = Mathf.MoveTowards(shownPercentage, isOpen ? 1f : 0f, Time.deltaTime / showTime);
        connectorLine.gameObject.SetActive(shownPercentage > 0f);
        infoPanel.gameObject.SetActive(shownPercentage > 0f);

        if (shownPercentage == 0f) return;

        // Update UI animation.
        Vector2 lineStartPos;
        if (targetTransform)
        {
            Vector3 screenPos3D = Camera.main.WorldToScreenPoint(targetTransform.position);
            // Convert to local space for some reason.
            lineStartPos = transform.InverseTransformPoint(screenPos3D);
        }
        else
        {
            // Use middle of the screen as fallback
            lineStartPos = transform.InverseTransformPoint(Camera.main.ViewportToScreenPoint(0.5f * Vector2.one));
        }

        // Drag infoPanel a bit towards the middle for a cool effect.
        infoPanel.localPosition = Vector2.Lerp(lineStartPos, infoPanelInitialPos, 0.6f);
        // Animate panel width in second 50% of animation.
        float panelProgress = Mathf.Clamp01(shownPercentage * 2f - 1f);
        infoPanel.localScale = new Vector3(panelProgress, 1, 1);

        // Animate line in first 50% of animation.
        Vector2 lineEndPos = infoPanel.localPosition;
        float lineProgress = Mathf.Clamp01(shownPercentage * 2f);
        SetLineFromTo(lineStartPos, Vector2.Lerp(lineStartPos, lineEndPos, lineProgress));
    }

    // All in local coordinates.
    private void SetLineFromTo(Vector2 lineStartPos, Vector2 lineEndPos)
    {
        connectorLine.localPosition = (lineStartPos + lineEndPos) / 2;
        Vector2 dif = lineEndPos - lineStartPos;
        if (dif.x < 0) dif = -dif;
        connectorLine.sizeDelta = new Vector3(dif.magnitude, 5);
        connectorLine.eulerAngles = new Vector3(0, 0, Mathf.Atan(dif.y / dif.x) * Mathf.Rad2Deg);
    }

    private void UpdateText(GameObject target, bool inInteractionRange)
    {
        if (target.TryGetComponent(out FollowPath followPath))
        {
            textLine1.text = "Platform";
            textLine2.text = followPath.isFollowEnabled
                ? $"moving at {followPath.velocity:0.0} m/s"
                : "disabled";
        }
        else if (target.TryGetComponent(out TriggerButton button))
        {
            textLine1.text = "Button";
            textLine2.text = inInteractionRange ? "Press F to activate." : "Step closer to press.";
        }
        else if (target.TryGetComponent(out TriggerLever lever))
        {
            textLine1.text = "Switch";
            textLine2.text = inInteractionRange ? $"Press F to {(lever.isOn ? "de" : "")}activate." : "Step closer to press.";
        }
        else if (target.TryGetComponent(out TriggerDoor door))
        {
            textLine1.text = "Exit door";
            textLine2.text = inInteractionRange ? "Just go inside already ..." : "This is where you need to go.";
        }
        else if (target.layer == LayerMask.NameToLayer("Anchors"))
        {
            textLine1.text = "Hook anchor";
            textLine2.text = "Shoot here and grapple your way to a better life!";
        }
        else if (target.transform.parent)
        {
            // Unidentified? Try same thing with parent.
            UpdateText(target.transform.parent.gameObject, inInteractionRange);
        }
        else
        {
            textLine1.text = "Object";
            textLine2.text = "???";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCardController : MonoBehaviour
{
    public Transform targetTransform;
    public RectTransform connectorLine;
    public RectTransform infoPanel;

    private Vector2 infoPanelInitialPos;

    void Start()
    {
        infoPanelInitialPos = infoPanel.localPosition;
    }

    void LateUpdate()
    {
        Vector2 screenSize = Camera.main.ViewportToScreenPoint(Vector2.one);
        Vector3 screenPos3D = Camera.main.WorldToScreenPoint(targetTransform.position);
        // Convert to local space for some reason.
        Vector2 lineStartPos = transform.InverseTransformPoint(screenPos3D);

        // Drag infoPanel a bit towards the middle for a cool effect.
        infoPanel.localPosition = Vector2.Lerp(lineStartPos, infoPanelInitialPos, 0.8f);

        Vector2 lineEndPos = infoPanel.localPosition;
        //lineStartPos = infoPanel2.localPosition;
        //lineStartPos = lineEndPos + new Vector2(200, 0);
        connectorLine.localPosition = (lineStartPos + lineEndPos) / 2;
        Vector2 dif = lineEndPos - lineStartPos;
        if (dif.x < 0) dif = -dif;
        connectorLine.sizeDelta = new Vector3(dif.magnitude, 5);
        connectorLine.eulerAngles = new Vector3(0, 0, Mathf.Atan(dif.y / dif.x) * Mathf.Rad2Deg);
    }
}

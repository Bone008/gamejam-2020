using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPath : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform waypointsRoot;

    public GameObject anchor;

    void Start()
    {
        waypointsRoot = GetComponent<Transform>();
        bool isLooping = anchor.GetComponent<FollowPath>().isLooping;
        if (waypointsRoot?.childCount < 2)
        {
            Debug.LogError("[RenderPath] Need at least 2 waypoints to form a path. Please attach empty GameObjects as children of the 'Path' object.", waypointsRoot);
            this.enabled = false;
            return;
        }
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = waypointsRoot.childCount;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.cyan;
        lineRenderer.startWidth = .1f;
        lineRenderer.endWidth = .1f;
        int i = 0;
        foreach (Transform waypoint in waypointsRoot.transform)
        {
            lineRenderer.SetPosition(i, waypoint.position);
            i += 1;
        }
        if (isLooping)
        {
            lineRenderer.loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for platforms to automatically follow a given path.
/// The path is specified by providing a GameObject with empty children whose positions indicate the waypoints.
/// </summary>
public class FollowPath : MonoBehaviour
{
    [Tooltip("IMPORTANT: Waypoints may NOT be children of this script, otherwise they would be moved along with the platform and break.")]
    public Transform waypointsRoot;
    public float velocity = 5;
    public bool isLooping = false;

    private int currentIndex = -1;
    private Transform currentWaypoint;
    private bool isGoingBackward = false;

    void Start()
    {
        NextWaypoint();
    }

    void Update()
    {
        Vector3 currentGoal = currentWaypoint.position;
        transform.position = Vector3.MoveTowards(transform.position, currentGoal, Time.deltaTime * velocity);

        if (transform.position == currentGoal)
        {
            NextWaypoint();
        }
    }

    private void NextWaypoint()
    {
        // Check if we reached end of path and change direction / roll over accordingly.
        if (!isGoingBackward && currentIndex >= waypointsRoot.transform.childCount - 1)
        {
            if (isLooping) currentIndex = -1;
            else isGoingBackward = true;
        }
        else if (isGoingBackward && currentIndex <= 0)
        {
            isGoingBackward = false;
        }

        currentIndex += (isGoingBackward ? -1 : 1);
        currentWaypoint = waypointsRoot.transform.GetChild(currentIndex);
    }

    private void OnDrawGizmos()
    {
        if (waypointsRoot == null) return;
        Vector3 prev = waypointsRoot.GetChild(0).position;
        foreach (Transform waypoint in waypointsRoot.transform)
        {
            Gizmos.DrawLine(prev, waypoint.position);
            Gizmos.DrawSphere(waypoint.position, 0.2f);
            prev = waypoint.position;
        }
        if (isLooping)
        {
            Gizmos.DrawLine(prev, waypointsRoot.GetChild(0).position);
        }
    }
}

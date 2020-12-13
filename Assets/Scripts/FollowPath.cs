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
    [Tooltip("IMPORTANT: Waypoints may NOT be children of the FollowPath script, otherwise they would be moved along with the platform and break.")]
    public Transform waypointsRoot;
    public float velocity = 5;
    public bool isLooping = false;
    public bool initiallyTeleportToFirstWaypoint = true;
    public bool isTurning = false;
    public bool isFollowEnabled = true;

    private int currentIndex = -1;
    private Transform currentWaypoint;
    private bool isGoingBackward = false;

    //private Rigidbody rb;

    void Start()
    {
        if (this.name.Equals("3Arrows"))
        {
            isTurning = true;
        }

        if (waypointsRoot?.childCount < 2)
        {
            Debug.LogError("[FollowPath] Need at least 2 waypoints to form a path. Please attach empty GameObjects as children of the 'Path' object.", waypointsRoot);
            this.enabled = false;
            return;
        }
        //rb = gameObject.AddComponent<Rigidbody>();
        //rb.isKinematic = true;
        NextWaypoint();

        if (initiallyTeleportToFirstWaypoint)
        {
            transform.position = currentWaypoint.position;
        }
    }

    void FixedUpdate()
    {
        if (!isFollowEnabled) return;

        Vector3 currentGoal = currentWaypoint.position;
        transform.position = Vector3.MoveTowards(transform.position, currentGoal, Time.fixedDeltaTime * velocity);

        if (isTurning)
        {
            Vector3 direction = currentGoal - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(direction);
        }


        // We might do this instead to play nicer with the physics engine.
        // On its own, the player still wiggles around a bit while standing on a platform and falls off.
        // Instead, the script "AttachStuffOnEnter" now takes care of parenting the player to the platform.
        //rb.MovePosition(Vector3.MoveTowards(transform.position, currentGoal, Time.fixedDeltaTime * velocity));

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
        if (waypointsRoot == null || waypointsRoot.childCount < 2) return;
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

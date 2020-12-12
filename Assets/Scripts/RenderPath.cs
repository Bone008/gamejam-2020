using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderPath : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform waypointsRoot;
    private float length;
    private Vector3[] positions;
    private Vector3 direction;
    private Vector3 position;
    private Quaternion rotation;
    private int num = 0;

    public GameObject anchor;
    public GameObject arrow;
    private GameObject SpawnArrow;
    //public int coverage = 1;
    public float spawnDistance = 3f;

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

        positions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(positions);
        // get length of path
        length = CalculateLength(positions);
        num = (int)((length / spawnDistance) - 1);
        direction = positions[1] - positions[0];
        position = waypointsRoot.GetChild(0).transform.position;
        rotation = Quaternion.LookRotation(direction);
        SpawnArrow = (GameObject)Instantiate(arrow, position, rotation);
        SpawnArrow.GetComponent<FollowPath>().waypointsRoot = this.waypointsRoot;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (SpawnArrow.transform.position - position).magnitude; //Vector3.Distance(position, SpawnArrow.transform.position);
        if (distance > spawnDistance && num > 0)
        {
            SpawnArrow = (GameObject)Instantiate(arrow, position, rotation);
            SpawnArrow.GetComponent<FollowPath>().waypointsRoot = this.waypointsRoot;
            num -= 1;
        }
    }

    float CalculateLength(Vector3[] positions)
    {
        bool isLooping = anchor.GetComponent<FollowPath>().isLooping;
        float length = 0.0f;
        Vector3 prev = positions[0];
        for (int i = 1; i < positions.Length; i++)
        {
            length += (positions[i] - prev).magnitude;
            prev = positions[i];
        }
        if (isLooping)
        {
            length += (positions[0] - prev).magnitude;
        }
        return length;
    }
}

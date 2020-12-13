using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
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
    private bool isLooping = false;

    private LevelManager levelManager;

    public GameObject anchor;
    public GameObject arrow;
    private GameObject lastSpawnedArrow;
    private List<Renderer> allArrowRenderers = new List<Renderer>();
    //public int coverage = 1;
    public float spawnDistance = 3f;

    void Start()
    {
        levelManager = LevelManager.FindActiveInstance();

        waypointsRoot = GetComponent<Transform>();
        isLooping = anchor.GetComponent<FollowPath>().isLooping;
        if (waypointsRoot?.childCount < 2)
        {
            Debug.LogError("[RenderPath] Need at least 2 waypoints to form a path. Please attach empty GameObjects as children of the 'Path' object.", waypointsRoot);
            this.enabled = false;
            return;
        }
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = waypointsRoot.childCount;
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
        SpawnNewArrow();
    }

    // Update is called once per frame
    void Update()
    {
        bool shouldEnable = levelManager.areVisualHintsVisible;
        if(lineRenderer.enabled != shouldEnable)
        {
            lineRenderer.enabled = shouldEnable;
            allArrowRenderers.ForEach(r => r.enabled = shouldEnable);
        }

        float distanceSqr = (lastSpawnedArrow.transform.position - position).sqrMagnitude; //Vector3.Distance(position, SpawnArrow.transform.position);
        if (distanceSqr > spawnDistance * spawnDistance && num > 0)
        {
            SpawnNewArrow();
            num--;
        }
    }

    private void SpawnNewArrow()
    {
        lastSpawnedArrow = Instantiate(arrow, position, rotation);
        lastSpawnedArrow.GetComponent<FollowPath>().waypointsRoot = this.waypointsRoot;
        lastSpawnedArrow.GetComponent<FollowPath>().isLooping = isLooping;
        var renderers = lastSpawnedArrow.GetComponentsInChildren<Renderer>();
        allArrowRenderers.AddRange(renderers);
        foreach (var r in renderers)
            r.enabled = levelManager.areVisualHintsVisible;
    }

    float CalculateLength(Vector3[] positions)
    {
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

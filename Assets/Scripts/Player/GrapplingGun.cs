using UnityEngine;

public class GrapplingGun : MonoBehaviour {

    private LineRenderer lr;
    private Collider grappleCollider;
    private Vector3 grapplePointOnCollider;
    private Vector3 grapplePoint {
        get {
            if (!grappleCollider) return Vector3.zero;
            return grappleCollider.transform.position + grapplePointOnCollider;
        } 
    }
    private float grappleProgress;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    private SpringJoint joint;
    public AudioSource audioSource;
    public AudioClip successAudio;
    public AudioClip failAudio;

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopGrapple();
        }
    }

    //Called after Update
    void LateUpdate() {
        if (IsGrappling())
        {
            joint.connectedAnchor = grapplePoint;
            joint.maxDistance -= 3.0f * Time.deltaTime;
            joint.maxDistance = Mathf.Max(joint.maxDistance, joint.minDistance);
        }
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartGrapple() {
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            grappleCollider = hit.collider;
            grapplePointOnCollider = hit.point - hit.collider.transform.position;
            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePosition = gunTip.position;

            audioSource.PlayOneShot(successAudio);
        }
        else
        {
            audioSource.PlayOneShot(failAudio);
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple() {
        lr.positionCount = 0;
        Destroy(joint);
    }

    private Vector3 currentGrapplePosition;
    
    void DrawRope() {
        //If not grappling, don't draw rope
        if (!IsGrappling()) return;

        currentGrapplePosition = grapplePoint; // Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}

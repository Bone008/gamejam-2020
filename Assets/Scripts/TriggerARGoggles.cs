using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerARGoggles : MonoBehaviour
{
    public GameObject hudGameObject;
    public MechanismTransform hideExitMechanism;
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        
        hudGameObject.SetActive(false);
    }
    public void Activate()
    {
        StartCoroutine(DoAnimation());
    }
    private IEnumerator DoAnimation()
    {
        Transform cam = Camera.main.transform;
        transform.SetParent(cam, true);
        transform.localPosition = new Vector3(0, -0.3f, 1);
        transform.localRotation = Quaternion.identity;

        GetComponent<Collider>().enabled = false;

        animator.SetTrigger("activate");
        yield return new WaitForSeconds(1f);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        // Activate HUD
        hudGameObject.SetActive(true);
        gameObject.SetActive(false);
        // Show exit door.
        hideExitMechanism.SetTriggerActive(false);
    }
}

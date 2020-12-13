using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class TriggerARGoggles : MonoBehaviour
{
    public GameObject hudGameObject;
    public MechanismTransform hideExitMechanism;
    private Animator animator;
    public PostProcessingBehaviour postProcessing;
    public GameObject[] stuffToDisableOnPickup;
    public GameObject[] stuffToEnableForAR;

    public Image uiImageToMakeHologram;
    public Material hologramMaterial;

    public GameObject initializingHud;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        
        hudGameObject.SetActive(false);
        postProcessing.enabled = false;
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

        foreach (var go in stuffToDisableOnPickup)
            go.SetActive(false);

        animator.SetTrigger("activate");
        yield return new WaitForSeconds(1f);
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            yield return null;

        // Activate HUD
        hudGameObject.SetActive(true);
        postProcessing.enabled = true;
        uiImageToMakeHologram.material = hologramMaterial;

        // Show exit door.
        hideExitMechanism.SetTriggerActive(false);

        foreach (var go in stuffToEnableForAR)
            go.SetActive(true);

        initializingHud.SetActive(true);
        var initText = initializingHud.GetComponentInChildren<Text>();
        string fullStr = initText.text;
        for(int i=1; i<fullStr.Length; i++)
        {
            initText.text = fullStr.Substring(0, i);
            yield return new WaitForSeconds(0.09f);
        }
        yield return new WaitForSeconds(2f);
        initializingHud.SetActive(false);

        gameObject.SetActive(false);
    }
}

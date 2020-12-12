using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{

    public GameObject panel;

    private bool playvideo = false;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // for testing purposes
        if (Input.GetKeyDown(KeyCode.V))
        {
               Play();
        }
    }

    IEnumerator CoFunc()
    {
        panel.SetActive(true);
        // manually change depending on video length
        yield return new WaitForSeconds(3);
        panel.SetActive(false);
    }

    public void Play()
    {
        StartCoroutine(CoFunc());
    }
}

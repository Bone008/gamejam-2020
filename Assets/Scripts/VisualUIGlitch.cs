using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VisualUIGlitch : MonoBehaviour
{
    private LevelManager levelManager;
    private Image[] panels;

    void Start()
    {
        levelManager = LevelManager.FindActiveInstance();
        panels = transform.Cast<Transform>().Select(t => t.GetComponent<Image>()).ToArray();
    }

    void Update()
    {
        bool shouldEnable = levelManager.isGlobalGlitchHappening;
        if (panels[0].enabled != shouldEnable)
        {
            foreach (var panel in panels)
            {
                panel.transform.localPosition = new Vector2(panel.transform.localPosition.x, UnityEngine.Random.Range(-800f, 800f));
                panel.enabled = shouldEnable;
            }
        }
    }
}

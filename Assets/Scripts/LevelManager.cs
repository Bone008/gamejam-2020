using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Sets up the level config from the configured data.
/// </summary>
public class LevelManager : MonoBehaviour
{
    private static int currentLevelIndex = -1;

    public AllLevelsData allData;
    public GrapplingGun grapplingScript;
    public PlayVideo playVideoScript;
    public InfoCardController infoCardScript;

    public Image blackScreenOverlay;

    /// <summary>Whether the platform path preview and related stuff is CURRENTLY visible.</summary>
    public bool isGlobalGlitchHappening { get; private set; }
    /// <summary>Whether the platform path preview and related stuff is CURRENTLY visible.</summary>
    public bool areVisualHintsVisible { get; private set; }

    private LevelInfo levelData;
    private float globalGlitchCooldown = -1f;

    void Awake()
    {
        if (allData.enableForcedConfigForEditor && Application.isEditor)
        {
            Debug.LogWarning("[LevelManager] Using settings from 'forced config for editor'. Use this for debugging only and do NOT check it in!");
            levelData = allData.forcedConfigForEditor;
        }
        else if (currentLevelIndex < 0)
        {
            Debug.Log("[LevelManager] Apparently starting level out of context. This is fine. Skipping initialization.");
            levelData = new LevelInfo();
            return;
            //levelData = allData.levels.First(level => level.sceneName == sceneName);
            //currentLevelIndex = Array.IndexOf(allData.levels, levelData);
            //Debug.Log("[LevelManager] Found configuration #" + currentLevelIndex);
        }
        else
        {
            levelData = allData.levels[currentLevelIndex];
            string sceneName = SceneManager.GetActiveScene().name;
            if (levelData.sceneName != sceneName)
                Debug.LogWarning($"[LevelManager] WEIRD THING WARNING: This is level index {currentLevelIndex}, but we are in scene {sceneName}, instead of expected scene {levelData}!");
        }

        InitializeLevel();
    }

    private void InitializeLevel()
    {
        Debug.Log($"[LevelManager] Config: brokenGrapple={levelData.brokenGrapplingHook}, brokenPathHints={levelData.brokenPathHints}, corruption={levelData.corruption}");

        grapplingScript.corrupted = levelData.brokenGrapplingHook;
        switch (levelData.corruption)
        {
            case CorruptionLevel.None:
                playVideoScript.videoProbability = 0f;
                infoCardScript.softCorruptionProbability = 0f;
                infoCardScript.hardCorruptionProbability = 0f;
                break;
            case CorruptionLevel.Low:
                globalGlitchCooldown = 1f; // start the glitches by setting >0
                playVideoScript.videoProbability = 0.1f;
                infoCardScript.softCorruptionProbability = 0.25f;
                infoCardScript.hardCorruptionProbability = 0.01f;
                break;
            case CorruptionLevel.Annoying:
                globalGlitchCooldown = 1f; // start the glitches by setting >0
                playVideoScript.videoProbability = 0.2f;
                infoCardScript.softCorruptionProbability = 0.9f;
                infoCardScript.hardCorruptionProbability = 0.4f;
                break;
        }

        SpeechManager.Instance.PlaySpecificClip(levelData.optionalEntranceSpeech);
    }

    void Update()
    {
        if (globalGlitchCooldown > 0)
        {
            globalGlitchCooldown -= Time.deltaTime;
            if (globalGlitchCooldown <= 0)
            {
                // Toggle and restart timer.
                isGlobalGlitchHappening = !isGlobalGlitchHappening;
                float duration = (levelData.corruption == CorruptionLevel.Annoying ? 0.5f : 0.25f);
                float cooldown = UnityEngine.Random.Range(5f, 30f) * (levelData.corruption == CorruptionLevel.Annoying ? 0.3f : 1f);
                globalGlitchCooldown = isGlobalGlitchHappening ? duration : cooldown;
            }
        }
        // Equivalent for now, might change decision.
        areVisualHintsVisible = !isGlobalGlitchHappening && !levelData.brokenPathHints;
    }

    public void LoadFirstLevel()
    {
        currentLevelIndex = -1;
        LoadNextLevel(); // Will load level 0.
    }

    public void LoadNextLevel()
    {
        if (currentLevelIndex >= allData.levels.Length - 1)
        {
            Debug.LogWarning("[LevelManager] Attempted to load next level, but already at maximum.");
            return;
        }
        if (currentLevelIndex < 0)
        {
            Debug.LogWarning("[LevelManager] Now we would switch to the next level, but we loaded out of order, so skipping that.");
            return;
        }

        currentLevelIndex++;
        string nextSceneName = allData.levels[currentLevelIndex].sceneName;
        Debug.Log($"[LevelManager] Switching to next scene #{currentLevelIndex}: {nextSceneName}.");
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    public void OnPlayerWinLevel(out float extraWaitTime)
    {
        extraWaitTime = levelData.optionalFinishSpeech ? levelData.optionalFinishSpeech.length : 0f;
        SpeechManager.Instance.PlaySpecificClip(levelData.optionalFinishSpeech);
        blackScreenOverlay.enabled = true;
    }

    /// <summary>WARNING: Very slow, do not call frequently!</summary>
    public static LevelManager FindActiveInstance()
    {
        return GameObject.FindObjectOfType<LevelManager>();
    }
}

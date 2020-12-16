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
    private static int lastPlayedIntroIndex = -1;
    private static bool hardcoreMode = false;

    public static void ExitToMainMenu()
    {
        currentLevelIndex = -1;
        lastPlayedIntroIndex = -1;
        hardcoreMode = false;
        Cursor.lockState = CursorLockMode.None;
        SpeechManager.Instance.StopSpeech();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public AllLevelsData allData;
    public GrapplingGun grapplingScript;
    public PlayVideo playVideoScript;
    public InfoCardController infoCardScript;
    public bool thisIsLevel0;

    public Image blackScreenOverlay;

    /// <summary>Whether the platform path preview and related stuff is CURRENTLY visible.</summary>
    public bool isGlobalGlitchHappening { get; private set; }
    /// <summary>Whether the platform path preview and related stuff is CURRENTLY visible.</summary>
    public bool areVisualHintsVisible { get; private set; }

    private LevelInfo levelData;
    private float globalGlitchCooldown = -1f;

    void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (allData.enableForcedConfigForEditor && Application.isEditor)
        {
            Debug.LogWarning("[LevelManager] Using settings from 'forced config for editor'. Use this for debugging only and do NOT check it in!");
            levelData = allData.forcedConfigForEditor;
        }
        else if (thisIsLevel0)
        {
            Debug.Log("[LevelManager] This is level 0, so we are starting our level config journey here!");
            currentLevelIndex = 0;
            levelData = allData.levels[0];
        }
        else if (currentLevelIndex < 0)
        {
            Debug.Log("[LevelManager] Apparently starting level out of context. This is fine. Looking for first time this scene appears in the config.");
            levelData = allData.levels.First(level => level.sceneName == sceneName);
            currentLevelIndex = Array.IndexOf(allData.levels, levelData);
            Debug.Log("[LevelManager] Found configuration #" + currentLevelIndex);
        }
        else
        {
            levelData = allData.levels[currentLevelIndex];
            if (levelData.sceneName != sceneName)
                Debug.LogWarning($"[LevelManager] WEIRD THING WARNING: This is level index {currentLevelIndex}, but we are in scene {sceneName}, instead of expected scene {levelData}!");
        }

        if(hardcoreMode)
        {
            levelData = new LevelInfo
            {
                brokenGrapplingHook = true,
                brokenPathHints = true,
                corruption = CorruptionLevel.Annoying,
                optionalEntranceSpeech = levelData.optionalEntranceSpeech,
                optionalFinishSpeech = levelData.optionalFinishSpeech,
            };
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
                infoCardScript.hardCorruptionProbability = 0.08f;
                break;
            case CorruptionLevel.Annoying:
                globalGlitchCooldown = 1f; // start the glitches by setting >0
                playVideoScript.videoProbability = 0.2f;
                infoCardScript.softCorruptionProbability = 0.9f;
                infoCardScript.hardCorruptionProbability = 0.25f;
                break;
        }

        if (currentLevelIndex != lastPlayedIntroIndex)
        {
            SpeechManager.Instance.PlaySpecificClip(levelData.optionalEntranceSpeech);
            lastPlayedIntroIndex = currentLevelIndex;
        }
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

    public void LoadNextLevel(bool goBackwards = false)
    {
        if (!goBackwards && currentLevelIndex >= allData.levels.Length - 1)
        {
            Debug.LogWarning("[LevelManager] Reached final level, starting from the beginning.");
            currentLevelIndex = 0; // moves to index 1
            hardcoreMode = true;
        }
        else if(goBackwards && currentLevelIndex == 0)
        {
            // Do nothing.
            return;
        }
        else if (currentLevelIndex < 0)
        {
            Debug.LogWarning("[LevelManager] Now we would switch to the next level, but we loaded out of order, so skipping that.");
            return;
        }

        currentLevelIndex += (goBackwards ? -1 : 1);
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

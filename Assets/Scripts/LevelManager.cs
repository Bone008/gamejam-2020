using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sets up the level config from the configured data.
/// </summary>
public class LevelManager : MonoBehaviour
{
    private static int currentLevelIndex = -1;

    public AllLevelsData allData;
    public GrapplingGun grapplingScript;
    public PlayerMovement playerMovementScript;
    public InfoCardController infoCardScript;

    private LevelInfo levelData;

    void Awake()
    {
        if(allData.enableForcedConfigForEditor && Application.isEditor)
        {
            Debug.Log("[LevelManager] Using settings from 'forced config for editor'. Use this for debugging only and do NOT check it in!");
            levelData = allData.forcedConfigForEditor;
        }
        else if (currentLevelIndex < 0)
        {
            Debug.Log("[LevelManager] Apparently starting level out of context. This is fine. Skipping initialization.");
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
        Debug.Log($"[LevelManager] Config: brokenGrapple={levelData.brokenGrapplingHook}, corruption={levelData.corruption}");

        grapplingScript.corrupted = levelData.brokenGrapplingHook;
        switch(levelData.corruption)
        {
            case CorruptionLevel.None:
                playerMovementScript.videoProbability = 0f;
                infoCardScript.softCorruptionProbability = 0f;
                infoCardScript.hardCorruptionProbability = 0f;
                break;
            case CorruptionLevel.Low:
                playerMovementScript.videoProbability = 0.08f;
                infoCardScript.softCorruptionProbability = 0.25f;
                infoCardScript.hardCorruptionProbability = 0.01f;
                break;
            case CorruptionLevel.Annoying:
                playerMovementScript.videoProbability = 0.2f;
                infoCardScript.softCorruptionProbability = 0.9f;
                infoCardScript.hardCorruptionProbability = 0.4f;
                break;
        }
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

        currentLevelIndex++;
        string nextSceneName = allData.levels[currentLevelIndex].sceneName;
        Debug.Log($"[LevelManager] Switching to next scene #{currentLevelIndex}: {nextSceneName}.");
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
    }

    /// <summary>WARNING: Very slow, do not call frequently!</summary>
    public static LevelManager FindActiveInstance()
    {
        return GameObject.FindObjectOfType<LevelManager>();
    }
}

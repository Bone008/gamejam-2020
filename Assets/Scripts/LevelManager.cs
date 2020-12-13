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
    private LevelInfo levelData;

    void Awake()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (currentLevelIndex < 0)
        {
            Debug.Log("[LevelManager] Apparently starting level out of context. This is fine. Choosing first available configuration.");
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

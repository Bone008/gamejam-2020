using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Management", menuName = "AllLevelsData")]
public class AllLevelsData : ScriptableObject
{
    public LevelInfo[] levels;
    public bool enableForcedConfigForEditor;
    public LevelInfo forcedConfigForEditor;
}

[System.Serializable]
public struct LevelInfo
{
    [Scene]
    public string sceneName;
    public bool brokenGrapplingHook;
    public CorruptionLevel corruption;
    public AudioClip optionalEntranceSpeech;
    public AudioClip optionalFinishSpeech;
}

public enum CorruptionLevel
{
    None,
    Low,
    Annoying
}

/// <summary>
/// Converts a string property into a Scene property in the inspector
/// </summary>
public class SceneAttribute : PropertyAttribute { }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Management", menuName = "AllLevelsData")]
public class AllLevelsData : ScriptableObject
{
    [Header("Random speech clips")]
    public AudioClip[] onDeathSpeeches;
    public AudioClip[] onBrokenGrappleSpeeches;
    public AudioClip[] onRandomVideoSpeeches;

    [Header("Levels")]
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
    public bool brokenPathHints;
    public CorruptionLevel corruption;
    public AudioClip optionalEntranceSpeech;
    public AudioClip optionalFinishSpeech;
    [Multiline]
    public string comment;
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

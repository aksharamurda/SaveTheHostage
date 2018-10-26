using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEditor;

[Serializable]
public class Level {
    public string levelName;
    [Range(0, 3)]
    public int findItem;
    public bool Unlocked;
    public bool levelComplete;

    public Level()
    {

    }

    public Level(string lvName)
    {
        levelName = lvName;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Zone{
    public string zoneName;
    public int itemGoal;
    public bool unlockedZone;
    public bool missionComplete;
    public List<Level> levels = new List<Level>();
}

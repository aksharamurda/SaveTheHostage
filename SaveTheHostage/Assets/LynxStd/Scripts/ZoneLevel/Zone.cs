using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Zone{
    public string zoneName;
    public bool Unlocked;
    //[HideInInspector]
    public List<Level> levels = new List<Level>();
}

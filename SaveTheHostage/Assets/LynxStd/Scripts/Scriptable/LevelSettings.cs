using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable/Level", order = 1)]
public class LevelSettings : ScriptableObject {
    public string zone;
    public string refLevelScene;

    public Level level;
    public GameObject prefabItem;
    public int itemSize;
    public Sprite itemSprite;
    public bool haveShield;

}

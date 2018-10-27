using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable/Level", order = 1)]
public class LevelSettings : ScriptableObject {
    public string zone;
    public string refLevelScene;
    public GameObject prefabItem;
    public int itemSize;
    public Sprite itemSprite;
    public bool haveShield;
    public bool isTutorial;

    [Header("Coin")]
    public int starA;
    public int starB;
    public int starC;

    [Header("Level")]
    public Level level;
}

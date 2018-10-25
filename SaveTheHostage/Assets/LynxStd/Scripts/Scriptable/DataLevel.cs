using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable/Level", order = 1)]
public class DataLevel : ScriptableObject {
    public string levelName;
    public GameObject prefabItem;
    public int itemSize;
    public Sprite itemSprite;
    public bool haveShield;

}

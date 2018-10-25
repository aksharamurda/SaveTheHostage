using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable/Stats", order = 1)]
public class PlayerStats : ScriptableObject {

    public string playerName = "[Player Name]";
    public int playerCoin = 100;
    public int playerStar;
    public int playerLevel;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerProfile {

    public string playerName = "[Player Name]";
    public int playerCoin = 100;
    public int playerStar;
    public int playerLevel;

    public PlayerProfile()
    {
        playerName = "[Player Name]";
        playerCoin = 100;
        playerStar = 0;
        playerLevel = 0;
    }
}

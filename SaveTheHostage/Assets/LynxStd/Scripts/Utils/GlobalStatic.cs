using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GlobalStatic{

    public static PlayerStats GetPlayerStats() {
        BinaryFormatter binFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/LynxGame.bin", FileMode.Open);

        PlayerStats playerStats = (PlayerStats)binFormat.Deserialize(fileStream);
        fileStream.Close();

        return playerStats;

    }

    public static void SavePlayerStats(PlayerStats playerStats)
    {
        BinaryFormatter binFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/LynxGame.bin", FileMode.Create);

        binFormat.Serialize(fileStream, playerStats);
        fileStream.Close();
    }

    public static void CreatePlayerStats()
    {
        Debug.Log(Application.persistentDataPath);
        if (!File.Exists(Application.persistentDataPath + "/LynxGame.bin"))
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/LynxGame.bin", FileMode.Create);
            PlayerStats playerStats = new PlayerStats();
            binFormat.Serialize(fileStream, playerStats);
            fileStream.Close();
        }

    }
}

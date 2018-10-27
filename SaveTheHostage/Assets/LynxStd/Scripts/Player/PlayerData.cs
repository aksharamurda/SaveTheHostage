using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

public class PlayerData{

    

    public static PlayerProfile GetPlayerProfile() {

        byte[] key = Convert.FromBase64String(Encryption.cryptoKey);
        BinaryFormatter binFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/Profile.bin", FileMode.Open);

        using (CryptoStream cryptoStream = Encryption.CreateDecryptionStream(key, fileStream))
        {
            return (PlayerProfile)binFormat.Deserialize(cryptoStream);
        }

    }

    public static void UpdatePlayerProfile(PlayerProfile playerProfile)
    {
        byte[] key = Convert.FromBase64String(Encryption.cryptoKey);

        BinaryFormatter binFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/Profile.bin", FileMode.Create);

        using (CryptoStream cryptoStream = Encryption.CreateEncryptionStream(key, fileStream))
        {
            binFormat.Serialize(cryptoStream, playerProfile);
        }

    }

    public static void CreatePlayerProfile()
    {
        if (!File.Exists(Application.persistentDataPath + "/Profile.bin"))
        {
            byte[] key = Convert.FromBase64String(Encryption.cryptoKey);
            BinaryFormatter binFormat = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/Profile.bin", FileMode.Create);
            PlayerProfile playerProfile = new PlayerProfile();

            using (CryptoStream cryptoStream = Encryption.CreateEncryptionStream(key, fileStream))
            {
                binFormat.Serialize(cryptoStream, playerProfile);
            }
        }

    }

    
}

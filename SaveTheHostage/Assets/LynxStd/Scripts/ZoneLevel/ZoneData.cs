using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class ZoneData
{
    public static void CreateZoneData(Zone zone)
    {
        Debug.Log(Application.persistentDataPath);
        if (!File.Exists(Application.persistentDataPath + "/" + zone.zoneName + ".bin"))
        {
            byte[] key = Convert.FromBase64String(Encryption.cryptoKey);
            BinaryFormatter binFormat = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + zone.zoneName + ".bin", FileMode.Create);

            using (CryptoStream cryptoStream = Encryption.CreateEncryptionStream(key, fileStream))
            {
                binFormat.Serialize(cryptoStream, zone);
            }
        }

    }

    public static void UpdateZoneData(Zone zone)
    {
        byte[] key = Convert.FromBase64String(Encryption.cryptoKey);

        BinaryFormatter binFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + zone.zoneName + ".bin", FileMode.Create);

        using (CryptoStream cryptoStream = Encryption.CreateEncryptionStream(key, fileStream))
        {
            binFormat.Serialize(cryptoStream, zone);
        }

    }

    public static Zone GetZoneData(string zoneName)
    {

        byte[] key = Convert.FromBase64String(Encryption.cryptoKey);
        BinaryFormatter binFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + zoneName + ".bin", FileMode.Open);

        using (CryptoStream cryptoStream = Encryption.CreateDecryptionStream(key, fileStream))
        {
            return (Zone)binFormat.Deserialize(cryptoStream);
        }

    }
}


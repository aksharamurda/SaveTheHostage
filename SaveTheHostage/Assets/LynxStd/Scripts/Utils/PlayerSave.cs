using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Security.Cryptography;

public class PlayerSave{

    private const int ivSize = 16;
    private const int keySize = 256;
    private const string cryptoKey = "YmlzbWlsbGFoaXJyYWhtYW5pcnJhaGlt";

    public static PlayerStats GetPlayerStats() {

        byte[] key = Convert.FromBase64String(cryptoKey);
        BinaryFormatter binFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/LynxGame.bin", FileMode.Open);

        using (CryptoStream cryptoStream = CreateDecryptionStream(key, fileStream))
        {
            return (PlayerStats)binFormat.Deserialize(cryptoStream);
        }

    }

    public static void SavePlayerStats(PlayerStats playerStats)
    {
        byte[] key = Convert.FromBase64String(cryptoKey);

        BinaryFormatter binFormat = new BinaryFormatter();
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/LynxGame.bin", FileMode.Create);

        using (CryptoStream cryptoStream = CreateEncryptionStream(key, fileStream))
        {
            binFormat.Serialize(cryptoStream, playerStats);
        }

    }

    public static void CreatePlayerStats()
    {
        Debug.Log(Application.persistentDataPath);
        if (!File.Exists(Application.persistentDataPath + "/LynxGame.bin"))
        {
            byte[] key = Convert.FromBase64String(cryptoKey);
            BinaryFormatter binFormat = new BinaryFormatter();
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/LynxGame.bin", FileMode.Create);
            PlayerStats playerStats = new PlayerStats();

            using (CryptoStream cryptoStream = CreateEncryptionStream(key, fileStream))
            {
                binFormat.Serialize(cryptoStream, playerStats);
            }
        }

    }

    public static CryptoStream CreateEncryptionStream(byte[] key, Stream outputStream)
    {
        byte[] iv = new byte[ivSize];

        using (var rng = new RNGCryptoServiceProvider())
        {
            // Using a cryptographic random number generator
            rng.GetNonZeroBytes(iv);
        }

        // Write IV to the start of the stream
        outputStream.Write(iv, 0, iv.Length);

        Rijndael rijndael = new RijndaelManaged();
        rijndael.KeySize = keySize;

        CryptoStream encryptor = new CryptoStream(
            outputStream,
            rijndael.CreateEncryptor(key, iv),
            CryptoStreamMode.Write);
        return encryptor;
    }

    public static CryptoStream CreateDecryptionStream(byte[] key, Stream inputStream)
    {
        byte[] iv = new byte[ivSize];

        if (inputStream.Read(iv, 0, iv.Length) != iv.Length)
        {
            throw new ApplicationException("Failed to read IV from stream.");
        }

        Rijndael rijndael = new RijndaelManaged();
        rijndael.KeySize = keySize;

        CryptoStream decryptor = new CryptoStream(
            inputStream,
            rijndael.CreateDecryptor(key, iv),
            CryptoStreamMode.Read);
        return decryptor;
    }
}

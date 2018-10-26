using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

public class Encryption
{
    private const int ivSize = 16;
    private const int keySize = 256;
    public const string cryptoKey = "YmlzbWlsbGFoaXJyYWhtYW5pcnJhaGlt";

    public static CryptoStream CreateEncryptionStream(byte[] key, Stream outputStream)
    {
        byte[] iv = new byte[ivSize];

        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        // Using a cryptographic random number generator
        rng.GetNonZeroBytes(iv);


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


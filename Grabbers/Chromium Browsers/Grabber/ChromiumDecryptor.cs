using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Newtonsoft.Json;

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

public static class ChromiumDecryptor
{
    public static byte[] GetKey(string keyPath)
    {
        dynamic json = JsonConvert.DeserializeObject(FileUtils.ReadFile(keyPath));
        return ProtectedData.Unprotect(Convert.FromBase64String((string)json.os_crypt.encrypted_key).Skip(5).ToArray(), null, DataProtectionScope.CurrentUser);
    }

    public static string Decrypt(byte[] encryptedBytes, byte[] key, byte[] iv)
    {
        try
        {
            var cipher = new GcmBlockCipher(new AesEngine());
            cipher.Init(false, new AeadParameters(new KeyParameter(key), 128, iv, null));
            var plainBytes = new byte[cipher.GetOutputSize(encryptedBytes.Length)];
            cipher.DoFinal(plainBytes, cipher.ProcessBytes(encryptedBytes, 0, encryptedBytes.Length, plainBytes, 0));
            return Encoding.UTF8.GetString(plainBytes).TrimEnd("\r\n\0".ToCharArray());
        }
        catch
        {
            return "";
        }
    }

    public static void Prepare(byte[] encryptedData, out byte[] nonce, out byte[] ciphertextTag)
    {
        nonce = new byte[12];
        ciphertextTag = new byte[encryptedData.Length - 3 - nonce.Length];

        Array.Copy(encryptedData, 3, nonce, 0, nonce.Length);
        Array.Copy(encryptedData, 3 + nonce.Length, ciphertextTag, 0, ciphertextTag.Length);
    }
}
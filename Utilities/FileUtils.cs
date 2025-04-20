using System.IO;
using System;
using System.DirectoryServices;
using System.Collections.Generic;

public class FileUtils
{
    public static string ReadFile(string file)
    {
        try
        {
            if (!System.IO.File.Exists(file))
            {
                return "";
            }

            string content = "";
            ProtoRandom protoRandom = new ProtoRandom(1);
            string newFile = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "Temp\\" + protoRandom.GetRandomString("abcdefghijklmnopqrstuvwxyz", 32) + ".dat";
            System.IO.File.Copy(file, newFile, true);
            System.IO.File.SetAttributes(newFile, FileAttributes.Hidden);

            using (FileStream stream = File.Open(newFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        content = reader.ReadToEnd();
                    }

                    reader.Close();
                    stream.Close();

                    reader.Dispose();
                    stream.Dispose();
                }
            }

            try
            {
                System.IO.File.Delete(newFile);
            }
            catch
            {

            }

            return content;
        }
        catch
        {
            return "";
        }
    }

    public static string CopyFile(string file)
    {
        try
        {
            if (!System.IO.File.Exists(file))
            {
                return "";
            }

            ProtoRandom protoRandom = new ProtoRandom(1);
            string newFile = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)) + "Temp\\" + protoRandom.GetRandomString("abcdefghijklmnopqrstuvwxyz", 32) + ".dat";
           
            System.IO.File.Copy(file, newFile, true);
            System.IO.File.SetAttributes(newFile, FileAttributes.Hidden);

            return newFile;
        }
        catch
        {
            return "";
        }
    }

    public static string GetWorkingDirectory(string dir)
    {
        foreach (var drive in DriveInfo.GetDrives())
        {
            foreach (string user in GetComputerUsers())
            {
                if (System.IO.File.Exists(drive + "Users\\" + user + "\\" + dir))
                {
                    return drive + "Users\\" + user + "\\" + dir;
                }
            }
        }

        return "";
    }

    public static List<string> GetComputerUsers()
    {
        List<string> users = new List<string>();

        using (DirectoryEntry computerEntry = new DirectoryEntry($"WinNT://{Environment.MachineName},computer"))
        {
            foreach (DirectoryEntry childEntry in computerEntry.Children)
            {
                if (childEntry.SchemaClassName == "User")
                {
                    users.Add(childEntry.Name);
                }
            }
        }

        return users;
    }
}
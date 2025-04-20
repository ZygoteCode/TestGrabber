using System.IO;
using System.Data.SQLite;

public class SQLUtils
{
    public static byte[] GetBytes(SQLiteDataReader reader, int columnIndex)
    {
        byte[] buffer = new byte[2 * 1024];
        long bytesRead;
        long fieldOffset = 0;

        using (MemoryStream stream = new MemoryStream())
        {
            while ((bytesRead = reader.GetBytes(columnIndex, fieldOffset, buffer, 0, buffer.Length)) > 0)
            {
                stream.Write(buffer, 0, (int)bytesRead);
                fieldOffset += bytesRead;
            }

            return stream.ToArray();
        }
    }
}
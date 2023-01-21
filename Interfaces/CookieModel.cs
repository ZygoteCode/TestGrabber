public class CookieModel
{
    public string Name { get; set; }
    public string Value { get; set; }
    public string Host { get; set; }
    public long CreationUTC { get; set; }
    public string CorrectCreationUTC { get; set; }
    public string TopFrameSiteKey { get; set; }
    public string Path { get; set; }
    public long ExpiresUTC { get; set; }
    public string CorrectExpiresUTC { get; set; }
    public bool IsSecure { get; set; }
    public bool IsHTTPOnly { get; set; }
    public long LastAccessUTC { get; set; }
    public string CorrectLastAccessUTC { get; set; }
    public bool HasExpires { get; set; }
    public bool IsPersistent { get; set; }
    public int Priority { get; set; }
    public bool Samesite { get; set; }
    public int SourceScheme { get; set; }
    public int SourcePort { get; set; }
    public bool SameParty { get; set; }
}
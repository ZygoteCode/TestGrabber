public class BodyUtils
{
    public static string GetCleanIP(string body)
    {
        int skips = 0;

        for (int i = 0; i < body.Length; i++)
        {
            if (!Microsoft.VisualBasic.Information.IsNumeric(body[i]))
            {
                skips++;
            }
            else
            {
                break;
            }
        }

        if (skips > 0)
        {
            body = body.Substring(skips);
        }

        skips = 0;

        for (int i = body.Length - 1; i >= 0; i--)
        {
            if (!Microsoft.VisualBasic.Information.IsNumeric(body[i]))
            {
                skips++;
            }
            else
            {
                break;
            }
        }

        if (skips > 0)
        {
            body = body.Substring(body.Length - skips);
        }

        return body;
    }

    public static string GetCleanJSON(string str)
    {
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '{' || str[i] == '[')
            {
                str = str.Substring(i);
                break;
            }
        }

        int steps = 0;

        for (int i = str.Length - 1; i >= 0; i--)
        {
            if (str[i] == '}' || str[i] == ']')
            {
                str = str.Substring(0, str.Length - steps);
                break;
            }

            steps++;
        }

        return str;
    }
}
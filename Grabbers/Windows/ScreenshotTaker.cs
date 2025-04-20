using System.Drawing;
using System.Windows.Forms;

public class ScreenshotTaker
{
    public static Bitmap GetScreenshot()
    {
        try
        {
            Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bm);
            g.CopyFromScreen(0, 0, 0, 0, bm.Size);
            return bm;
        }
        catch
        {
            return null;
        }
    }
}
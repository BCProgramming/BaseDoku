using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASeDoku
{
    public class BoardColourTheme
    {
        public TextThemeColors Standard = new TextThemeColors(Color.Black, Color.White, Color.Blue, Color.SkyBlue);
        public TextThemeColors Selected = new TextThemeColors(Color.Navy, Color.Yellow, Color.Red, Color.AliceBlue);
        public TextThemeColors Locked = new TextThemeColors(Color.DarkSlateGray,Color.LightSteelBlue,Color.SkyBlue,Color.Navy);

        public Color MiniGridColorA = Color.SkyBlue;
        public Color MiniGridColorB = Color.IndianRed;
        public Color FullGridColorA = Color.White;
        public Color FullGridColorB = Color.AntiqueWhite;
    }
    public class TextThemeColors
    {
        public Color TextColor;
        public Color BackColor;
        public Color Outline;
        public Color Shadow;
        public TextThemeColors(Color pText,Color pBackColor,Color pOutline,Color pShadow)
        {
            TextColor = pText;
            BackColor = pBackColor;
            Outline = pOutline;
            Shadow = pShadow;
        }
    }
}

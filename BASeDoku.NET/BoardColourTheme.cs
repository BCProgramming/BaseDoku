using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BASeDoku
{
    public class BoardColourTheme
    {
        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        private static extern void DwmGetColorizationParameters(ref DWMCOLORIZATIONPARAMS parms);
        private struct DWMCOLORIZATIONPARAMS
        {
            public uint ColorizationColor,
                ColorizationAfterglow,
                ColorizationColorBalance,
                ColorizationAfterglowBalance,
                ColorizationBlurBalance,
                ColorizationGlassReflectionIntensity,
                ColorizationOpaqueBlend;
        }
        private static Color GetWindowColorizationColor(bool opaque)
        {
       
        DWMCOLORIZATIONPARAMS parms = new DWMCOLORIZATIONPARAMS();
            DwmGetColorizationParameters(ref parms);

            //Color.FromArgb(parms.ColorizationColor);
            return Color.FromArgb(
                (byte)(opaque ? 255 : (parms.ColorizationColor >> 24) / 2),
                (byte)(parms.ColorizationColor >> 16),
                (byte)(parms.ColorizationColor >> 8),
                (byte)parms.ColorizationColor
            );
        }
        public static Color MixColor(Color ColorA, Color ColorB)
        {
            return Color.FromArgb(
                (ColorA.A + ColorB.A) / 2,
                (ColorA.R + ColorB.R) / 2,
                (ColorA.G + ColorB.G) / 2,
                (ColorA.B + ColorB.B) / 2
            );
        }
        public static Color InvertColor(Color ColorA)
        {
            return Color.FromArgb(255 - ColorA.A, 255 - ColorA.R, 255 - ColorA.G, 255 - ColorA.B);
        }
        public static BoardColourTheme Windows10Theme()
        {
            Color AccentColour = GetWindowColorizationColor(true);

            BoardColourTheme resultTheme = new BoardColourTheme();
            resultTheme.Background = AccentColour;
            resultTheme.FullGridColorA = AccentColour;
            resultTheme.FullGridColorB = MixColor(AccentColour, MixColor(AccentColour, Color.Black));
            resultTheme.MiniGridColorA = MixColor(AccentColour, MixColor(resultTheme.FullGridColorB, Color.Aqua));
            resultTheme.MiniGridColorB = MixColor(Color.BlueViolet, MixColor(resultTheme.MiniGridColorA, resultTheme.FullGridColorB));
            resultTheme.Standard = new TextThemeColors(Color.White, AccentColour, Color.Transparent, MixColor(AccentColour, Color.DimGray));
            resultTheme.Selected = new TextThemeColors(Color.Black, MixColor(Color.DarkBlue, MixColor(Color.DarkBlue,InvertColor(AccentColour))), Color.Transparent, MixColor(InvertColor(AccentColour), Color.DimGray));
            resultTheme.Highlighted = new TextThemeColors(
                MixColor(Color.DarkBlue,resultTheme.Standard.TextColor),
                MixColor(Color.Lime, resultTheme.Standard.BackColor),
                MixColor(MixColor(resultTheme.Standard.Outline, Color.DarkBlue), resultTheme.Standard.Outline),
                MixColor(MixColor(resultTheme.Standard.Shadow, Color.DarkBlue), resultTheme.Standard.Shadow)
                );
            resultTheme.Locked = new TextThemeColors(
                MixColor(resultTheme.Standard.TextColor, Color.DimGray),
                MixColor(resultTheme.Standard.BackColor, Color.DimGray),
                Color.Transparent, Color.Black);

            return resultTheme;

        }
        public TextThemeColors Standard = new TextThemeColors(Color.Black, Color.White, Color.Blue, Color.SkyBlue);
        public TextThemeColors Selected = new TextThemeColors(Color.Navy, Color.Yellow, Color.Red, Color.AliceBlue);
        public TextThemeColors Locked = new TextThemeColors(Color.DarkSlateGray,Color.LightSteelBlue,Color.SkyBlue,Color.Navy);
        public TextThemeColors Highlighted = new TextThemeColors(Color.White,Color.DarkOrchid,Color.YellowGreen,Color.Green);
        public Color Background = Color.White;
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

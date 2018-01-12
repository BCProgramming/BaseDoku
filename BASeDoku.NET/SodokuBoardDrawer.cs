using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BASeDoku
{
    public class SodokuBoardGDIPlusHandler
    {
        public BoardColourTheme ColourScheme = new BoardColourTheme();
        public SodokuBoard GameBoard = null;
        public bool DrawPossible = false;
        public SodokuBoardGDIPlusHandler(SodokuBoard gameBoard,BoardColourTheme Scheme)
        {
            GameBoard = gameBoard;
            ColourScheme = Scheme;
        }
        private void DrawCell(Graphics Target, Font useFont, SodokuCell pCell, float xPos, float yPos, float pWidth, float pHeight)
        {
            TextThemeColors ActiveTextScheme = ColourScheme.Standard;
            if (pCell.Locked) ActiveTextScheme = ColourScheme.Locked;
            else if (pCell.Selected) ActiveTextScheme = ColourScheme.Selected;
            if (pCell.Highlighted) ActiveTextScheme = ColourScheme.Highlighted;
            Color useBackColor = ActiveTextScheme.BackColor;
            Color useForeColor = ActiveTextScheme.TextColor;
            Color useOutline = ActiveTextScheme.Outline;
            Color useShadow = ActiveTextScheme.Shadow;
            int MiniX = ((pCell.X - 1) / 3) + 1;
            int MiniY = ((pCell.Y - 1) / 3) + 1;
            Color MixGrid = ((MiniX - 1) * 3 + MiniY - 1) % 2 == 0 ? Color.SkyBlue : Color.IndianRed;
            Color MixFullGrid = ((pCell.X - 1) * 9 + (pCell.Y - 1)) % 2 == 0 ? Color.White : Color.AntiqueWhite;

            Color MixAccent = BoardColourTheme.MixColor(MixGrid, MixFullGrid);

            useBackColor = BoardColourTheme.MixColor(useBackColor, MixAccent);
            var useShadowcolor = BoardColourTheme.MixColor(useBackColor, useShadow);
            String sCellText = pCell.Value.ToString();
            var MeasureSize = Target.MeasureString(sCellText, useFont);
            PointF TextPosition = new PointF((xPos + (pWidth / 2) - MeasureSize.Width / 2), (yPos + (pHeight / 2) - MeasureSize.Height / 2));
            
            Target.FillRectangle(new SolidBrush(useBackColor), xPos, yPos, pWidth, pHeight);






            using (LinearGradientBrush lgb = new LinearGradientBrush(new RectangleF(xPos, yPos, pWidth, pHeight), Color.FromArgb(96, Color.Black), Color.FromArgb(0, Color.Black), LinearGradientMode.ForwardDiagonal))
            {
                Target.FillRectangle(lgb, xPos, yPos, pWidth, pHeight);
            }
                //Target.DrawRectangle(new Pen(Color.Black,1),xPos,yPos,pWidth,pHeight);
                if (pCell.Value != 0)
                {
                    //GraphicsPath attempt...
                    /*using (GraphicsPath gpText = new GraphicsPath())
                    {
                        gpText.AddString(sCellText, useFont.FontFamily, (int)FontStyle.Bold, useFont.Size, TextPosition, StringFormat.GenericDefault);


                        Target.FillPath(new SolidBrush(useForeColor), gpText);
                        if(pCell.Locked) Target.DrawPath(new Pen(useOutline), gpText);
                    }*/
                    Target.DrawString(sCellText, useFont, new SolidBrush(useShadowcolor), new PointF(TextPosition.X + 2, TextPosition.Y + 2));
                    Target.DrawString(sCellText, useFont, new SolidBrush(useForeColor), TextPosition);
                }
                else
                {
                    if (DrawPossible)
                    {

                        var ValidValues = GameBoard.GetValidValuesForCell(pCell);
                        Font SmallStyle = new Font(new FontFamily(GenericFontFamilies.Monospace), (float)((pHeight * 1) / ValidValues.Count), FontStyle.Italic, GraphicsUnit.Pixel);
                        String sPossible = String.Join(",", ValidValues);
                        SizeF MeSize = Target.MeasureString(sPossible, SmallStyle);
                        if (MeSize.Width < pWidth)
                        {
                            PointF TextPos = new PointF((xPos + (pWidth / 2) - MeSize.Width / 2), (yPos + (pHeight / 2) - MeSize.Height / 2));
                            Target.DrawString(sPossible, SmallStyle, new SolidBrush(useForeColor), TextPos);
                        }

                    }
                }
            

        }
        private String DefaultCellFont = "Arial";
        public void Draw(Graphics Target, int pWidth, int pHeight)
        {
            Target.CompositingQuality = CompositingQuality.HighQuality;
            Target.InterpolationMode = InterpolationMode.HighQualityBilinear;
            Target.Clear(ColourScheme.Background);
            float pUseWidth = pWidth / 9f;
            float pUseHeight = pHeight / 9f;
            Pen UsePen = new Pen(Color.Black, 1);
            Pen AltPen = new Pen(Color.Black, 2);
            float FontPixelHeight = pUseHeight * .8f;
            Font UseFont = new Font(new FontFamily(DefaultCellFont), FontPixelHeight, FontStyle.Bold, GraphicsUnit.Pixel);
            for (int x = 1; x <= 9; x++)
            {
                float XDrawPos = ((x - 1) * pUseWidth);
                for (int y = 1; y <= 9; y++)
                {
                    float YDrawPos = ((y - 1) * pUseHeight);
                    SodokuCell GrabCell = GameBoard.GetCellAtPosition(x, y);
                    DrawCell(Target, UseFont, GrabCell, XDrawPos, YDrawPos, (int)pUseWidth, (int)pUseHeight);

                }


            }
            for (int x = 1; x <= 9; x++)
            {
                float XDrawPos = ((x - 1) * pUseWidth);
                Target.DrawLine((x - 1) % 3 == 0 ? AltPen : UsePen, XDrawPos, 0, XDrawPos, pHeight);
            }
            for (int y = 1; y <= 9; y++)
            {
                float YDrawPos = ((y - 1) * pUseHeight);
                Target.DrawLine((y - 1) % 3 == 0 ? AltPen : UsePen, 0, YDrawPos, pWidth, YDrawPos);
            }
        }
    }
}

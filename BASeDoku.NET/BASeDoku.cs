using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BASeDoku
{
    public partial class BASeDoku : Form
    {
        public BASeDoku()
        {
            InitializeComponent();
        }
        SodokuBoard GameBoard = null;
        private void BASeDoku_Load(object sender, EventArgs e)
        {
            Random rgen = new Random();
            GameBoard  = new SodokuBoard();
            mStripMain.Renderer = new Win10MenuRenderer();
            foreach(var currCell in GameBoard.AllCells())
            {
                //currCell.Value = rgen.Next(1, 9);
            }
            PicSodoku.Invalidate();
            PicSodoku.Update();
            PicSodoku.Refresh();
            Text += " " + ProductVersion;
        }

        private void PicSodoku_Paint(object sender, PaintEventArgs e)
        {
            GameBoard.Draw(e.Graphics, PicSodoku.ClientSize.Width, PicSodoku.ClientSize.Height);
        }

        private void BASeDoku_Resize(object sender, EventArgs e)
        {
            PicSodoku.Invalidate();
            PicSodoku.Refresh();
        }
        private SodokuCell SelectedCell = null;
        private void PicSodoku_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (SelectedCell != null) SelectedCell.Selected = false;
                SelectedCell = GameBoard.HitTest(e.X, e.Y, PicSodoku.ClientSize.Width, PicSodoku.ClientSize.Height);
                SelectedCell.Selected = true;
                PicSodoku.Invalidate();
                PicSodoku.Refresh();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (SelectedCell != null) SelectedCell.Selected = false;
                SelectedCell = GameBoard.HitTest(e.X, e.Y, PicSodoku.ClientSize.Width, PicSodoku.ClientSize.Height);
                SelectedCell.Selected = true;
                PicSodoku.Invalidate();
                PicSodoku.Refresh();

                List<int> ValidValues = new List<int>(GameBoard.GetValidValuesForCell(SelectedCell));
                if (ValidValues.Any() && !SelectedCell.Locked)
                {
                    ContextMenuStrip cms = new ContextMenuStrip();

                    for (int i = 1; i < 10; i++)
                    {

                        ToolStripMenuItem BuildItem = new ToolStripMenuItem(i.ToString());
                        BuildItem.Click += (o, ev) =>
                        {
                            SelectedCell.Value = int.Parse(BuildItem.Text);
                            PicSodoku.Invalidate();
                            PicSodoku.Refresh();
                        };
                        BuildItem.Enabled = ValidValues.Contains(i);
                        cms.Items.Add(BuildItem);
                    }
                    ToolStripMenuItem ClearItem = new ToolStripMenuItem("Clear");
                    ClearItem.Click += (o, ev) =>
                    {
                        SelectedCell.Value = 0;
                        PicSodoku.Invalidate();
                        PicSodoku.Refresh();
                    };
                    if (SelectedCell.Value != 0)
                        cms.Items.Add(ClearItem);

                    cms.Opening += (Contextstrip, arguments) => { ((ContextMenuStrip)Contextstrip).Renderer = new Win10MenuRenderer(null, true); };


                    cms.Show(PicSodoku, e.X, e.Y);


                }
            }

            
            

        }

        private void solvePuzzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GameBoard.SolvePuzzle(0))
                {
                    PicSodoku.Invalidate();
                    PicSodoku.Refresh();
                    MessageBox.Show("Puzzle Solved.");
                }
            }
            catch(Exception exp)
            {
                MessageBox.Show("Puzzle could not be solved.");
            }
        }

        private void txtInputText_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtInputText.Text = "";
            if (SelectedCell == null) return;
            var validEntries = from i in Enumerable.Range(1, 9) select i.ToString()[0];
            if (validEntries.Contains(e.KeyChar))
            {
                int EnteredValue = int.Parse(e.KeyChar.ToString());

                var ValidCellValues = GameBoard.GetValidValuesForCell(SelectedCell);
                if(!ValidCellValues.Contains(EnteredValue))
                {
                    SystemSounds.Beep.Play();
                }
                else
                {
                    SelectedCell.Value = EnteredValue;
                    SelectedCell.Selected = false;
                    PicSodoku.Invalidate();
                    PicSodoku.Refresh();
                }

            }
        }
        private String FileFilter = "BASeDoku Sodoku Puzzle (*.BaseDoku)|*.BaseDoku|All Files (*.*)|*.*";
        private void saveAsLockedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = FileFilter;
                sfd.Title = "Save Locked Puzzle";
                if(sfd.ShowDialog(this)==DialogResult.OK)
                {
                    GameBoard.Save(sfd.FileName,true);
                }

            }
        }
        private String SavedPuzzleFile = null;
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                
                sfd.Filter = FileFilter;
                sfd.Title = "Save Puzzle";
                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    SavedPuzzleFile = sfd.FileName;
                    GameBoard.Save(sfd.FileName, true);
                }

            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = FileFilter;
                ofd.Title = "Open Puzzle";
                if(ofd.ShowDialog(this)==DialogResult.OK)
                {
                    SavedPuzzleFile = ofd.FileName;
                    GameBoard.Clear();
                    GameBoard.Load(ofd.FileName);
                    PicSodoku.Invalidate();
                    PicSodoku.Refresh();
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(SavedPuzzleFile==null)
            {
                saveToolStripMenuItem_Click(sender,e);
            }
            else
            {
                GameBoard.Save(SavedPuzzleFile,false);
            }
        }

        private void generatePuzzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameBoard = SodokuBoard.GeneratePuzzle();
            PicSodoku.Invalidate();
            PicSodoku.Refresh();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
        SodokuBoardGDIPlusHandler BoardDrawHandler;
        private DateTime _StartPuzzleTime = DateTime.MinValue;
        private System.Threading.Timer PuzzleTimer = null;
        public DateTime StartPuzzleTime
        {
            get { return _StartPuzzleTime; }
            set
            {
                _StartPuzzleTime = DateTime.Now;
                if(PuzzleTimer!=null)
                {
                    PuzzleTimer.Dispose();
                    PuzzleTimer = null;
                }
                PuzzleTimer = new System.Threading.Timer(PuzzleTime,null,0,100);
            }
        }
        private void PuzzleTime(Object state)
        {
            try
            {
                Invoke((MethodInvoker)(() =>
                {
                    tStripElapsed.Text = (DateTime.Now - StartPuzzleTime).ToString(@"mm\:ss\.ff") + " Elapsed.";
                }));
            }
            catch(Exception exx)
            {

            }
        }
        private void BASeDoku_Load(object sender, EventArgs e)
        {
            Icon = Resources.Application_Icon;
            Random rgen = new Random();
            GameBoard  = new SodokuBoard();
            BoardDrawHandler = new SodokuBoardGDIPlusHandler(GameBoard, BoardColourTheme.Windows10Theme());
            mStripMain.Renderer = new Win10MenuRenderer();
            BackColor = BoardDrawHandler.ColourScheme.Background;
            statMain.BackColor = BackColor;
            mStripMain.BackColor = SystemColors.Control;
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

            BoardDrawHandler.Draw(e.Graphics, PicSodoku.ClientSize.Width, PicSodoku.ClientSize.Height);
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
                        BuildItem.Font = new Font(new FontFamily("Consolas"), 20, FontStyle.Bold);
                        if (ValidValues.Contains(i))
                        {
                            BuildItem.Click += (o, ev) =>
                            {
                                PushBoardState();
                                SelectedCell.Value = int.Parse(BuildItem.Text);
                                PicSodoku.Invalidate();
                                PicSodoku.Refresh();
                            };
                        }
                        else
                        {
                            //a "disabled" item.
                            BuildItem.ForeColor = Color.FromArgb(128, 64, 64, 64);
                            BuildItem.BackColor = Color.FromArgb(0, 64, 64, 64);
                            BuildItem.MouseHover += (o2, ev2) =>
                            {
                                //when we hover over a disabled item, we want to highlight the cell(s) that are why this number cannot be selected.
                                int FindNumber = int.Parse(BuildItem.Text);
                                HashSet<SodokuCell> FoundNumbered = new HashSet<SodokuCell>();
                                //look through the selected item's row...
                                foreach(var findrow in GameBoard.GetRow(SelectedCell.Y))
                                {
                                    if(findrow.Value==FindNumber)
                                        FoundNumbered.Add(findrow);
                                }
                                foreach(var findcol in GameBoard.GetColumn(SelectedCell.X))
                                {
                                    if (findcol.Value == FindNumber)
                                        FoundNumbered.Add(findcol);
                                }
                                foreach(var searchmini in GameBoard.GetMiniGrid(SelectedCell).AllCells())
                                {
                                    if(searchmini.Value==FindNumber)
                                    {
                                        if(!FoundNumbered.Contains(searchmini))
                                        {
                                            FoundNumbered.Add(searchmini);
                                        }
                                    }
                                }
                                GameBoard.SetHighlight(FoundNumbered);
                                PicSodoku.Invalidate();
                                PicSodoku.Refresh();

                            };
                            
                            cms.Closing += (o3, ev3) =>
                            {
                                GameBoard.ClearHighlight();
                                PicSodoku.Invalidate();
                                PicSodoku.Refresh();
                            };
                        }
                        
                        //BuildItem.Enabled = ValidValues.Contains(i);
                        cms.Items.Add(BuildItem);
                    }
                    ToolStripMenuItem ClearItem = new ToolStripMenuItem("Clear");
                    ClearItem.Click += (o, ev) =>
                    {
                        PushBoardState();
                        SelectedCell.Value = 0;
                        PicSodoku.Invalidate();
                        PicSodoku.Refresh();
                    };
                    if (SelectedCell.Value != 0)
                        cms.Items.Add(ClearItem);

                    cms.Opening += (Contextstrip, arguments) => { ((ContextMenuStrip)Contextstrip).Renderer = new Win10MenuRenderer(); };
                    cms.Opened += Cms_Opened;

                    cms.Show(PicSodoku, e.X, e.Y);


                }
            }

            
            

        }

        private void Cms_Opened(object sender, EventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            if (cms.Renderer is Win10MenuRenderer) 
            {
                Win10MenuRenderer.DWMNativeMethods.DWM_BLURBEHIND bb = new Win10MenuRenderer.DWMNativeMethods.DWM_BLURBEHIND(true);
                Win10MenuRenderer.DWMNativeMethods.EnableBlur(cms.Handle);
            }
            else
            {
                Win10MenuRenderer.DWMNativeMethods.EnableBlur(cms.Handle, false);
            }
        }

        private void solvePuzzleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PushBoardState();
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
        private Stack<int[,]> UndoStack = new Stack<int[,]>();
        private Stack<int[,]> RedoStack = new Stack<int[,]>();

        public void PushBoardState()
        {
            var boardstate = GameBoard.GetBoardState();
            UndoStack.Push(boardstate);
            RedoStack.Clear();
        }
        public void UndoState()
        {
            var popped = UndoStack.Pop();
            var CurrentState = GameBoard.GetBoardState();
            RedoStack.Push(CurrentState);
            GameBoard.SetBoardState(popped);

        }
        public void RedoState()
        {
            var popped = RedoStack.Pop();
            UndoStack.Push(popped);
            GameBoard.SetBoardState(popped);
        }
        public void ClearUndoStacks()
        {
            UndoStack.Clear();
            RedoStack.Clear();
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
                    PushBoardState();
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
                    StartPuzzleTime = DateTime.Now;
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
            int emptycells = getEmptyCellCountForDifficulty();
            GameBoard = SodokuBoard.GeneratePuzzle(emptycells);
            BoardDrawHandler.GameBoard = GameBoard;
            PicSodoku.Invalidate();
            PicSodoku.Refresh();
            StartPuzzleTime = DateTime.Now;
        }
        private int getEmptyCellCountForDifficulty()
        {
            Random rgen = new Random();
            if (easyToolStripMenuItem.Checked)
                return 40 + rgen.Next(5);
            else if(mediumToolStripMenuItem.Checked)
            {
                return 46 + rgen.Next(5);
            }
            else if(hardToolStripMenuItem.Checked)
            {
                return 50 + rgen.Next(3);
            }
            else if(veryHardToolStripMenuItem.Checked)
            {
                return 54 + rgen.Next(4);
            }
            return 45;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BASeDoku_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                PuzzleTimer.Dispose();
            }
            catch(Exception exx)
            {

            }
        }

        private void emptyBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameBoard = new SodokuBoard();
            BoardDrawHandler.GameBoard = GameBoard;
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            easyToolStripMenuItem.Checked = !(mediumToolStripMenuItem.Checked = hardToolStripMenuItem.Checked = veryHardToolStripMenuItem.Checked = false);

        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediumToolStripMenuItem.Checked = !(easyToolStripMenuItem.Checked = hardToolStripMenuItem.Checked = veryHardToolStripMenuItem.Checked = false);
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hardToolStripMenuItem.Checked = !(easyToolStripMenuItem.Checked = mediumToolStripMenuItem.Checked = veryHardToolStripMenuItem.Checked = false);
        }

        private void veryHardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            veryHardToolStripMenuItem.Checked = !(easyToolStripMenuItem.Checked = mediumToolStripMenuItem.Checked = hardToolStripMenuItem.Checked = false);
        }

        private void editToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            redoToolStripMenuItem.Enabled = RedoStack.Count > 0;
            undoToolStripMenuItem.Enabled = UndoStack.Count > 0;
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoState();
            PicSodoku.Invalidate();
            PicSodoku.Refresh();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RedoState();
            PicSodoku.Invalidate();
            PicSodoku.Refresh();
        }

        private void fileToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {

        }
        private void ToolStripMenuDropDown_Opened(object sender,EventArgs e)
        {
            ToolStripMenuItem cms = sender as ToolStripMenuItem;
            var dd = cms.DropDown;
            if (dd.Renderer is Win10MenuRenderer)
            {
                Win10MenuRenderer.DWMNativeMethods.DWM_BLURBEHIND bb = new Win10MenuRenderer.DWMNativeMethods.DWM_BLURBEHIND(true);
                Win10MenuRenderer.DWMNativeMethods.EnableBlur(dd.Handle);
            }
            else
            {
                Win10MenuRenderer.DWMNativeMethods.EnableBlur(dd.Handle, false);
            }
        }

        private void debugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<TimeSpan> Elapsed = new List<TimeSpan>();
            int ExecuteCount = 500;
            int emptycells = getEmptyCellCountForDifficulty();
            TimeSpan MinExec = TimeSpan.MaxValue;
            TimeSpan MaxExec = TimeSpan.MinValue;
            for(int i=0;i<ExecuteCount;i++)
            {
                Stopwatch watcher = new Stopwatch();
                Thread.Sleep(500);
                watcher.Start();
                SodokuBoard GenerateBoard = SodokuBoard.GeneratePuzzle(emptycells);
                watcher.Stop();
                Debug.Print("Puzzle #" + i + " Generated in " + watcher.Elapsed.ToString());
                if (MinExec > watcher.Elapsed) MinExec = watcher.Elapsed;
                if (MaxExec < watcher.Elapsed) MaxExec = watcher.Elapsed;
                Elapsed.Add(watcher.Elapsed);
                //BoardDrawHandler.GameBoard = GameBoard;
                Text = "Debug:" + i;
                //PicSodoku.Invalidate();
                //PicSodoku.Refresh();
            }

            TimeSpan AvgExec = new TimeSpan((int)Elapsed.Average((r) => r.Ticks));

            String BuildStr = "Minimum:" + MinExec.ToString() + "\n" +
                              "Maximum:" + MaxExec.ToString() + "\n" +
                              "Average:" + AvgExec.ToString() + "\n";
            Debug.Print("Minimum:" + MinExec.ToString());
            Debug.Print("Maximum:" + MinExec.ToString());
            Debug.Print("Average:" + AvgExec.ToString());
            MessageBox.Show(BuildStr);

        }
    }
}

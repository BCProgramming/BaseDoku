namespace BASeDoku
{
    partial class BASeDoku
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mStripMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsLockedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.levelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.easyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.veryHardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.hintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.solvePuzzleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statMain = new System.Windows.Forms.StatusStrip();
            this.PanMainView = new System.Windows.Forms.Panel();
            this.PicSodoku = new System.Windows.Forms.PictureBox();
            this.txtInputText = new System.Windows.Forms.TextBox();
            this.emptyBoardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generatePuzzleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mStripMain.SuspendLayout();
            this.PanMainView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PicSodoku)).BeginInit();
            this.SuspendLayout();
            // 
            // mStripMain
            // 
            this.mStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.levelToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mStripMain.Location = new System.Drawing.Point(0, 0);
            this.mStripMain.Name = "mStripMain";
            this.mStripMain.Size = new System.Drawing.Size(432, 28);
            this.mStripMain.TabIndex = 0;
            this.mStripMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.saveAsLockedToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emptyBoardToolStripMenuItem,
            this.generatePuzzleToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
            this.newToolStripMenuItem.Text = "New";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(192, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // saveAsLockedToolStripMenuItem
            // 
            this.saveAsLockedToolStripMenuItem.Name = "saveAsLockedToolStripMenuItem";
            this.saveAsLockedToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
            this.saveAsLockedToolStripMenuItem.Text = "Save As Locked...";
            this.saveAsLockedToolStripMenuItem.Click += new System.EventHandler(this.saveAsLockedToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(192, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(195, 26);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(47, 24);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(120, 26);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(120, 26);
            this.redoToolStripMenuItem.Text = "Redo";
            // 
            // levelToolStripMenuItem
            // 
            this.levelToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.easyToolStripMenuItem,
            this.mediumToolStripMenuItem,
            this.hardToolStripMenuItem,
            this.veryHardToolStripMenuItem,
            this.toolStripMenuItem3,
            this.hintToolStripMenuItem,
            this.solvePuzzleToolStripMenuItem});
            this.levelToolStripMenuItem.Name = "levelToolStripMenuItem";
            this.levelToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.levelToolStripMenuItem.Text = "Level";
            // 
            // easyToolStripMenuItem
            // 
            this.easyToolStripMenuItem.Name = "easyToolStripMenuItem";
            this.easyToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.easyToolStripMenuItem.Text = "Easy";
            // 
            // mediumToolStripMenuItem
            // 
            this.mediumToolStripMenuItem.Name = "mediumToolStripMenuItem";
            this.mediumToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.mediumToolStripMenuItem.Text = "Medium";
            // 
            // hardToolStripMenuItem
            // 
            this.hardToolStripMenuItem.Name = "hardToolStripMenuItem";
            this.hardToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.hardToolStripMenuItem.Text = "Hard";
            // 
            // veryHardToolStripMenuItem
            // 
            this.veryHardToolStripMenuItem.Name = "veryHardToolStripMenuItem";
            this.veryHardToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.veryHardToolStripMenuItem.Text = "Very Hard";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(178, 6);
            // 
            // hintToolStripMenuItem
            // 
            this.hintToolStripMenuItem.Name = "hintToolStripMenuItem";
            this.hintToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.hintToolStripMenuItem.Text = "Hint";
            // 
            // solvePuzzleToolStripMenuItem
            // 
            this.solvePuzzleToolStripMenuItem.Name = "solvePuzzleToolStripMenuItem";
            this.solvePuzzleToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.solvePuzzleToolStripMenuItem.Text = "Solve Puzzle";
            this.solvePuzzleToolStripMenuItem.Click += new System.EventHandler(this.solvePuzzleToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(125, 26);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // statMain
            // 
            this.statMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statMain.Location = new System.Drawing.Point(0, 380);
            this.statMain.Name = "statMain";
            this.statMain.Size = new System.Drawing.Size(432, 22);
            this.statMain.TabIndex = 1;
            // 
            // PanMainView
            // 
            this.PanMainView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanMainView.Controls.Add(this.PicSodoku);
            this.PanMainView.Location = new System.Drawing.Point(0, 31);
            this.PanMainView.Name = "PanMainView";
            this.PanMainView.Size = new System.Drawing.Size(432, 346);
            this.PanMainView.TabIndex = 2;
            // 
            // PicSodoku
            // 
            this.PicSodoku.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PicSodoku.BackColor = System.Drawing.Color.White;
            this.PicSodoku.Location = new System.Drawing.Point(3, 3);
            this.PicSodoku.Name = "PicSodoku";
            this.PicSodoku.Size = new System.Drawing.Size(426, 340);
            this.PicSodoku.TabIndex = 0;
            this.PicSodoku.TabStop = false;
            this.PicSodoku.Paint += new System.Windows.Forms.PaintEventHandler(this.PicSodoku_Paint);
            this.PicSodoku.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PicSodoku_MouseDown);
            // 
            // txtInputText
            // 
            this.txtInputText.Location = new System.Drawing.Point(-305, 380);
            this.txtInputText.Name = "txtInputText";
            this.txtInputText.Size = new System.Drawing.Size(100, 22);
            this.txtInputText.TabIndex = 3;
            this.txtInputText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInputText_KeyPress);
            // 
            // emptyBoardToolStripMenuItem
            // 
            this.emptyBoardToolStripMenuItem.Name = "emptyBoardToolStripMenuItem";
            this.emptyBoardToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
            this.emptyBoardToolStripMenuItem.Text = "Empty Board";
            // 
            // generatePuzzleToolStripMenuItem
            // 
            this.generatePuzzleToolStripMenuItem.Name = "generatePuzzleToolStripMenuItem";
            this.generatePuzzleToolStripMenuItem.Size = new System.Drawing.Size(190, 26);
            this.generatePuzzleToolStripMenuItem.Text = "Generate Puzzle";
            this.generatePuzzleToolStripMenuItem.Click += new System.EventHandler(this.generatePuzzleToolStripMenuItem_Click);
            // 
            // BASeDoku
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(432, 402);
            this.Controls.Add(this.txtInputText);
            this.Controls.Add(this.PanMainView);
            this.Controls.Add(this.statMain);
            this.Controls.Add(this.mStripMain);
            this.MainMenuStrip = this.mStripMain;
            this.Name = "BASeDoku";
            this.Text = "BASeDoku";
            this.Load += new System.EventHandler(this.BASeDoku_Load);
            this.Resize += new System.EventHandler(this.BASeDoku_Resize);
            this.mStripMain.ResumeLayout(false);
            this.mStripMain.PerformLayout();
            this.PanMainView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PicSodoku)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mStripMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem levelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem easyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediumToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem veryHardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statMain;
        private System.Windows.Forms.Panel PanMainView;
        private System.Windows.Forms.PictureBox PicSodoku;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem hintToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem solvePuzzleToolStripMenuItem;
        private System.Windows.Forms.TextBox txtInputText;
        private System.Windows.Forms.ToolStripMenuItem saveAsLockedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emptyBoardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generatePuzzleToolStripMenuItem;
    }
}


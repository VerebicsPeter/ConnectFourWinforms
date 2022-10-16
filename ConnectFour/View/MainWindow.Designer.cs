namespace ConnectFour
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GamePanel = new System.Windows.Forms.Panel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StartGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetSizeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripPauseButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripPlayerLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTimeLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripXLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripOLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // GamePanel
            // 
            this.GamePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GamePanel.Location = new System.Drawing.Point(11, 35);
            this.GamePanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(600, 600);
            this.GamePanel.TabIndex = 0;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(622, 31);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartGameMenuItem,
            this.SaveGameMenuItem,
            this.LoadGameMenuItem,
            this.SetSizeMenuItem,
            this.ExitMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(69, 27);
            this.fileToolStripMenuItem.Text = "Game";
            // 
            // StartGameMenuItem
            // 
            this.StartGameMenuItem.Name = "StartGameMenuItem";
            this.StartGameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.StartGameMenuItem.Size = new System.Drawing.Size(238, 28);
            this.StartGameMenuItem.Text = "Start Game";
            // 
            // SaveGameMenuItem
            // 
            this.SaveGameMenuItem.Name = "SaveGameMenuItem";
            this.SaveGameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveGameMenuItem.Size = new System.Drawing.Size(238, 28);
            this.SaveGameMenuItem.Text = "Save Game";
            // 
            // LoadGameMenuItem
            // 
            this.LoadGameMenuItem.Name = "LoadGameMenuItem";
            this.LoadGameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.LoadGameMenuItem.Size = new System.Drawing.Size(238, 28);
            this.LoadGameMenuItem.Text = "Load Game";
            // 
            // SetSizeMenuItem
            // 
            this.SetSizeMenuItem.Name = "SetSizeMenuItem";
            this.SetSizeMenuItem.Size = new System.Drawing.Size(238, 28);
            this.SetSizeMenuItem.Text = "Set Game Size";
            this.SetSizeMenuItem.Click += new System.EventHandler(this.SetSizeMenuItem_Click);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(238, 28);
            this.ExitMenuItem.Text = "Exit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(71, 27);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.toolStripPauseButton,
            this.toolStripSeparator2,
            this.toolStripPlayerLabel,
            this.toolStripSeparator3,
            this.toolStripTimeLabel,
            this.toolStripSeparator4,
            this.toolStripXLabel,
            this.toolStripSeparator5,
            this.toolStripOLabel});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip.Location = new System.Drawing.Point(0, 643);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(622, 30);
            this.toolStrip.TabIndex = 2;
            this.toolStrip.Text = "toolStrip";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripPauseButton
            // 
            this.toolStripPauseButton.Image = global::ConnectFour.Properties.Resources.pause;
            this.toolStripPauseButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPauseButton.Name = "toolStripPauseButton";
            this.toolStripPauseButton.Size = new System.Drawing.Size(78, 27);
            this.toolStripPauseButton.Text = "Pause";
            this.toolStripPauseButton.Click += new System.EventHandler(this.ToolStripPauseButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripPlayerLabel
            // 
            this.toolStripPlayerLabel.Name = "toolStripPlayerLabel";
            this.toolStripPlayerLabel.Size = new System.Drawing.Size(65, 23);
            this.toolStripPlayerLabel.Text = "Player: ";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripTimeLabel
            // 
            this.toolStripTimeLabel.Name = "toolStripTimeLabel";
            this.toolStripTimeLabel.Size = new System.Drawing.Size(56, 23);
            this.toolStripTimeLabel.Text = "Time: ";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
            // 
            // toolStripXLabel
            // 
            this.toolStripXLabel.Name = "toolStripXLabel";
            this.toolStripXLabel.Size = new System.Drawing.Size(71, 23);
            this.toolStripXLabel.Text = "X Time: ";
            // 
            // toolStripOLabel
            // 
            this.toolStripOLabel.Name = "toolStripOLabel";
            this.toolStripOLabel.Size = new System.Drawing.Size(74, 23);
            this.toolStripOLabel.Text = "O Time: ";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 673);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.GamePanel);
            this.Controls.Add(this.menuStrip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.Text = "Connect Four";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel GamePanel;
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem StartGameMenuItem;
        private ToolStripMenuItem SetSizeMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem LoadGameMenuItem;
        private ToolStripMenuItem ExitMenuItem;
        private ToolStrip toolStrip;
        private ToolStripLabel toolStripPlayerLabel;
        private ToolStripLabel toolStripTimeLabel;
        private ToolStripMenuItem SaveGameMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton toolStripPauseButton;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripLabel toolStripXLabel;
        private ToolStripLabel toolStripOLabel;
        private ToolStripSeparator toolStripSeparator5;
    }
}
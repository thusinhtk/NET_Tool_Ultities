namespace Ultities
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dBCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareWithCanMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.genScripForBSWShim = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dBCToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(730, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dBCToolStripMenuItem
            // 
            this.dBCToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generataToolStripMenuItem,
            this.compareWithCanMatrixToolStripMenuItem});
            this.dBCToolStripMenuItem.Image = global::Ultities.Properties.Resources.menu;
            this.dBCToolStripMenuItem.Name = "dBCToolStripMenuItem";
            this.dBCToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.M)));
            this.dBCToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.dBCToolStripMenuItem.Text = "Menu";
            this.dBCToolStripMenuItem.Click += new System.EventHandler(this.dBCToolStripMenuItem_Click);
            // 
            // generataToolStripMenuItem
            // 
            this.generataToolStripMenuItem.Image = global::Ultities.Properties.Resources.loading;
            this.generataToolStripMenuItem.Name = "generataToolStripMenuItem";
            this.generataToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.generataToolStripMenuItem.Text = "1.Load CanMatrix file";
            this.generataToolStripMenuItem.Click += new System.EventHandler(this.generateToolStripMenuItem_Click);
            // 
            // compareWithCanMatrixToolStripMenuItem
            // 
            this.compareWithCanMatrixToolStripMenuItem.Image = global::Ultities.Properties.Resources.generate;
            this.compareWithCanMatrixToolStripMenuItem.Name = "compareWithCanMatrixToolStripMenuItem";
            this.compareWithCanMatrixToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.compareWithCanMatrixToolStripMenuItem.Text = "2. Generate FW list";
            this.compareWithCanMatrixToolStripMenuItem.Click += new System.EventHandler(this.compareWithCanMatrixToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.genScripForBSWShim});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // genScripForBSWShim
            // 
            this.genScripForBSWShim.Name = "genScripForBSWShim";
            this.genScripForBSWShim.Size = new System.Drawing.Size(214, 22);
            this.genScripForBSWShim.Text = "Generate scipt for BSWSim";
            this.genScripForBSWShim.Click += new System.EventHandler(this.genScripForBSWShim_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::Ultities.Properties.Resources.exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(531, 416);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(199, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "© Copyright 2019 NET Team (RBVH).";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Ultities.Properties.Resources.maxresdefault;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(730, 435);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ultities tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dBCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareWithCanMatrixToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem genScripForBSWShim;
    }
}


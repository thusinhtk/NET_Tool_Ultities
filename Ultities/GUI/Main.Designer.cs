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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dBCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareWithCanMatrixToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dBCToolStripMenuItem});
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
            this.dBCToolStripMenuItem.Name = "dBCToolStripMenuItem";
            this.dBCToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.dBCToolStripMenuItem.Text = "DBC";
            this.dBCToolStripMenuItem.Click += new System.EventHandler(this.dBCToolStripMenuItem_Click);
            // 
            // generataToolStripMenuItem
            // 
            this.generataToolStripMenuItem.Name = "generataToolStripMenuItem";
            this.generataToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.generataToolStripMenuItem.Text = "Generate";
            this.generataToolStripMenuItem.Click += new System.EventHandler(this.generateToolStripMenuItem_Click);
            // 
            // compareWithCanMatrixToolStripMenuItem
            // 
            this.compareWithCanMatrixToolStripMenuItem.Name = "compareWithCanMatrixToolStripMenuItem";
            this.compareWithCanMatrixToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.compareWithCanMatrixToolStripMenuItem.Text = "Compare with can matrix";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 435);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Ultities tool";
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
    }
}


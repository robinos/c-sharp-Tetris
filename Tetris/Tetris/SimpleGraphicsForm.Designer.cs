namespace Tetris
{
    partial class SimpleGraphicsForm
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
            this.GamePanel = new System.Windows.Forms.Panel();
            this.ToolboxPanel = new System.Windows.Forms.Panel();
            this.HelpPanel = new System.Windows.Forms.Panel();
            this.NextPanel = new System.Windows.Forms.Panel();
            this.SidePanel = new System.Windows.Forms.Panel();
            this.InfoPanel = new System.Windows.Forms.Panel();
            this.HelpPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // GamePanel
            // 
            this.GamePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GamePanel.BackColor = System.Drawing.Color.White;
            this.GamePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GamePanel.Cursor = System.Windows.Forms.Cursors.Cross;
            this.GamePanel.Location = new System.Drawing.Point(300, 50);
            this.GamePanel.Margin = new System.Windows.Forms.Padding(0);
            this.GamePanel.Name = "GamePanel";
            this.GamePanel.Size = new System.Drawing.Size(200, 400);
            this.GamePanel.TabIndex = 0;
            // 
            // ToolboxPanel
            // 
            this.ToolboxPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ToolboxPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ToolboxPanel.Location = new System.Drawing.Point(0, 0);
            this.ToolboxPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ToolboxPanel.Name = "ToolboxPanel";
            this.ToolboxPanel.Size = new System.Drawing.Size(784, 50);
            this.ToolboxPanel.TabIndex = 1;
            // 
            // HelpPanel
            // 
            this.HelpPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.HelpPanel.Controls.Add(this.NextPanel);
            this.HelpPanel.Location = new System.Drawing.Point(500, 50);
            this.HelpPanel.Margin = new System.Windows.Forms.Padding(0);
            this.HelpPanel.Name = "HelpPanel";
            this.HelpPanel.Size = new System.Drawing.Size(150, 400);
            this.HelpPanel.TabIndex = 2;
            // 
            // NextPanel
            // 
            this.NextPanel.BackColor = System.Drawing.Color.White;
            this.NextPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.NextPanel.Location = new System.Drawing.Point(15, 15);
            this.NextPanel.Margin = new System.Windows.Forms.Padding(0);
            this.NextPanel.Name = "NextPanel";
            this.NextPanel.Size = new System.Drawing.Size(120, 120);
            this.NextPanel.TabIndex = 0;
            // 
            // SidePanel
            // 
            this.SidePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SidePanel.Location = new System.Drawing.Point(150, 50);
            this.SidePanel.Margin = new System.Windows.Forms.Padding(0);
            this.SidePanel.Name = "SidePanel";
            this.SidePanel.Size = new System.Drawing.Size(150, 400);
            this.SidePanel.TabIndex = 3;
            // 
            // InfoPanel
            // 
            this.InfoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InfoPanel.Location = new System.Drawing.Point(3, 449);
            this.InfoPanel.Name = "InfoPanel";
            this.InfoPanel.Size = new System.Drawing.Size(780, 113);
            this.InfoPanel.TabIndex = 4;
            // 
            // SimpleGraphicsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.GamePanel);
            this.Controls.Add(this.InfoPanel);
            this.Controls.Add(this.SidePanel);
            this.Controls.Add(this.HelpPanel);
            this.Controls.Add(this.ToolboxPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "SimpleGraphicsForm";
            this.Text = "Test";
            this.HelpPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel GamePanel;
        private System.Windows.Forms.Panel ToolboxPanel;
        private System.Windows.Forms.Panel HelpPanel;
        private System.Windows.Forms.Panel NextPanel;
        private System.Windows.Forms.Panel SidePanel;
        private System.Windows.Forms.Panel InfoPanel;
    }
}


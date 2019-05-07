namespace csgoDemoParser
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.VectorMapPicturePanel = new System.Windows.Forms.Panel();
            this.VectorMapPictureBox = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.HelpButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.ParseDemFileToCSV_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seperatePlayercsvIntoPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripDropDownButton();
            this.AutoSamplingExperimentToolStripMenuItem_Button = new System.Windows.Forms.ToolStripMenuItem();
            this.ResultsToolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.combineResultsCSVFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.testAgainstControlDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomScrollBar = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.VectorMapPicturePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VectorMapPictureBox)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // VectorMapPicturePanel
            // 
            this.VectorMapPicturePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VectorMapPicturePanel.AutoScroll = true;
            this.VectorMapPicturePanel.BackColor = System.Drawing.SystemColors.Window;
            this.VectorMapPicturePanel.Controls.Add(this.VectorMapPictureBox);
            this.VectorMapPicturePanel.Location = new System.Drawing.Point(12, 56);
            this.VectorMapPicturePanel.Name = "VectorMapPicturePanel";
            this.VectorMapPicturePanel.Size = new System.Drawing.Size(1222, 382);
            this.VectorMapPicturePanel.TabIndex = 17;
            // 
            // VectorMapPictureBox
            // 
            this.VectorMapPictureBox.Location = new System.Drawing.Point(4, 4);
            this.VectorMapPictureBox.Name = "VectorMapPictureBox";
            this.VectorMapPictureBox.Size = new System.Drawing.Size(1080, 1080);
            this.VectorMapPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.VectorMapPictureBox.TabIndex = 0;
            this.VectorMapPictureBox.TabStop = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.HelpButton,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.ResultsToolStripDropDownButton3,
            this.toolStripDropDownButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1246, 30);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // HelpButton
            // 
            this.HelpButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.HelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.HelpButton.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HelpButton.Image = ((System.Drawing.Image)(resources.GetObject("HelpButton.Image")));
            this.HelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(52, 27);
            this.HelpButton.Text = "Help";
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ParseDemFileToCSV_ToolStripMenuItem});
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(82, 27);
            this.toolStripButton1.Text = "Dem File";
            // 
            // ParseDemFileToCSV_ToolStripMenuItem
            // 
            this.ParseDemFileToCSV_ToolStripMenuItem.Name = "ParseDemFileToCSV_ToolStripMenuItem";
            this.ParseDemFileToCSV_ToolStripMenuItem.Size = new System.Drawing.Size(225, 26);
            this.ParseDemFileToCSV_ToolStripMenuItem.Text = "Parse .dem file to .csv";
            this.ParseDemFileToCSV_ToolStripMenuItem.Click += new System.EventHandler(this.ParseDemFileToCSV_ToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem,
            this.seperatePlayercsvIntoPathsToolStripMenuItem});
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(76, 27);
            this.toolStripButton2.Text = "CSV File";
            // 
            // SeperateRawDataCSVIntoPlayers_ToolStripMenuItem
            // 
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem.Name = "SeperateRawDataCSVIntoPlayers_ToolStripMenuItem";
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem.Size = new System.Drawing.Size(348, 26);
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem.Text = "Seperate rawData.csv file(s) into players";
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem.Click += new System.EventHandler(this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem_Click);
            // 
            // seperatePlayercsvIntoPathsToolStripMenuItem
            // 
            this.seperatePlayercsvIntoPathsToolStripMenuItem.Name = "seperatePlayercsvIntoPathsToolStripMenuItem";
            this.seperatePlayercsvIntoPathsToolStripMenuItem.Size = new System.Drawing.Size(348, 26);
            this.seperatePlayercsvIntoPathsToolStripMenuItem.Text = "Seperate player#.csv file(s) into paths";
            this.seperatePlayercsvIntoPathsToolStripMenuItem.Click += new System.EventHandler(this.SeperatePlayercsvIntoPaths_ToolStripMenuItem_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem});
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(105, 27);
            this.toolStripButton3.Text = "Debug tools";
            // 
            // CheckMapNamesOfdemSamples_ToolStripMenuItem
            // 
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem.Name = "CheckMapNamesOfdemSamples_ToolStripMenuItem";
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem.Size = new System.Drawing.Size(460, 26);
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem.Text = "Check map names of .dem samples";
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem.Click += new System.EventHandler(this.CheckMapNamesOfdemSamples_ToolStripMenuItem_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AutoSamplingExperimentToolStripMenuItem_Button});
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(98, 27);
            this.toolStripButton4.Text = "Experiment";
            // 
            // AutoSamplingExperimentToolStripMenuItem_Button
            // 
            this.AutoSamplingExperimentToolStripMenuItem_Button.Name = "AutoSamplingExperimentToolStripMenuItem_Button";
            this.AutoSamplingExperimentToolStripMenuItem_Button.Size = new System.Drawing.Size(344, 26);
            this.AutoSamplingExperimentToolStripMenuItem_Button.Text = "Auto sampling experiment";
            this.AutoSamplingExperimentToolStripMenuItem_Button.Click += new System.EventHandler(this.AutoSamplingExperimentToolStripMenuItem_Button_Click);
            // 
            // ResultsToolStripDropDownButton3
            // 
            this.ResultsToolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.combineResultsCSVFilesToolStripMenuItem});
            this.ResultsToolStripDropDownButton3.Name = "ResultsToolStripDropDownButton3";
            this.ResultsToolStripDropDownButton3.Size = new System.Drawing.Size(69, 27);
            this.ResultsToolStripDropDownButton3.Text = "Results";
            // 
            // combineResultsCSVFilesToolStripMenuItem
            // 
            this.combineResultsCSVFilesToolStripMenuItem.Name = "combineResultsCSVFilesToolStripMenuItem";
            this.combineResultsCSVFilesToolStripMenuItem.Size = new System.Drawing.Size(250, 26);
            this.combineResultsCSVFilesToolStripMenuItem.Text = "Combine results.CSV files";
            this.combineResultsCSVFilesToolStripMenuItem.Click += new System.EventHandler(this.combineResultsCSVFilesToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testAgainstControlDataToolStripMenuItem});
            this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(49, 27);
            this.toolStripDropDownButton3.Text = "Test";
            // 
            // testAgainstControlDataToolStripMenuItem
            // 
            this.testAgainstControlDataToolStripMenuItem.Name = "testAgainstControlDataToolStripMenuItem";
            this.testAgainstControlDataToolStripMenuItem.Size = new System.Drawing.Size(247, 26);
            this.testAgainstControlDataToolStripMenuItem.Text = "Test against control data";
            this.testAgainstControlDataToolStripMenuItem.Click += new System.EventHandler(this.testAgainstControlDataToolStripMenuItem_Click);
            // 
            // ZoomScrollBar
            // 
            this.ZoomScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ZoomScrollBar.Location = new System.Drawing.Point(713, 35);
            this.ZoomScrollBar.Name = "ZoomScrollBar";
            this.ZoomScrollBar.Size = new System.Drawing.Size(500, 14);
            this.ZoomScrollBar.TabIndex = 1;
            this.ZoomScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.ZoomScrollBar_Scroll);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(620, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "Zoom image:";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1246, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ZoomScrollBar);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.VectorMapPicturePanel);
            this.Name = "mainForm";
            this.Text = "CSGO Demo Parser";
            this.VectorMapPicturePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.VectorMapPictureBox)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel VectorMapPicturePanel;
        private System.Windows.Forms.PictureBox VectorMapPictureBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem ParseDemFileToCSV_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem SeperateRawDataCSVIntoPlayers_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seperatePlayercsvIntoPathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton3;
        private System.Windows.Forms.ToolStripMenuItem CheckMapNamesOfdemSamples_ToolStripMenuItem;
        private new System.Windows.Forms.ToolStripButton HelpButton;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton4;
        private System.Windows.Forms.HScrollBar ZoomScrollBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem AutoSamplingExperimentToolStripMenuItem_Button;
        private System.Windows.Forms.ToolStripDropDownButton ResultsToolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem combineResultsCSVFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem testAgainstControlDataToolStripMenuItem;
    }
}


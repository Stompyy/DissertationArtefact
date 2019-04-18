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
            this.CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.ConnectToDatabase_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToExistingDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewDatabaseFromMastercsvFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.QueryMinMaxPositionValues_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripDropDownButton();
            this.LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPlayerPathsToExperimentUponToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PerformExperiment_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.loadMasterCSVIntoMemory_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZoomScrollBar = new System.Windows.Forms.HScrollBar();
            this.label1 = new System.Windows.Forms.Label();
            this.AutoSamplingExperimentToolStripMenuItem_Button = new System.Windows.Forms.ToolStripMenuItem();
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
            this.toolStripDropDownButton1,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripDropDownButton2});
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
            this.seperatePlayercsvIntoPathsToolStripMenuItem,
            this.CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem});
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(76, 27);
            this.toolStripButton2.Text = "CSV File";
            // 
            // SeperateRawDataCSVIntoPlayers_ToolStripMenuItem
            // 
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem.Name = "SeperateRawDataCSVIntoPlayers_ToolStripMenuItem";
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem.Text = "Seperate rawData.csv file(s) into players";
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem.Click += new System.EventHandler(this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem_Click);
            // 
            // seperatePlayercsvIntoPathsToolStripMenuItem
            // 
            this.seperatePlayercsvIntoPathsToolStripMenuItem.Name = "seperatePlayercsvIntoPathsToolStripMenuItem";
            this.seperatePlayercsvIntoPathsToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.seperatePlayercsvIntoPathsToolStripMenuItem.Text = "Seperate player#.csv file(s) into paths";
            this.seperatePlayercsvIntoPathsToolStripMenuItem.Click += new System.EventHandler(this.SeperatePlayercsvIntoPaths_ToolStripMenuItem_Click);
            // 
            // CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem
            // 
            this.CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem.Name = "CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem";
            this.CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem.Size = new System.Drawing.Size(363, 26);
            this.CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem.Text = "Combine multiple .csv files into master.csv";
            this.CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem.Click += new System.EventHandler(this.CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ConnectToDatabase_ToolStripMenuItem,
            this.QueryMinMaxPositionValues_ToolStripMenuItem,
            this.CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem,
            this.CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(86, 27);
            this.toolStripDropDownButton1.Text = "Database";
            // 
            // ConnectToDatabase_ToolStripMenuItem
            // 
            this.ConnectToDatabase_ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToExistingDatabaseToolStripMenuItem,
            this.createNewDatabaseFromMastercsvFileToolStripMenuItem});
            this.ConnectToDatabase_ToolStripMenuItem.Name = "ConnectToDatabase_ToolStripMenuItem";
            this.ConnectToDatabase_ToolStripMenuItem.Size = new System.Drawing.Size(503, 26);
            this.ConnectToDatabase_ToolStripMenuItem.Text = "Connect to database";
            // 
            // connectToExistingDatabaseToolStripMenuItem
            // 
            this.connectToExistingDatabaseToolStripMenuItem.Name = "connectToExistingDatabaseToolStripMenuItem";
            this.connectToExistingDatabaseToolStripMenuItem.Size = new System.Drawing.Size(356, 26);
            this.connectToExistingDatabaseToolStripMenuItem.Text = "Connect to existing Database";
            this.connectToExistingDatabaseToolStripMenuItem.Click += new System.EventHandler(this.ConnectToExistingDatabase_ToolStripMenuItem_Click);
            // 
            // createNewDatabaseFromMastercsvFileToolStripMenuItem
            // 
            this.createNewDatabaseFromMastercsvFileToolStripMenuItem.Name = "createNewDatabaseFromMastercsvFileToolStripMenuItem";
            this.createNewDatabaseFromMastercsvFileToolStripMenuItem.Size = new System.Drawing.Size(356, 26);
            this.createNewDatabaseFromMastercsvFileToolStripMenuItem.Text = "Create new database from master.csv file";
            this.createNewDatabaseFromMastercsvFileToolStripMenuItem.Click += new System.EventHandler(this.CreateNewDatabaseFromMastercsvFile_ToolStripMenuItem_Click);
            // 
            // QueryMinMaxPositionValues_ToolStripMenuItem
            // 
            this.QueryMinMaxPositionValues_ToolStripMenuItem.Enabled = false;
            this.QueryMinMaxPositionValues_ToolStripMenuItem.Name = "QueryMinMaxPositionValues_ToolStripMenuItem";
            this.QueryMinMaxPositionValues_ToolStripMenuItem.Size = new System.Drawing.Size(503, 26);
            this.QueryMinMaxPositionValues_ToolStripMenuItem.Text = "Query min max position values";
            this.QueryMinMaxPositionValues_ToolStripMenuItem.Click += new System.EventHandler(this.QueryMinMaxPositionValues_ToolStripMenuItem_Click);
            // 
            // CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem
            // 
            this.CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem.Enabled = false;
            this.CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem.Name = "CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem";
            this.CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem.Size = new System.Drawing.Size(503, 26);
            this.CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem.Text = "Create average velocity trend MasterVelocity.csv from database";
            this.CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem.Click += new System.EventHandler(this.CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem_Click);
            // 
            // CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem
            // 
            this.CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem.Name = "CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem";
            this.CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem.Size = new System.Drawing.Size(503, 26);
            this.CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem.Text = "Combine velocityColumn#.csv files if querying seperately main";
            this.CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem.Click += new System.EventHandler(this.CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem,
            this.CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem});
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
            // CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem
            // 
            this.CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem.Name = "CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem";
            this.CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem.Size = new System.Drawing.Size(460, 26);
            this.CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem.Text = "Combine velocityColumn#.csv files if querying seperately";
            this.CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem.Click += new System.EventHandler(this.CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem,
            this.loadPlayerPathsToExperimentUponToolStripMenuItem,
            this.PerformExperiment_ToolStripMenuItem,
            this.AutoSamplingExperimentToolStripMenuItem_Button});
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(98, 27);
            this.toolStripButton4.Text = "Experiment";
            // 
            // LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem
            // 
            this.LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem.Name = "LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem";
            this.LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem.Size = new System.Drawing.Size(344, 26);
            this.LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem.Text = "Load LedReckoning MasterVelocity.CSV";
            this.LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem.Click += new System.EventHandler(this.LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem_Click);
            // 
            // loadPlayerPathsToExperimentUponToolStripMenuItem
            // 
            this.loadPlayerPathsToExperimentUponToolStripMenuItem.Enabled = false;
            this.loadPlayerPathsToExperimentUponToolStripMenuItem.Name = "loadPlayerPathsToExperimentUponToolStripMenuItem";
            this.loadPlayerPathsToExperimentUponToolStripMenuItem.Size = new System.Drawing.Size(344, 26);
            this.loadPlayerPathsToExperimentUponToolStripMenuItem.Text = "Load player paths to experiment upon";
            this.loadPlayerPathsToExperimentUponToolStripMenuItem.Click += new System.EventHandler(this.LoadPlayerPathsToExperimentUpon_ToolStripMenuItem_Click);
            // 
            // PerformExperiment_ToolStripMenuItem
            // 
            this.PerformExperiment_ToolStripMenuItem.Enabled = false;
            this.PerformExperiment_ToolStripMenuItem.Name = "PerformExperiment_ToolStripMenuItem";
            this.PerformExperiment_ToolStripMenuItem.Size = new System.Drawing.Size(344, 26);
            this.PerformExperiment_ToolStripMenuItem.Text = "Perform experiment";
            this.PerformExperiment_ToolStripMenuItem.Click += new System.EventHandler(this.PerformExperiment_ToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadMasterCSVIntoMemory_ToolStripMenuItem});
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(92, 27);
            this.toolStripDropDownButton2.Text = "Optimised";
            // 
            // loadMasterCSVIntoMemory_ToolStripMenuItem
            // 
            this.loadMasterCSVIntoMemory_ToolStripMenuItem.Name = "loadMasterCSVIntoMemory_ToolStripMenuItem";
            this.loadMasterCSVIntoMemory_ToolStripMenuItem.Size = new System.Drawing.Size(269, 26);
            this.loadMasterCSVIntoMemory_ToolStripMenuItem.Text = "LoadMasterCSVIntoMemory";
            this.loadMasterCSVIntoMemory_ToolStripMenuItem.Click += new System.EventHandler(this.loadMasterCSVIntoMemory_ToolStripMenuItem_Click);
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
            // AutoSamplingExperimentToolStripMenuItem_Button
            // 
            this.AutoSamplingExperimentToolStripMenuItem_Button.Name = "AutoSamplingExperimentToolStripMenuItem_Button";
            this.AutoSamplingExperimentToolStripMenuItem_Button.Size = new System.Drawing.Size(344, 26);
            this.AutoSamplingExperimentToolStripMenuItem_Button.Text = "Auto sampling experiment";
            this.AutoSamplingExperimentToolStripMenuItem_Button.Click += new System.EventHandler(this.AutoSamplingExperimentToolStripMenuItem_Button_Click);
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
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem ConnectToDatabase_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton3;
        private System.Windows.Forms.ToolStripMenuItem CheckMapNamesOfdemSamples_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem QueryMinMaxPositionValues_ToolStripMenuItem;
        private new System.Windows.Forms.ToolStripButton HelpButton;
        private System.Windows.Forms.ToolStripMenuItem connectToExistingDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewDatabaseFromMastercsvFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateAverageVelocityTrendMasterCSVFromDatabase_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton4;
        private System.Windows.Forms.ToolStripMenuItem LoadLedReckoningMasterVelocityCSV_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPlayerPathsToExperimentUponToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PerformExperiment_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CombineVelocityColumnCSVFilesIfQueryingSeperately_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CombineVelocityColumncsvFilesIfQueryingSeperatelyMain_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem loadMasterCSVIntoMemory_ToolStripMenuItem;
        private System.Windows.Forms.HScrollBar ZoomScrollBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem AutoSamplingExperimentToolStripMenuItem_Button;
    }
}


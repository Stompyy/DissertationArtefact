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
            this.GetMinMaxButton = new System.Windows.Forms.Button();
            this.ConnectToDatabaseButton = new System.Windows.Forms.Button();
            this.CreateSuperVelocityCSVButton = new System.Windows.Forms.Button();
            this.CombineVelCSVs = new System.Windows.Forms.Button();
            this.PerformExperimentButton = new System.Windows.Forms.Button();
            this.LoadLedReckoningMasterCSVButton = new System.Windows.Forms.Button();
            this.VectorMapPicturePanel = new System.Windows.Forms.Panel();
            this.VectorMapPictureBox = new System.Windows.Forms.PictureBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.ParseDemFileToCSV_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.SeperateRawDataCSVIntoPlayers_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seperatePlayercsvIntoPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.ConnectToExistingDatabase_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.QueryMinMaxPositionValues_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpButton = new System.Windows.Forms.ToolStripButton();
            this.VectorMapPicturePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.VectorMapPictureBox)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GetMinMaxButton
            // 
            this.GetMinMaxButton.Location = new System.Drawing.Point(0, 0);
            this.GetMinMaxButton.Name = "GetMinMaxButton";
            this.GetMinMaxButton.Size = new System.Drawing.Size(75, 23);
            this.GetMinMaxButton.TabIndex = 20;
            // 
            // ConnectToDatabaseButton
            // 
            this.ConnectToDatabaseButton.Location = new System.Drawing.Point(0, 0);
            this.ConnectToDatabaseButton.Name = "ConnectToDatabaseButton";
            this.ConnectToDatabaseButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectToDatabaseButton.TabIndex = 19;
            // 
            // CreateSuperVelocityCSVButton
            // 
            this.CreateSuperVelocityCSVButton.Location = new System.Drawing.Point(12, 220);
            this.CreateSuperVelocityCSVButton.Name = "CreateSuperVelocityCSVButton";
            this.CreateSuperVelocityCSVButton.Size = new System.Drawing.Size(251, 39);
            this.CreateSuperVelocityCSVButton.TabIndex = 10;
            this.CreateSuperVelocityCSVButton.Text = "CreateSuperVelocityCSV!!!";
            this.CreateSuperVelocityCSVButton.UseVisualStyleBackColor = true;
            this.CreateSuperVelocityCSVButton.Click += new System.EventHandler(this.CreateSuperVelocityCSVButton_Click);
            // 
            // CombineVelCSVs
            // 
            this.CombineVelCSVs.Location = new System.Drawing.Point(12, 265);
            this.CombineVelCSVs.Name = "CombineVelCSVs";
            this.CombineVelCSVs.Size = new System.Drawing.Size(251, 39);
            this.CombineVelCSVs.TabIndex = 12;
            this.CombineVelCSVs.Text = "Combine Vel csvs";
            this.CombineVelCSVs.UseVisualStyleBackColor = true;
            this.CombineVelCSVs.Click += new System.EventHandler(this.CombineVelCSVs_Click);
            // 
            // PerformExperimentButton
            // 
            this.PerformExperimentButton.Location = new System.Drawing.Point(12, 354);
            this.PerformExperimentButton.Name = "PerformExperimentButton";
            this.PerformExperimentButton.Size = new System.Drawing.Size(251, 84);
            this.PerformExperimentButton.TabIndex = 13;
            this.PerformExperimentButton.Text = "Perform Experiment";
            this.PerformExperimentButton.UseVisualStyleBackColor = true;
            this.PerformExperimentButton.Click += new System.EventHandler(this.PerformExperimentButton_Click);
            // 
            // LoadLedReckoningMasterCSVButton
            // 
            this.LoadLedReckoningMasterCSVButton.Location = new System.Drawing.Point(12, 310);
            this.LoadLedReckoningMasterCSVButton.Name = "LoadLedReckoningMasterCSVButton";
            this.LoadLedReckoningMasterCSVButton.Size = new System.Drawing.Size(251, 39);
            this.LoadLedReckoningMasterCSVButton.TabIndex = 14;
            this.LoadLedReckoningMasterCSVButton.Text = "Load Led Reckoning Master.csv";
            this.LoadLedReckoningMasterCSVButton.UseVisualStyleBackColor = true;
            this.LoadLedReckoningMasterCSVButton.Click += new System.EventHandler(this.LoadLedReckoningMasterCSVButton_Click);
            // 
            // VectorMapPicturePanel
            // 
            this.VectorMapPicturePanel.AutoScroll = true;
            this.VectorMapPicturePanel.Controls.Add(this.VectorMapPictureBox);
            this.VectorMapPicturePanel.Location = new System.Drawing.Point(269, 69);
            this.VectorMapPicturePanel.Name = "VectorMapPicturePanel";
            this.VectorMapPicturePanel.Size = new System.Drawing.Size(965, 369);
            this.VectorMapPicturePanel.TabIndex = 17;
            // 
            // VectorMapPictureBox
            // 
            this.VectorMapPictureBox.Location = new System.Drawing.Point(4, 4);
            this.VectorMapPictureBox.Name = "VectorMapPictureBox";
            this.VectorMapPictureBox.Size = new System.Drawing.Size(1080, 1080);
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
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1246, 27);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ParseDemFileToCSV_ToolStripMenuItem});
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(82, 24);
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
            this.toolStripButton2.Size = new System.Drawing.Size(76, 24);
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
            this.seperatePlayercsvIntoPathsToolStripMenuItem.Click += new System.EventHandler(this.seperatePlayercsvIntoPathsToolStripMenuItem_Click);
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
            this.ConnectToExistingDatabase_ToolStripMenuItem,
            this.CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem,
            this.QueryMinMaxPositionValues_ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(86, 24);
            this.toolStripDropDownButton1.Text = "Database";
            // 
            // ConnectToExistingDatabase_ToolStripMenuItem
            // 
            this.ConnectToExistingDatabase_ToolStripMenuItem.Name = "ConnectToExistingDatabase_ToolStripMenuItem";
            this.ConnectToExistingDatabase_ToolStripMenuItem.Size = new System.Drawing.Size(356, 26);
            this.ConnectToExistingDatabase_ToolStripMenuItem.Text = "Connect to existing database";
            this.ConnectToExistingDatabase_ToolStripMenuItem.Click += new System.EventHandler(this.ConnectToExistingDatabase_ToolStripMenuItem_Click);
            // 
            // CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem
            // 
            this.CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem.Name = "CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem";
            this.CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem.Size = new System.Drawing.Size(356, 26);
            this.CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem.Text = "Create new database from master.csv file";
            this.CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem.Click += new System.EventHandler(this.CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem_Click);
            // 
            // QueryMinMaxPositionValues_ToolStripMenuItem
            // 
            this.QueryMinMaxPositionValues_ToolStripMenuItem.Name = "QueryMinMaxPositionValues_ToolStripMenuItem";
            this.QueryMinMaxPositionValues_ToolStripMenuItem.Size = new System.Drawing.Size(356, 26);
            this.QueryMinMaxPositionValues_ToolStripMenuItem.Text = "Query min max position values";
            this.QueryMinMaxPositionValues_ToolStripMenuItem.Click += new System.EventHandler(this.QueryMinMaxPositionValues_ToolStripMenuItem_Click);
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
            this.toolStripButton3.Size = new System.Drawing.Size(105, 24);
            this.toolStripButton3.Text = "Debug tools";
            // 
            // CheckMapNamesOfdemSamples_ToolStripMenuItem
            // 
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem.Name = "CheckMapNamesOfdemSamples_ToolStripMenuItem";
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem.Size = new System.Drawing.Size(317, 26);
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem.Text = "Check map names of .dem samples";
            this.CheckMapNamesOfdemSamples_ToolStripMenuItem.Click += new System.EventHandler(this.CheckMapNamesOfdemSamples_ToolStripMenuItem_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.BackColor = System.Drawing.SystemColors.ControlLight;
            this.HelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.HelpButton.Image = ((System.Drawing.Image)(resources.GetObject("HelpButton.Image")));
            this.HelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(45, 24);
            this.HelpButton.Text = "Help";
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(1246, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.VectorMapPicturePanel);
            this.Controls.Add(this.LoadLedReckoningMasterCSVButton);
            this.Controls.Add(this.PerformExperimentButton);
            this.Controls.Add(this.CombineVelCSVs);
            this.Controls.Add(this.CreateSuperVelocityCSVButton);
            this.Controls.Add(this.ConnectToDatabaseButton);
            this.Controls.Add(this.GetMinMaxButton);
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
        private System.Windows.Forms.Button GetMinMaxButton;
        private System.Windows.Forms.Button ConnectToDatabaseButton;
        private System.Windows.Forms.Button CreateSuperVelocityCSVButton;
        private System.Windows.Forms.Button CombineVelCSVs;
        private System.Windows.Forms.Button PerformExperimentButton;
        private System.Windows.Forms.Button LoadLedReckoningMasterCSVButton;
        private System.Windows.Forms.Panel VectorMapPicturePanel;
        private System.Windows.Forms.PictureBox VectorMapPictureBox;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem ParseDemFileToCSV_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton2;
        private System.Windows.Forms.ToolStripMenuItem SeperateRawDataCSVIntoPlayers_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seperatePlayercsvIntoPathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem ConnectToExistingDatabase_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateNewDatabaseFromMasterCSVFile_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CombineMultipleCSVFilesIntoMasterCSV_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripButton3;
        private System.Windows.Forms.ToolStripMenuItem CheckMapNamesOfdemSamples_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem QueryMinMaxPositionValues_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton HelpButton;
    }
}


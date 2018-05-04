namespace TowerDefense {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.showVerticesBtn = new System.Windows.Forms.Button();
            this.globalTimer = new System.Windows.Forms.Timer(this.components);
            this.playerGoldLabel = new System.Windows.Forms.Label();
            this.playerGoldAmount = new System.Windows.Forms.Label();
            this.playerLivesLabel = new System.Windows.Forms.Label();
            this.playerLivesAmount = new System.Windows.Forms.Label();
            this.Tower2PB = new System.Windows.Forms.PictureBox();
            this.Tower1PB = new System.Windows.Forms.PictureBox();
            this.GameWorldPB = new System.Windows.Forms.PictureBox();
            this.handSelectPB = new System.Windows.Forms.PictureBox();
            this.deleteTowerBtn = new System.Windows.Forms.Button();
            this.selectedTowerNameLabel = new System.Windows.Forms.Label();
            this.selectedTowerAtkDmgLabel = new System.Windows.Forms.Label();
            this.selectedTowerKillsLabel = new System.Windows.Forms.Label();
            this.selectedTowerASLabel = new System.Windows.Forms.Label();
            this.selectedTowerName = new System.Windows.Forms.Label();
            this.selectedTowerDamage = new System.Windows.Forms.Label();
            this.selectedTowerAS = new System.Windows.Forms.Label();
            this.selectedTowerKills = new System.Windows.Forms.Label();
            this.Tower3PB = new System.Windows.Forms.PictureBox();
            this.selectedTowerShotsFiredLabel = new System.Windows.Forms.Label();
            this.selectedTowerShotsFired = new System.Windows.Forms.Label();
            this.selectedTowerTotalDamageLabel = new System.Windows.Forms.Label();
            this.selectedTowerTotalDamage = new System.Windows.Forms.Label();
            this.DogHouseTowerPB = new System.Windows.Forms.PictureBox();
            this.nextWaveBtn = new System.Windows.Forms.Button();
            this.FuzzyTowerPB = new System.Windows.Forms.PictureBox();
            this.towerDescription = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Tower2PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tower1PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GameWorldPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.handSelectPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tower3PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DogHouseTowerPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FuzzyTowerPB)).BeginInit();
            this.SuspendLayout();
            // 
            // showVerticesBtn
            // 
            this.showVerticesBtn.Location = new System.Drawing.Point(663, 567);
            this.showVerticesBtn.Name = "showVerticesBtn";
            this.showVerticesBtn.Size = new System.Drawing.Size(109, 33);
            this.showVerticesBtn.TabIndex = 3;
            this.showVerticesBtn.Text = "Show Vertices";
            this.showVerticesBtn.UseVisualStyleBackColor = true;
            this.showVerticesBtn.Click += new System.EventHandler(this.showVerticesBtn_Click);
            // 
            // globalTimer
            // 
            this.globalTimer.Tick += new System.EventHandler(this.globalTimer_Tick);
            // 
            // playerGoldLabel
            // 
            this.playerGoldLabel.AutoSize = true;
            this.playerGoldLabel.Location = new System.Drawing.Point(617, 162);
            this.playerGoldLabel.Name = "playerGoldLabel";
            this.playerGoldLabel.Size = new System.Drawing.Size(32, 13);
            this.playerGoldLabel.TabIndex = 4;
            this.playerGoldLabel.Text = "Gold:";
            // 
            // playerGoldAmount
            // 
            this.playerGoldAmount.AutoSize = true;
            this.playerGoldAmount.Location = new System.Drawing.Point(656, 162);
            this.playerGoldAmount.Name = "playerGoldAmount";
            this.playerGoldAmount.Size = new System.Drawing.Size(13, 13);
            this.playerGoldAmount.TabIndex = 5;
            this.playerGoldAmount.Text = "0";
            // 
            // playerLivesLabel
            // 
            this.playerLivesLabel.AutoSize = true;
            this.playerLivesLabel.Location = new System.Drawing.Point(615, 179);
            this.playerLivesLabel.Name = "playerLivesLabel";
            this.playerLivesLabel.Size = new System.Drawing.Size(35, 13);
            this.playerLivesLabel.TabIndex = 6;
            this.playerLivesLabel.Text = "Lives:";
            // 
            // playerLivesAmount
            // 
            this.playerLivesAmount.AutoSize = true;
            this.playerLivesAmount.Location = new System.Drawing.Point(656, 179);
            this.playerLivesAmount.Name = "playerLivesAmount";
            this.playerLivesAmount.Size = new System.Drawing.Size(13, 13);
            this.playerLivesAmount.TabIndex = 7;
            this.playerLivesAmount.Text = "0";
            // 
            // Tower2PB
            // 
            this.Tower2PB.BackColor = System.Drawing.SystemColors.InfoText;
            this.Tower2PB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Tower2PB.Location = new System.Drawing.Point(618, 68);
            this.Tower2PB.Name = "Tower2PB";
            this.Tower2PB.Size = new System.Drawing.Size(50, 50);
            this.Tower2PB.TabIndex = 2;
            this.Tower2PB.TabStop = false;
            this.Tower2PB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tower2PB_MouseDown);
            // 
            // Tower1PB
            // 
            this.Tower1PB.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Tower1PB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Tower1PB.Image = global::TowerDefense.Properties.Resources.ArrowTower;
            this.Tower1PB.InitialImage = ((System.Drawing.Image)(resources.GetObject("Tower1PB.InitialImage")));
            this.Tower1PB.Location = new System.Drawing.Point(618, 12);
            this.Tower1PB.Name = "Tower1PB";
            this.Tower1PB.Size = new System.Drawing.Size(50, 50);
            this.Tower1PB.TabIndex = 1;
            this.Tower1PB.TabStop = false;
            this.Tower1PB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tower1PB_MouseDown);
            // 
            // GameWorldPB
            // 
            this.GameWorldPB.InitialImage = ((System.Drawing.Image)(resources.GetObject("GameWorldPB.InitialImage")));
            this.GameWorldPB.Location = new System.Drawing.Point(0, 0);
            this.GameWorldPB.Name = "GameWorldPB";
            this.GameWorldPB.Size = new System.Drawing.Size(600, 600);
            this.GameWorldPB.TabIndex = 0;
            this.GameWorldPB.TabStop = false;
            this.GameWorldPB.Paint += new System.Windows.Forms.PaintEventHandler(this.GameWorldPB_Paint);
            this.GameWorldPB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.GameWorldPB_MouseDown);
            this.GameWorldPB.MouseLeave += new System.EventHandler(this.GameWorldPB_MouseLeave);
            this.GameWorldPB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.GameWorldPB_MouseMove);
            // 
            // handSelectPB
            // 
            this.handSelectPB.BackColor = System.Drawing.SystemColors.Highlight;
            this.handSelectPB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.handSelectPB.Location = new System.Drawing.Point(674, 12);
            this.handSelectPB.Name = "handSelectPB";
            this.handSelectPB.Size = new System.Drawing.Size(50, 50);
            this.handSelectPB.TabIndex = 8;
            this.handSelectPB.TabStop = false;
            this.handSelectPB.Click += new System.EventHandler(this.handSelectPB_Click);
            // 
            // deleteTowerBtn
            // 
            this.deleteTowerBtn.Location = new System.Drawing.Point(620, 335);
            this.deleteTowerBtn.Name = "deleteTowerBtn";
            this.deleteTowerBtn.Size = new System.Drawing.Size(51, 23);
            this.deleteTowerBtn.TabIndex = 9;
            this.deleteTowerBtn.Text = "Delete";
            this.deleteTowerBtn.UseVisualStyleBackColor = true;
            this.deleteTowerBtn.Visible = false;
            this.deleteTowerBtn.Click += new System.EventHandler(this.deleteTowerBtn_Click);
            // 
            // selectedTowerNameLabel
            // 
            this.selectedTowerNameLabel.AutoSize = true;
            this.selectedTowerNameLabel.Location = new System.Drawing.Point(617, 237);
            this.selectedTowerNameLabel.Name = "selectedTowerNameLabel";
            this.selectedTowerNameLabel.Size = new System.Drawing.Size(38, 13);
            this.selectedTowerNameLabel.TabIndex = 10;
            this.selectedTowerNameLabel.Text = "Name:";
            this.selectedTowerNameLabel.Visible = false;
            // 
            // selectedTowerAtkDmgLabel
            // 
            this.selectedTowerAtkDmgLabel.AutoSize = true;
            this.selectedTowerAtkDmgLabel.Location = new System.Drawing.Point(617, 250);
            this.selectedTowerAtkDmgLabel.Name = "selectedTowerAtkDmgLabel";
            this.selectedTowerAtkDmgLabel.Size = new System.Drawing.Size(50, 13);
            this.selectedTowerAtkDmgLabel.TabIndex = 11;
            this.selectedTowerAtkDmgLabel.Text = "Damage:";
            this.selectedTowerAtkDmgLabel.Visible = false;
            // 
            // selectedTowerKillsLabel
            // 
            this.selectedTowerKillsLabel.AutoSize = true;
            this.selectedTowerKillsLabel.Location = new System.Drawing.Point(617, 301);
            this.selectedTowerKillsLabel.Name = "selectedTowerKillsLabel";
            this.selectedTowerKillsLabel.Size = new System.Drawing.Size(28, 13);
            this.selectedTowerKillsLabel.TabIndex = 12;
            this.selectedTowerKillsLabel.Text = "Kills:";
            this.selectedTowerKillsLabel.Visible = false;
            // 
            // selectedTowerASLabel
            // 
            this.selectedTowerASLabel.AutoSize = true;
            this.selectedTowerASLabel.Location = new System.Drawing.Point(617, 263);
            this.selectedTowerASLabel.Name = "selectedTowerASLabel";
            this.selectedTowerASLabel.Size = new System.Drawing.Size(75, 13);
            this.selectedTowerASLabel.TabIndex = 13;
            this.selectedTowerASLabel.Text = "Attack Speed:";
            this.selectedTowerASLabel.Visible = false;
            // 
            // selectedTowerName
            // 
            this.selectedTowerName.AutoSize = true;
            this.selectedTowerName.Location = new System.Drawing.Point(693, 237);
            this.selectedTowerName.Name = "selectedTowerName";
            this.selectedTowerName.Size = new System.Drawing.Size(10, 13);
            this.selectedTowerName.TabIndex = 14;
            this.selectedTowerName.Text = "-";
            this.selectedTowerName.Visible = false;
            // 
            // selectedTowerDamage
            // 
            this.selectedTowerDamage.AutoSize = true;
            this.selectedTowerDamage.Location = new System.Drawing.Point(696, 250);
            this.selectedTowerDamage.Name = "selectedTowerDamage";
            this.selectedTowerDamage.Size = new System.Drawing.Size(13, 13);
            this.selectedTowerDamage.TabIndex = 15;
            this.selectedTowerDamage.Text = "0";
            this.selectedTowerDamage.Visible = false;
            // 
            // selectedTowerAS
            // 
            this.selectedTowerAS.AutoSize = true;
            this.selectedTowerAS.Location = new System.Drawing.Point(696, 263);
            this.selectedTowerAS.Name = "selectedTowerAS";
            this.selectedTowerAS.Size = new System.Drawing.Size(13, 13);
            this.selectedTowerAS.TabIndex = 16;
            this.selectedTowerAS.Text = "0";
            this.selectedTowerAS.Visible = false;
            // 
            // selectedTowerKills
            // 
            this.selectedTowerKills.AutoSize = true;
            this.selectedTowerKills.Location = new System.Drawing.Point(696, 301);
            this.selectedTowerKills.Name = "selectedTowerKills";
            this.selectedTowerKills.Size = new System.Drawing.Size(13, 13);
            this.selectedTowerKills.TabIndex = 17;
            this.selectedTowerKills.Text = "0";
            this.selectedTowerKills.Visible = false;
            // 
            // Tower3PB
            // 
            this.Tower3PB.BackColor = System.Drawing.Color.DarkRed;
            this.Tower3PB.Location = new System.Drawing.Point(675, 69);
            this.Tower3PB.Name = "Tower3PB";
            this.Tower3PB.Size = new System.Drawing.Size(49, 49);
            this.Tower3PB.TabIndex = 18;
            this.Tower3PB.TabStop = false;
            this.Tower3PB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tower3PB_MouseDown);
            // 
            // selectedTowerShotsFiredLabel
            // 
            this.selectedTowerShotsFiredLabel.AutoSize = true;
            this.selectedTowerShotsFiredLabel.Location = new System.Drawing.Point(617, 276);
            this.selectedTowerShotsFiredLabel.Name = "selectedTowerShotsFiredLabel";
            this.selectedTowerShotsFiredLabel.Size = new System.Drawing.Size(60, 13);
            this.selectedTowerShotsFiredLabel.TabIndex = 19;
            this.selectedTowerShotsFiredLabel.Text = "Shots fired:";
            this.selectedTowerShotsFiredLabel.Visible = false;
            // 
            // selectedTowerShotsFired
            // 
            this.selectedTowerShotsFired.AutoSize = true;
            this.selectedTowerShotsFired.Location = new System.Drawing.Point(696, 276);
            this.selectedTowerShotsFired.Name = "selectedTowerShotsFired";
            this.selectedTowerShotsFired.Size = new System.Drawing.Size(13, 13);
            this.selectedTowerShotsFired.TabIndex = 20;
            this.selectedTowerShotsFired.Text = "0";
            this.selectedTowerShotsFired.Visible = false;
            // 
            // selectedTowerTotalDamageLabel
            // 
            this.selectedTowerTotalDamageLabel.AutoSize = true;
            this.selectedTowerTotalDamageLabel.Location = new System.Drawing.Point(617, 289);
            this.selectedTowerTotalDamageLabel.Name = "selectedTowerTotalDamageLabel";
            this.selectedTowerTotalDamageLabel.Size = new System.Drawing.Size(75, 13);
            this.selectedTowerTotalDamageLabel.TabIndex = 21;
            this.selectedTowerTotalDamageLabel.Text = "Total damage:";
            this.selectedTowerTotalDamageLabel.Visible = false;
            // 
            // selectedTowerTotalDamage
            // 
            this.selectedTowerTotalDamage.AutoSize = true;
            this.selectedTowerTotalDamage.Location = new System.Drawing.Point(696, 289);
            this.selectedTowerTotalDamage.Name = "selectedTowerTotalDamage";
            this.selectedTowerTotalDamage.Size = new System.Drawing.Size(13, 13);
            this.selectedTowerTotalDamage.TabIndex = 22;
            this.selectedTowerTotalDamage.Text = "0";
            this.selectedTowerTotalDamage.Visible = false;
            // 
            // DogHouseTowerPB
            // 
            this.DogHouseTowerPB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.DogHouseTowerPB.Location = new System.Drawing.Point(699, 125);
            this.DogHouseTowerPB.Name = "DogHouseTowerPB";
            this.DogHouseTowerPB.Size = new System.Drawing.Size(25, 25);
            this.DogHouseTowerPB.TabIndex = 23;
            this.DogHouseTowerPB.TabStop = false;
            this.DogHouseTowerPB.Click += new System.EventHandler(this.DogHouseTowerPB_Click);
            // 
            // nextWaveBtn
            // 
            this.nextWaveBtn.Location = new System.Drawing.Point(663, 527);
            this.nextWaveBtn.Name = "nextWaveBtn";
            this.nextWaveBtn.Size = new System.Drawing.Size(109, 34);
            this.nextWaveBtn.TabIndex = 24;
            this.nextWaveBtn.Text = "Next wave!";
            this.nextWaveBtn.UseVisualStyleBackColor = true;
            this.nextWaveBtn.Click += new System.EventHandler(this.nextWaveBtn_Click);
            // 
            // FuzzyTowerPB
            // 
            this.FuzzyTowerPB.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.FuzzyTowerPB.Location = new System.Drawing.Point(730, 12);
            this.FuzzyTowerPB.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FuzzyTowerPB.Name = "FuzzyTowerPB";
            this.FuzzyTowerPB.Size = new System.Drawing.Size(41, 50);
            this.FuzzyTowerPB.TabIndex = 25;
            this.FuzzyTowerPB.TabStop = false;
            this.FuzzyTowerPB.Click += new System.EventHandler(this.FuzzyTowerPB_Click);
            // 
            // towerDescription
            // 
            this.towerDescription.Location = new System.Drawing.Point(620, 374);
            this.towerDescription.Name = "towerDescription";
            this.towerDescription.ReadOnly = true;
            this.towerDescription.Size = new System.Drawing.Size(151, 138);
            this.towerDescription.TabIndex = 26;
            this.towerDescription.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 709);
            this.Controls.Add(this.towerDescription);
            this.Controls.Add(this.FuzzyTowerPB);
            this.Controls.Add(this.nextWaveBtn);
            this.Controls.Add(this.DogHouseTowerPB);
            this.Controls.Add(this.selectedTowerTotalDamage);
            this.Controls.Add(this.selectedTowerTotalDamageLabel);
            this.Controls.Add(this.selectedTowerShotsFired);
            this.Controls.Add(this.selectedTowerShotsFiredLabel);
            this.Controls.Add(this.Tower3PB);
            this.Controls.Add(this.selectedTowerKills);
            this.Controls.Add(this.selectedTowerAS);
            this.Controls.Add(this.selectedTowerDamage);
            this.Controls.Add(this.selectedTowerName);
            this.Controls.Add(this.selectedTowerASLabel);
            this.Controls.Add(this.selectedTowerKillsLabel);
            this.Controls.Add(this.selectedTowerAtkDmgLabel);
            this.Controls.Add(this.selectedTowerNameLabel);
            this.Controls.Add(this.deleteTowerBtn);
            this.Controls.Add(this.handSelectPB);
            this.Controls.Add(this.playerLivesAmount);
            this.Controls.Add(this.playerLivesLabel);
            this.Controls.Add(this.playerGoldAmount);
            this.Controls.Add(this.playerGoldLabel);
            this.Controls.Add(this.showVerticesBtn);
            this.Controls.Add(this.Tower2PB);
            this.Controls.Add(this.Tower1PB);
            this.Controls.Add(this.GameWorldPB);
            this.DoubleBuffered = true;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Tower2PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tower1PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GameWorldPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.handSelectPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tower3PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DogHouseTowerPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FuzzyTowerPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox GameWorldPB;
        private System.Windows.Forms.PictureBox Tower1PB;
        private System.Windows.Forms.PictureBox Tower2PB;
        private System.Windows.Forms.Button showVerticesBtn;
        private System.Windows.Forms.Timer globalTimer;
        private System.Windows.Forms.Label playerGoldLabel;
        public System.Windows.Forms.Label playerGoldAmount;
        private System.Windows.Forms.Label playerLivesLabel;
        private System.Windows.Forms.Label playerLivesAmount;
        private System.Windows.Forms.PictureBox handSelectPB;
        private System.Windows.Forms.Button deleteTowerBtn;
        private System.Windows.Forms.Label selectedTowerNameLabel;
        private System.Windows.Forms.Label selectedTowerAtkDmgLabel;
        private System.Windows.Forms.Label selectedTowerKillsLabel;
        private System.Windows.Forms.Label selectedTowerASLabel;
        private System.Windows.Forms.Label selectedTowerName;
        private System.Windows.Forms.Label selectedTowerDamage;
        private System.Windows.Forms.Label selectedTowerAS;
        private System.Windows.Forms.Label selectedTowerKills;
        private System.Windows.Forms.PictureBox Tower3PB;
        private System.Windows.Forms.Label selectedTowerShotsFiredLabel;
        private System.Windows.Forms.Label selectedTowerShotsFired;
        private System.Windows.Forms.Label selectedTowerTotalDamageLabel;
        private System.Windows.Forms.Label selectedTowerTotalDamage;
        private System.Windows.Forms.PictureBox DogHouseTowerPB;
        private System.Windows.Forms.Button nextWaveBtn;
        private System.Windows.Forms.PictureBox FuzzyTowerPB;
        private System.Windows.Forms.RichTextBox towerDescription;
    }
}


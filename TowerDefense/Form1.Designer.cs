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
            ((System.ComponentModel.ISupportInitialize)(this.Tower2PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tower1PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GameWorldPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.handSelectPB)).BeginInit();
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
            this.globalTimer.Interval = 250;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 709);
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
    }
}


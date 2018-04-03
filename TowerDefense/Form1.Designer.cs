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
            this.GameWorldPB = new System.Windows.Forms.PictureBox();
            this.Tower1PB = new System.Windows.Forms.PictureBox();
            this.Tower2PB = new System.Windows.Forms.PictureBox();
            this.showVerticesBtn = new System.Windows.Forms.Button();
            this.globalTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GameWorldPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tower1PB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tower2PB)).BeginInit();
            this.SuspendLayout();
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
            // Tower1PB
            // 
            this.Tower1PB.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Tower1PB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Tower1PB.Image = ((System.Drawing.Image)(resources.GetObject("Tower1PB.Image")));
            this.Tower1PB.InitialImage = ((System.Drawing.Image)(resources.GetObject("Tower1PB.InitialImage")));
            this.Tower1PB.Location = new System.Drawing.Point(722, 12);
            this.Tower1PB.Name = "Tower1PB";
            this.Tower1PB.Size = new System.Drawing.Size(50, 50);
            this.Tower1PB.TabIndex = 1;
            this.Tower1PB.TabStop = false;
            this.Tower1PB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tower1PB_MouseDown);
            // 
            // Tower2PB
            // 
            this.Tower2PB.BackColor = System.Drawing.SystemColors.InfoText;
            this.Tower2PB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Tower2PB.Location = new System.Drawing.Point(722, 68);
            this.Tower2PB.Name = "Tower2PB";
            this.Tower2PB.Size = new System.Drawing.Size(50, 50);
            this.Tower2PB.TabIndex = 2;
            this.Tower2PB.TabStop = false;
            this.Tower2PB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Tower2PB_MouseDown);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 761);
            this.Controls.Add(this.showVerticesBtn);
            this.Controls.Add(this.Tower2PB);
            this.Controls.Add(this.Tower1PB);
            this.Controls.Add(this.GameWorldPB);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.GameWorldPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tower1PB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Tower2PB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox GameWorldPB;
        private System.Windows.Forms.PictureBox Tower1PB;
        private System.Windows.Forms.PictureBox Tower2PB;
        private System.Windows.Forms.Button showVerticesBtn;
        private System.Windows.Forms.Timer globalTimer;
    }
}


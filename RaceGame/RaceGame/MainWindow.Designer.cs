namespace RaceGame
{
    partial class MainWindow
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
            this.Meter1 = new System.Windows.Forms.Label();
            this.Meter2 = new System.Windows.Forms.Label();
            this.canvas = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // Meter1
            // 
            this.Meter1.AutoSize = true;
            this.Meter1.BackColor = System.Drawing.Color.Transparent;
            this.Meter1.Location = new System.Drawing.Point(12, 9);
            this.Meter1.Name = "Meter1";
            this.Meter1.Size = new System.Drawing.Size(51, 65);
            this.Meter1.TabIndex = 3;
            this.Meter1.Text = "Player 1\r\nSpeed: 0\r\nFuel: 100\r\nLaps: 0\r\nPits: 0";
            // 
            // Meter2
            // 
            this.Meter2.AutoSize = true;
            this.Meter2.BackColor = System.Drawing.Color.Transparent;
            this.Meter2.Location = new System.Drawing.Point(936, 9);
            this.Meter2.Name = "Meter2";
            this.Meter2.Size = new System.Drawing.Size(51, 65);
            this.Meter2.TabIndex = 4;
            this.Meter2.Text = "Player 2\r\n0 :Speed\r\n100 :Fuel\r\n0 :Laps\r\n0 :Pits";
            this.Meter2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // canvas
            // 
            this.canvas.BackColor = System.Drawing.Color.ForestGreen;
            this.canvas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.canvas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.canvas.Cursor = System.Windows.Forms.Cursors.Default;
            this.canvas.Location = new System.Drawing.Point(-2, -1);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(1013, 729);
            this.canvas.TabIndex = 2;
            this.canvas.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.Meter2);
            this.Controls.Add(this.Meter1);
            this.Controls.Add(this.canvas);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.MinimumSize = new System.Drawing.Size(1024, 726);
            this.Name = "MainWindow";
            this.Text = "Race Game";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Exit);
            ((System.ComponentModel.ISupportInitialize)(this.canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.PictureBox canvas;
        private System.Windows.Forms.Label Meter1;
        private System.Windows.Forms.Label Meter2;
    }
}


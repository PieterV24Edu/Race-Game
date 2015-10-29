namespace RaceGame
{
    partial class StartMenu
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
            this.StartButton = new System.Windows.Forms.Button();
            this.Player1Textbox = new System.Windows.Forms.TextBox();
            this.Player2Textbox = new System.Windows.Forms.TextBox();
            this.RoundTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // StartButton
            // 
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartButton.Location = new System.Drawing.Point(417, 480);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(190, 118);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            // 
            // Player1Textbox
            // 
            this.Player1Textbox.Location = new System.Drawing.Point(90, 290);
            this.Player1Textbox.Name = "Player1Textbox";
            this.Player1Textbox.Size = new System.Drawing.Size(278, 20);
            this.Player1Textbox.TabIndex = 2;
            this.Player1Textbox.Text = "Player 1";
            this.Player1Textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Player2Textbox
            // 
            this.Player2Textbox.Location = new System.Drawing.Point(656, 290);
            this.Player2Textbox.Name = "Player2Textbox";
            this.Player2Textbox.Size = new System.Drawing.Size(278, 20);
            this.Player2Textbox.TabIndex = 3;
            this.Player2Textbox.Text = "Player 2";
            this.Player2Textbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RoundTextbox
            // 
            this.RoundTextbox.Location = new System.Drawing.Point(453, 290);
            this.RoundTextbox.Name = "RoundTextbox";
            this.RoundTextbox.Size = new System.Drawing.Size(118, 20);
            this.RoundTextbox.TabIndex = 4;
            this.RoundTextbox.Text = "Rounds(default: 3)";
            this.RoundTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // StartMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::RaceGame.Properties.Resources.MenuBackground;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.RoundTextbox);
            this.Controls.Add(this.Player2Textbox);
            this.Controls.Add(this.Player1Textbox);
            this.Controls.Add(this.StartButton);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1024, 768);
            this.Name = "StartMenu";
            this.Text = "StartMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.TextBox Player1Textbox;
        private System.Windows.Forms.TextBox Player2Textbox;
        private System.Windows.Forms.TextBox RoundTextbox;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceGame
{
    public partial class StartMenu : Form
    {
        public StartMenu()
        {
            InitializeComponent();
            StartButton.Click += new EventHandler(StartButton_Click);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (Player1Textbox.Text == "")
            {
                Player1Textbox.Text = "Player 1";
            }
            if (Player2Textbox.Text == "")
            {
                Player2Textbox.Text = "Player 2";
            }
            if (RoundTextbox.Text == "" || RoundTextbox.Text == "Rounds(default: 3)")
            {
                RoundTextbox.Text = "3";
            }
            this.Hide();
            MainWindow MW = new MainWindow(Player1Textbox.Text, Player2Textbox.Text, int.Parse(RoundTextbox.Text));
            MW.Closed += (s, args) => this.Close();
            MW.Show();
        }
        
        
    }
}

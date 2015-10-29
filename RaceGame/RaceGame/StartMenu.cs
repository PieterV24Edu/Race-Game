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
        //pass names and rounds to the mainwindow
        //placeholders will be added if no input is given
        private void StartButton_Click(object sender, EventArgs e)
        {
            int rounds;
            if (Player1Textbox.Text == "")
            {
                Player1Textbox.Text = "Player 1";
            }
            if (Player2Textbox.Text == "")
            {
                Player2Textbox.Text = "Player 2";
            }
            if (int.TryParse(RoundTextbox.Text, out rounds) == false)
            {
                rounds = 3;
            }
            //close this form and open the game itself
            this.Hide();
            MainWindow MW = new MainWindow(Player1Textbox.Text, Player2Textbox.Text, rounds);
            MW.Closed += (s, args) => this.Close();
            MW.Show();
        }
        
        
    }
}

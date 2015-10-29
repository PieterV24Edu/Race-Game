using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RaceGame
{
    //Form to display once the game is won
    public partial class FinishWindow : Form
    {
        private string Winner;
        public FinishWindow(string winner)
        {
            InitializeComponent();
            //write the winner to the middle of the screen
            Winner = winner;
            label1.Text = "The winner is:" + Winner;
            label1.Location = new Point((1024 - label1.Width) / 2, (768 - label1.Height) / 2);
            //get user input about starting over or exiting the game
            OverButton.Click += new EventHandler(OverButton_Click);
            ExitButton.Click += new EventHandler(ExitButton_Click);
        }

        private void OverButton_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Race_Game2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Bitmap Backbuffer;
            Point Blockpoint = new Point(50, 50);
            Point BlockSpeed = new Point(0, 0);
            int DefaultBlockspeed = 10;
            float angle;
        }
    }
}

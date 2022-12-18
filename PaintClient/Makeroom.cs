using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace drawingtest
{
    public partial class Makeroom : Form
    {
        
        public string roomname;
        public string roompw;
        
        public Makeroom(TcpClient client)
        {
            InitializeComponent();
            roompw_textBox.Enabled = false;
           
        }

        

        private void button2_Click(object sender, EventArgs e)
        {
            roompw_textBox.Enabled=true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (roomname != "")
            {
                roomname = roomname_textBox.Text;
                roompw = roompw_textBox.Text;
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}

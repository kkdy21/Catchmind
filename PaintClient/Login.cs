using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
namespace drawingtest
{
    public partial class Login : Form
    {
        string bindIp = "127.0.0.1";
        const int bindPort = 81;
        const int subbindPort = 82;

        TcpClient client;
        TcpClient subclient;

        IPEndPoint localAddress;
        IPEndPoint sublocalAddress;

        NetworkStream stream;
        NetworkStream substream;

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                Openclient();

                string flag = "Login";
                string ID = textBox1.Text;
                byte[] packet = new byte[256];

                packet = Encoding.UTF8.GetBytes(flag);
                stream.Write(packet, 0, packet.Length);
                Thread.Sleep(500);
                packet = Encoding.UTF8.GetBytes(ID);
                stream.Write(packet, 0, packet.Length);

                Array.Clear(packet,0,packet.Length);
                int len = stream.Read(packet, 0, packet.Length);
                string loginresult = Encoding.UTF8.GetString(packet, 0, len);

                if (loginresult == "0")
                {
                    subOpenclient();
                    MessageBox.Show("로그인 성공");
                    Rooms rooms = new Rooms(client,subclient);
                    this.Hide();
                    rooms.ShowDialog();
                    this.Show();
                    stream.Close();
                    client.Close();
                    substream.Close();
                    subclient.Close();
                }
                else
                {
                    MessageBox.Show("중복된 닉네임 입니다.");
                    stream.Close();
                    client.Close();
                }
            }
        }


        public void Openclient()
        {
            client = new TcpClient();
            localAddress = new IPEndPoint(IPAddress.Parse(bindIp), bindPort);
            client.Connect(localAddress);
            stream = client.GetStream();



        }

        public void subOpenclient()
        {
            subclient = new TcpClient();
            sublocalAddress = new IPEndPoint(IPAddress.Parse(bindIp), subbindPort);
            subclient.Connect(sublocalAddress);
            substream = client.GetStream();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }
    }
}

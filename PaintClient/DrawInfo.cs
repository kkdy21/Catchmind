using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Timers;
namespace drawingtest
{
    public partial class Paint_practice
    {
        drawInfo drawinfosend = new drawInfo();

        private void DrawRecv(drawInfo drawinforead)
        {
            try
            {

                for (int info = 0; info < drawinforead.curTool.Count; info++)
                {
                    switch (drawinforead.curTool[info])
                    {
                        case (int)Art_tools.PEN:
                            using (Graphics graphics = Graphics.FromImage(drawimage))

                            {
                                Pen pen = new Pen(drawinforead.curColor[info], drawinforead.width[info]);
                                graphics.DrawLine(pen, drawinforead.mouse_pos[info].X, drawinforead.mouse_pos[info].Y, drawinforead.last_mouse_pos[info].X, drawinforead.last_mouse_pos[info].Y);
                                pictureBox1.Image = drawimage;
                            }
                            break;
                        case (int)Art_tools.ERASER:
                            using (Graphics graphics = Graphics.FromImage(drawimage))
                            {
                                Pen pen = new Pen(drawinforead.curColor[info], drawinforead.width[info] * 7);
                                graphics.DrawLine(pen, drawinforead.mouse_pos[info].X, drawinforead.mouse_pos[info].Y, drawinforead.last_mouse_pos[info].X, drawinforead.last_mouse_pos[info].Y);
                                pictureBox1.Image = drawimage;

                            }
                            break;
                        case (int)Art_tools.FIGURE:
                            using (Graphics graphics = Graphics.FromImage(drawimage))
                            {
                                Pen pen = new Pen(drawinforead.curColor[info], drawinforead.width[info]);
                                graphics.DrawRectangle(pen, drawinforead.mouse_pos[info].X, drawinforead.mouse_pos[info].Y, drawinforead.last_mouse_pos[info].X, drawinforead.last_mouse_pos[info].Y);
                                pictureBox1.Image = drawimage;

                            }
                            break;
                        case (int)Art_tools.CIRCLE:
                            //using (Graphics graphics = pictureBox1.CreateGraphics())
                            using (Graphics graphics = Graphics.FromImage(drawimage))
                            {
                                Pen pen = new Pen(drawinforead.curColor[info], drawinforead.width[info]);
                                graphics.DrawEllipse(pen, drawinforead.mouse_pos[info].X, drawinforead.mouse_pos[info].Y, drawinforead.last_mouse_pos[info].X, drawinforead.last_mouse_pos[info].Y);
                                pictureBox1.Image = drawimage;

                            }
                            break;
                        case (int)Art_tools.PAINT:
                            pictureBox1.Image = null;
                            drawimage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                            break;
                    }
                }
            }
            catch
            {

            }
        }

        private void DrawSave(Color color, int width, Point e, Point mouse_pos, int curTool)
        {
            drawinfosend.curColor.Add(color);
            drawinfosend.width.Add(width);
            drawinfosend.last_mouse_pos.Add(mouse_pos);
            drawinfosend.mouse_pos.Add(e);
            drawinfosend.curTool.Add(curTool);
        }

        private void DrawSend(object sender, ElapsedEventArgs e)
        {
            if (myturn == true)
            {
                byte[] packet = new byte[50000];

                using (MemoryStream Mstream = new MemoryStream())
                {
                    formatter.Serialize(Mstream, drawinfosend);
                    drawinfosend = new drawInfo();
                    packet = Mstream.ToArray();
                    substream.Write(packet, 0, packet.Length);

                }
            }
        }

        private void subread_func()
        {
            try
            {
                while (true)
                {
                    byte[] packet = new byte[50000];
                    int len = substream.Read(packet, 0, packet.Length);


                    if (len < 1500)
                    {
                        string readResult = Encoding.UTF8.GetString(packet, 0, len);

                        if (readResult == "EndTask")
                            break;
                    }
                    else
                    {
                        Task t3 = new Task(() => ByteTodrawInfo(packet));
                        t3.Start();
                    }
                }
            }
            catch
            {

            }
            
        }

        public void read_func()
        {
            while (true)
            {
                byte[] packet = new byte[50000];
                int len = stream.Read(packet, 0, packet.Length);
                string readResult = Encoding.UTF8.GetString(packet, 0, len);

                if (readResult == "EndTask")
                    break;
                else if (readResult == "DrawRoomMsg")
                {
                    byte[] packet_ = new byte[1024];
                    stream.Read(packet_, 0, packet_.Length);
                    string encode_packet_ = Encoding.UTF8.GetString(packet_);

                    roomtalk_richtext.Invoke(new MethodInvoker(delegate ()
                    {
                        roomtalk_richtext.AppendText(encode_packet_);
                        roomtalk_richtext.AppendText("\n");
                        roomtalk_richtext.Select(roomtalk_richtext.Text.Length, 0);
                        roomtalk_richtext.ScrollToCaret();
                    }));
                }
                else if (readResult == "AnswerKey")
                {
                    byte[] packet_ = new byte[128];
                    int len_ = stream.Read(packet_, 0, packet_.Length);
                    AnswerKey = Encoding.UTF8.GetString(packet_, 0, len_);

                    answerkey_textbox.Invoke(new MethodInvoker(delegate ()
                   {
                       answerkey_textbox.Text = AnswerKey;
                       if (myturn)
                           answerkey_textbox.Visible = true;
                       else
                           answerkey_textbox.Visible = false;

                   }));
                }
                else if (readResult == "YourTurn")
                {
                    myturn = true;

                    answerkey_textbox.Invoke(new MethodInvoker(delegate ()
                    {
                        answerkey_textbox.Visible = true;
                    }));

                }
                else if (readResult == "NotYourTurn")
                {
                    myturn = false;

                    answerkey_textbox.Invoke(new MethodInvoker(delegate ()
                    {
                        answerkey_textbox.Visible = false;
                    }));

                    pictureBox1.Image = null;

                    drawimage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                }
                else if (readResult == "GameStart")
                {
                    byte[] packet_ = new byte[1024];
                    stream.Read(packet_, 0, packet_.Length);
                    string encode_packet_ = Encoding.UTF8.GetString(packet_);

                    richtextbox_input(encode_packet_, Color.Red);
                }
                else if (readResult == "Notify")
                {
                    byte[] packet_ = new byte[1024];
                    stream.Read(packet_, 0, packet_.Length);
                    string encode_packet_ = Encoding.UTF8.GetString(packet_);

                    richtextbox_input(encode_packet_, Color.Blue);

                }
                else if (readResult == "EnterMsg")
                {
                    byte[] packet_ = new byte[1024];
                    stream.Read(packet_, 0, packet_.Length);
                    string encode_packet_ = Encoding.UTF8.GetString(packet_);

                    richtextbox_input(encode_packet_, Color.Green);

                }

                else if (readResult == "OutMsg")
                {
                    byte[] packet_ = new byte[1024];
                    stream.Read(packet_, 0, packet_.Length);
                    string encode_packet_ = Encoding.UTF8.GetString(packet_);

                    richtextbox_input(encode_packet_, Color.Orange);
                }
                else if (readResult == "Win")
                {
                    byte[] packet_ = new byte[1024];
                    stream.Read(packet_, 0, packet_.Length);
                    string encode_packet_ = Encoding.UTF8.GetString(packet_);

                    Task.Run(() =>
                    {MessageBox.Show(encode_packet_);});


                    //richtextbox_input(encode_packet_, Color.Red);

                    myturn = false;
                    gamestart_btn.Invoke(new MethodInvoker(delegate ()
                    {
                        if (host)
                            gamestart_btn.Enabled = true;
                    }));
                    
                    answerkey_textbox.Invoke(new MethodInvoker(delegate ()
                    {
                        answerkey_textbox.Visible = false;
                    }));
                }
                else if (readResult== "CurentScore")
                {

                    byte[] packet_ = new byte[1024];
                    stream.Read(packet_, 0, packet_.Length);
                    string encode_packet_ = Encoding.UTF8.GetString(packet_);
                    string[] SplitData = encode_packet_.Split("_");

                    Insert_data(SplitData,SplitData.Length);
                }
            }
        }

        private void richtextbox_input(string input , Color c)
        {
            roomtalk_richtext.Invoke(new MethodInvoker(delegate ()
            {
                roomtalk_richtext.SelectionColor = c;
                roomtalk_richtext.AppendText(input);
                roomtalk_richtext.AppendText("\n");
                roomtalk_richtext.Select(roomtalk_richtext.Text.Length, 0);
                roomtalk_richtext.ScrollToCaret();
            }));
        }

        private void Insert_data(string[] insertdata, int j)    //잘라진데이터, string.length
        {
            listView1.Invoke(new MethodInvoker(delegate ()
            {
                listView1.Clear();

                listView1.View = View.Details;
                listView1.FullRowSelect = true;
                listView1.Columns.Add("ID", 125);
                listView1.Columns.Add("Score", 125);

                listView1.BeginUpdate();


                for (int i = 0; i < j - 1; i += 2)
                {
                    if (insertdata[i] != "")
                    {
                        ListViewItem LVI = new ListViewItem(insertdata[i]);
                        LVI.SubItems.Add(insertdata[i + 1]);
                        LVI.ImageIndex = 0;
                        listView1.Items.Add(LVI);
                    }
                }

                listView1.EndUpdate();
            }));
            
        }

        private void ByteTodrawInfo(byte[] packet)
        {
            try
            {
                drawInfo drawinfoRead;

                using (MemoryStream Mstream = new MemoryStream(packet))
                {
                    drawinfoRead = (drawInfo)formatter2.Deserialize(Mstream);
                    DrawRecv(drawinfoRead);
                }
            }
            catch
            {

            }
        }

        public void close_event(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("종료하시겠습니까?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                timer.Stop();
                e.Cancel = false;
                byte[] packet = Encoding.UTF8.GetBytes("ENDDRAW");
                stream.Write(packet, 0, packet.Length);
                substream.Write(packet, 0, packet.Length);

            }
            else
                e.Cancel = true;

        }
    }


    [Serializable]
    public class drawInfo
    {
        public List<Point> mouse_pos = new List<Point>();
        public List<Point> last_mouse_pos = new List<Point>();
        public List<Color> curColor = new List<Color>();
        public List<int> curTool = new List<int>();
        public List<int> width = new List<int>();

    }

}

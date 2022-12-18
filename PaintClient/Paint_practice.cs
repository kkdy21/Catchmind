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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using System.Timers;

namespace drawingtest
{


    public partial class Paint_practice : Form
    {
        Point mouse_pos = new Point(0, 0);
        Color curColor = Color.Black;

        drawInfo drawinfo = new drawInfo();
        SolidBrush brushBlue;

        BinaryFormatter formatter;
        BinaryFormatter formatter2;

        Bitmap drawimage;//, drawimage2;

        TcpClient mysock = new TcpClient();
        TcpClient submysock = new TcpClient();
        NetworkStream stream, substream;
        int curTool = (int)Art_tools.DEFAULT;
        int curWidth = 0;

        System.Timers.Timer timer = new System.Timers.Timer();

        bool myturn = false;
        bool host = false;

        string AnswerKey;

        public Paint_practice(TcpClient client, TcpClient subclient, bool host)
        {
            InitializeComponent();
            mysock = client;
            submysock = subclient;
            this.host = host;

            stream = mysock.GetStream();
            substream = submysock.GetStream();
            Controls_init();

            formatter = new BinaryFormatter();
            formatter2 = new BinaryFormatter();
            curWidth = Convert.ToInt32(comboBox1.SelectedItem.ToString());

            drawimage = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            //drawimage2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);


            Task t = new Task(() => read_func());
            t.Start();

            Task t2 = new Task(() => subread_func());
            t2.Start();

            timer.Interval = 100; // 1초
            timer.Elapsed += new ElapsedEventHandler(DrawSend);
            timer.Start();

        }

        private void Controls_init()
        {

            Allclearbtn.Click += Allclearbtn_Click;

            palette.Click += palette_select;

            curColor_picture.BackColor = curColor;

            comboBox1.SelectedIndex = 0;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDown;

            roomtalk_richtext.Multiline = true;
            roomtalk_richtext.ReadOnly = true;
            roomtalk_richtext.BackColor = Color.White;

            msgsend_btn.Click += MsgSend;

            draw_textbox.KeyDown += draw_textbox_down;


            if (host)
            {
                gamestart_btn.Enabled = true;
                gamestart_btn.Visible = true;
                answerkey_textbox.Visible = true;
            }
            else
            {
                gamestart_btn.Enabled = false;
                gamestart_btn.Visible = false;
                answerkey_textbox.Visible = false;
            }


            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.Columns.Add("ID", 125);
            listView1.Columns.Add("Score", 125);




            this.Shown += EnterMsg;
            this.FormClosing += close_event;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ((int)Art_tools.PEN == curTool || (int)Art_tools.ERASER == curTool) && myturn)
            {
                brushBlue = new SolidBrush(curColor);
                //using (Graphics graphics = pictureBox1.CreateGraphics())
                using (Graphics graphics = Graphics.FromImage(drawimage))
                {
                    Pen pen;
                    if (curTool == (int)Art_tools.PEN)
                    {
                        curColor = curColor_picture.BackColor;
                        pen = new Pen(curColor, curWidth);
                    }
                    else
                    {
                        curColor = pictureBox1.BackColor;
                        pen = new Pen(curColor, curWidth * 7);

                    }

                    graphics.DrawLine(pen, e.X, e.Y, mouse_pos.X, mouse_pos.Y); //현재픽쳐박스에서 누른 좌표와 픽쳐박스에서 눌렀던 좌표
                                                                                //save_points.Add(e, mouse_pos);
                    pictureBox1.Image = drawimage;
                    DrawSave(curColor, curWidth, e.Location, mouse_pos, curTool);

                    mouse_pos = pictureBox1.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y)); //pictureBox1상의 좌표로 변환한 위치
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            curWidth = Convert.ToInt32(comboBox1.SelectedItem.ToString());
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_pos = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && curTool == (int)Art_tools.FIGURE && myturn)
            {
                using (Graphics graphics = Graphics.FromImage(drawimage))
                {
                    Point last_mouse_pos = e.Location;
                    Point curPos, lastPos;
                    curColor = curColor_picture.BackColor;
                    Pen pen = new Pen(curColor, curWidth);

                    //control 의 x,y 컴마기준 아래 오른쪽으로 뻗어나감
                    if (mouse_pos.X - last_mouse_pos.X > 0 && mouse_pos.Y - last_mouse_pos.Y > 0)
                    {
                        graphics.DrawRectangle(pen, last_mouse_pos.X, last_mouse_pos.Y, mouse_pos.X - last_mouse_pos.X, mouse_pos.Y - last_mouse_pos.Y);
                        curPos = last_mouse_pos;
                        lastPos = new Point(mouse_pos.X - last_mouse_pos.X, mouse_pos.Y - last_mouse_pos.Y);

                        DrawSave(curColor, curWidth, curPos, lastPos, curTool);

                    }
                    else if (mouse_pos.X - last_mouse_pos.X > 0 && last_mouse_pos.Y - mouse_pos.Y > 0)
                    {
                        graphics.DrawRectangle(pen, last_mouse_pos.X, mouse_pos.Y, mouse_pos.X - last_mouse_pos.X, last_mouse_pos.Y - mouse_pos.Y);
                        curPos = new Point(last_mouse_pos.X, mouse_pos.Y);
                        lastPos = new Point(mouse_pos.X - last_mouse_pos.X, last_mouse_pos.Y - mouse_pos.Y);

                        DrawSave(curColor, curWidth, curPos, lastPos, curTool);

                    }

                    else if (last_mouse_pos.X - mouse_pos.X > 0 && mouse_pos.Y - last_mouse_pos.Y > 0)
                    {
                        graphics.DrawRectangle(pen, mouse_pos.X, last_mouse_pos.Y, last_mouse_pos.X - mouse_pos.X, mouse_pos.Y - last_mouse_pos.Y);
                        curPos = new Point(mouse_pos.X, last_mouse_pos.Y);
                        lastPos = new Point(last_mouse_pos.X - mouse_pos.X, mouse_pos.Y - last_mouse_pos.Y);

                        DrawSave(curColor, curWidth, curPos, lastPos, curTool);

                    }

                    else if (last_mouse_pos.X - mouse_pos.X > 0 && last_mouse_pos.Y - mouse_pos.Y > 0)
                    {
                        graphics.DrawRectangle(pen, mouse_pos.X, mouse_pos.Y, last_mouse_pos.X - mouse_pos.X, last_mouse_pos.Y - mouse_pos.Y);
                        curPos = mouse_pos;
                        lastPos = new Point(last_mouse_pos.X - mouse_pos.X, last_mouse_pos.Y - mouse_pos.Y);

                        DrawSave(curColor, curWidth, curPos, lastPos, curTool);

                    }
                    pictureBox1.Image = drawimage;
                }
            }

            if (e.Button == MouseButtons.Left && curTool == (int)Art_tools.CIRCLE && myturn)
            {
                using (Graphics graphics = Graphics.FromImage(drawimage))
                {
                    Point last_mouse_pos = e.Location;
                    Point curPos, lastPos;

                    curColor = curColor_picture.BackColor;
                    Pen pen = new Pen(curColor, curWidth);
                    // graphics.DrawEllipse(pen, last_mouse_pos.X, last_mouse_pos.Y, mouse_pos.X - last_mouse_pos.X, mouse_pos.Y - last_mouse_pos.Y);
                    graphics.DrawEllipse(pen, mouse_pos.X, mouse_pos.Y, last_mouse_pos.X - mouse_pos.X, last_mouse_pos.Y - mouse_pos.Y);

                    curPos = mouse_pos;
                    lastPos = new Point(last_mouse_pos.X - mouse_pos.X, last_mouse_pos.Y - mouse_pos.Y);

                    pictureBox1.Image = drawimage;

                    DrawSave(curColor, curWidth, curPos, lastPos, curTool);

                }
            }
        }


        private void draw_textbox_down(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                MsgSend(sender, e);
        }



        private void MsgSend(object sender, EventArgs e)
        {
            if (draw_textbox.Text != "")
            {
                byte[] input = Encoding.UTF8.GetBytes(draw_textbox.Text);
                byte[] flag = Encoding.UTF8.GetBytes("DrawRoomMsg");

                stream.Write(flag, 0, flag.Length);
                stream.Write(input, 0, input.Length);
                draw_textbox.Text = "";
            }
        }


        private void EnterMsg(object sender, EventArgs e)
        {
            byte[] flag = Encoding.UTF8.GetBytes("EnterMsg");
            stream.Write(flag, 0, flag.Length);
        }

        private void gamestart_btn_Click(object sender, EventArgs e)
        {
            byte[] flag = Encoding.UTF8.GetBytes("GameStart");
            stream.Write(flag, 0, flag.Length);

            gamestart_btn.Enabled = false;

        }

        private void score_btn_Click(object sender, EventArgs e)
        {

            byte[] flag = Encoding.UTF8.GetBytes("CurentScore");
            stream.Write(flag, 0, flag.Length);

        }

    }

}

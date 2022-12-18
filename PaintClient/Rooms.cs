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
using System.Threading;

namespace drawingtest
{
    public partial class Rooms : Form
    {
        TcpClient client,subclient;
        NetworkStream stream;
        List<string[]> RoomsList = new List<string[]>();
        Button[] room = new Button[6];

        public Rooms(TcpClient client,TcpClient subclient)
        {
            InitializeComponent();

            this.client = client;
            this.subclient = subclient;
            stream = this.client.GetStream();

            Task task = new Task(() => waitroomrecv());
            task.Start();

            Controls_init();
            this.Shown += refresh_btn_Click;
        }

        private void Controls_init()
        {

            for (int i = 0; i < 6; i++)
            {
                room[i] = new Button();
                room[i].Text = "Empty";
                room[i].BackColor = Color.LightGray;
                room[i].Enabled = false;
                room[i].Size = new Size(309, 85);



                if (i == 0)
                    room[i].Location = new Point(199, 25);
                else if (i == 1)
                    room[i].Location = new Point(524, 25);
                else if (i == 2)
                    room[i].Location = new Point(199, 136);
                else if (i == 3)
                    room[i].Location = new Point(524, 136);
                else if (i == 4)
                    room[i].Location = new Point(199, 249);
                else if (i == 5)
                    room[i].Location = new Point(524, 249);

                room[i].Click += room_btn_Click;
                Controls.Add(room[i]);


            }

            waitroomtalk_richtext.Multiline = true;
            waitroomtalk_richtext.ReadOnly = true;
            waitroomtalk_richtext.BackColor = Color.White;

            this.FormClosing += close_event;


        }

        private void waitroomrecv()
        {
            while (true)
            {
                byte[] packet = new byte[1024];
                int len = stream.Read(packet, 0, packet.Length);
                string encode_packet = Encoding.UTF8.GetString(packet, 0, len);

                if (encode_packet == "WaitRoomMsg")
                {
                    byte[] packet_ = new byte[1024];
                    stream.Read(packet_, 0, packet_.Length);
                    string encode_packet_ = Encoding.UTF8.GetString(packet_);

                    waitroomtalk_richtext.Invoke(new MethodInvoker(delegate ()
                    {
                        waitroomtalk_richtext.AppendText(encode_packet_);
                        waitroomtalk_richtext.AppendText("\n");
                        waitroomtalk_richtext.Select(waitroomtalk_richtext.Text.Length, 0);
                        waitroomtalk_richtext.ScrollToCaret();
                    }));
                }
                else if (encode_packet == "Refresh")
                {
                    byte[] IDpacket = new byte[1024];
                    len = stream.Read(IDpacket, 0, IDpacket.Length);
                    string IDdatas = Encoding.UTF8.GetString(IDpacket, 0, len);
                    string[] IDdatas_split = IDdatas.Split("_");


                    waitroominfo_listbox.Invoke(new MethodInvoker(delegate ()
                    {
                        waitroominfo_listbox.Items.Clear();
                        waitroominfo_listbox.Items.Add("<접속중인 유저목록>");

                        for (int i = 0; i < IDdatas_split.Length; i++)
                        {
                            if (IDdatas_split[i] != "")
                                waitroominfo_listbox.Items.Add(IDdatas_split[i]);

                        }
                    }));

                }
                else if (encode_packet == "SearchRoom")
                {
                    byte[] roomspacket = new byte[1024];
                    int len_ = stream.Read(roomspacket, 0, roomspacket.Length);
                    string roomsinfoBeforsplit = Encoding.UTF8.GetString(roomspacket, 0, len_);
                    string[] roomsinfobeforsplitnum = roomsinfoBeforsplit.Split("^");


                    RoomsList.Clear();
                    for (int i = 0; i < 6; i++)
                    {
                        room[i].Invoke(new MethodInvoker(delegate()
                            {
                                room[i].Text = "Empty";
                                room[i].BackColor = Color.LightGray;
                                room[i].Enabled = false;
                            }));
                        
                    }
                    for (int i = 0; i < roomsinfobeforsplitnum.Length; i++)
                    {
                        if (roomsinfobeforsplitnum[i] != "")
                        {
                            string[] roomsinfo = roomsinfobeforsplitnum[i].Split("_");
                            RoomsList.Add(roomsinfo);
                        }
                    }

                    for (int i = 0; i < RoomsList.Count; i++)
                    {
                        if (i < 6)
                        {
                            int roomnum = Convert.ToInt32(RoomsList[i][4]);

                            room[roomnum].Invoke(new MethodInvoker(delegate ()
                            {
                                room[roomnum].Text = $" 방이름 : {RoomsList[i][0]}\n인원수 : {RoomsList[i][2]}/4";
                                room[roomnum].BackColor = Color.AliceBlue;
                                room[roomnum].Enabled = true;
                            }));
                        }
                    }
                }
                else if (encode_packet == "EndTask")
                    break;
            }
        }

        private void makeroom_btn_Click(object sender, EventArgs e)
        {
            SearchRoom();
            Makeroom mr = new Makeroom(client);
            DialogResult result = mr.ShowDialog();
            if (result == DialogResult.OK)
            {
                int i;
                for (i = 0; i <= 6; i++)
                {
                    if (i == 6)
                        break;

                    if (room[i].Text == "Empty")
                    {
                        byte[] packet_ = Encoding.UTF8.GetBytes("Makeroom");
                        stream.Write(packet_, 0, packet_.Length);
                        Thread.Sleep(500);
                        byte[] RoomData = Encoding.UTF8.GetBytes($"{mr.roomname}_{mr.roompw}_{i}");
                        stream.Write(RoomData, 0, RoomData.Length);
                        break;
                    }
                }

                if (i < 6)
                {
                    try
                    {
                        Thread.Sleep(500);
                        bool host = true;
                        Paint_practice Paint = new Paint_practice(client, subclient, host);
                        byte[] _packet = Encoding.UTF8.GetBytes("END");
                        stream.Write(_packet, 0, _packet.Length);
                        Thread.Sleep(500);
                        this.Hide();
                        Paint.ShowDialog();
                        Task task = new Task(() => waitroomrecv());
                        task.Start();
                        this.Show();
                        refresh_btn_Click(sender, e);
                    }
                    catch
                    {

                    }
                    
                }
                else
                    MessageBox.Show("현재 만들수 있는 방이 없습니다.");
            }
        }

        private void room_btn_Click(object sender, EventArgs e)
        {
            SearchRoom();

            Inputpassword IPW = new Inputpassword();
            DialogResult result = IPW.ShowDialog();
            
            if (result == DialogResult.OK)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (room[i] == sender)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (room[i].Text == $" 방이름 : {RoomsList[j][0]}\n인원수 : {RoomsList[j][2]}/4")
                            {
                                if (RoomsList[j][1] == IPW.PW)
                                {
                                    if ((Convert.ToInt32(RoomsList[j][2]) < 4))
                                    {
                                        bool host = false;
                                        Paint_practice Paint = new Paint_practice(client, subclient,host);

                                        byte[] flag = Encoding.UTF8.GetBytes("RoomEnter");
                                        stream.Write(flag, 0, flag.Length);
                                        Thread.Sleep(500);

                                        byte[] makeroomID_roomnum = Encoding.UTF8.GetBytes($"{RoomsList[j][3]}_{RoomsList[j][4]}");
                                        stream.Write(makeroomID_roomnum, 0, makeroomID_roomnum.Length);
                                        Thread.Sleep(500);

                                        byte[] packet = Encoding.UTF8.GetBytes("END");
                                        stream.Write(packet, 0, packet.Length);
                                        Thread.Sleep(500);

                                        this.Hide();
                                        Paint.ShowDialog();
                                        Task task = new Task(() => waitroomrecv());
                                        task.Start();
                                        this.Show();
                                        refresh_btn_Click(sender, e);
                                        break;
                                    }
                                    else
                                    {
                                        MessageBox.Show("입장할수 있는 인원이 초과되었습니다");
                                        break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("비밀번호가 일치하지 않습니다.");
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void refresh_btn_Click(object sender, EventArgs e)
        {
            byte[] flag = Encoding.UTF8.GetBytes("Refresh");
            stream.Write(flag, 0, flag.Length);

            Thread.Sleep(300);
            SearchRoom();
        }

        private void waitroommsgsend_btn_Click(object sender, EventArgs e)
        {
            if (waitroommsg_textbox.Text != "")
            {
                byte[] input = Encoding.UTF8.GetBytes(waitroommsg_textbox.Text);
                byte[] flag = Encoding.UTF8.GetBytes("WaitRoomMsg");

                stream.Write(flag, 0, flag.Length);
                stream.Write(input, 0, input.Length);
                waitroommsg_textbox.Text = "";
            }

        }

        private void SearchRoom()
        {
            byte[] rooms = Encoding.UTF8.GetBytes("Rooms");
            stream.Write(rooms, 0, rooms.Length);
        }

        private void waitroommsg_textbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                waitroommsgsend_btn_Click(sender, e);
        }


        public void close_event(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("종료하시겠습니까?", "My Application", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                e.Cancel = false;
                byte[] packet = Encoding.UTF8.GetBytes("END");
                stream.Write(packet, 0, packet.Length);

                Thread.Sleep(300);
                byte[] packet_ = Encoding.UTF8.GetBytes("Deleteuserinfo");
                stream.Write(packet_, 0, packet_.Length);
            }
            else
                e.Cancel = true;
        }
    }
}

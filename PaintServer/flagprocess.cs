using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using System.Threading;

namespace PaintServer
{
    public class Flagprocess
    {
        List<TcpClient> clientList = new List<TcpClient>();
        Dictionary<TcpClient, string> ClientList = new Dictionary<TcpClient, string>();

        TcpListener subservsock;
        drawInfo DI = new drawInfo();

        string[] AnswerKey = { "컴퓨터", "물병", "책", "맥주", "과자", "해수면", "스마트폰", "카톡", "무지개", "노트북", "선풍기", "USB", "마스크", "포크레인","칫솔","치약","건전지","향수","포스트잇","인형","에어팟","아이폰","갤럭시","별","달","사람","버즈","가방","모니터","휴지","코카콜라","펩시","캐치마인드","충전기","라면","파스타","마우스","키보드","쓰레기통","빗자루","머리카락","손가락","손금","손바닥","나무젓가락","태블릿" };
        string Answer;

        string _server = "localhost";
        int _port = 3306;
        string _database = "paint";
        string _id = "root";
        string _pw = "1234";
        string connectionAddress;

        public Flagprocess(TcpListener sub)
        {
            connectionAddress = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", _server, _port, _database, _id, _pw);
            subservsock = sub;
            subservsock.Start();
        }

        public void FlagHandleProcess(TcpClient client)
        {

            clientList.Add(client);
            int MAX_SIZE = 50000;  // 가정
            NetworkStream stream = client.GetStream();

            try
            {
                while (true)
                {
                    var buff = new byte[MAX_SIZE];
                    int nbytes = stream.Read(buff, 0, buff.Length);
                    if (nbytes > 0)
                    {
                        Console.WriteLine($"{nbytes}at {DateTime.Now}");

                        string msg = Encoding.UTF8.GetString(buff, 0, nbytes);


                        if (msg == "Login")
                            Login_func(stream, client);
                        else if (msg == "END")
                            End_func(stream, client);
                        else if (msg == "WaitRoomMsg")
                            WaitRoomMsg_func(stream, client);
                        else if (msg == "Refresh")
                            Refresh_func(client);
                        else if (msg == "Makeroom")
                            Makeroom_func(stream, client);
                        else if (msg == "Rooms")
                            Rooms_func(client);
                        else if (msg == "ENDDRAW")
                            EndDraw_func(client, stream);
                        else if (msg == "RoomEnter")
                            RoomEnter_func(client, stream);
                        else if (msg == "DrawRoomMsg")
                            DrawRoomMsg_func(client, stream);
                        else if (msg == "Deleteuserinfo")
                            Deleteuserinfo_func(client);
                        else if (msg == "GameStart")
                        {
                            notify_func(client, "게임을 시작합니다","GameStart");
                            notifyturn_func(client, stream);
                            AnswerKey_func(client, stream);
                            ResetScore_func(client);
                        }
                        else if (msg == "EnterMsg")
                            EnterOutMsg_func(client,stream, "입장", "EnterMsg");
                        else if (msg == "CurentScore") 
                            CurentScore_func(client,stream);

                    }
                    else
                        break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("오류로인한 클라이언트 종료");
                
            }
            finally
            {
                subEndDraw_func(client);
                Deleteuserinfo_func(client);
                Console.WriteLine("클라이언트 종료");
                stream.Close();
                client.Close();
                clientList.Remove(client);
                ClientList.Remove(client);
            }
        }

        private void CurentScore_func(TcpClient client,NetworkStream stream)
        {
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    
                    mysql.Open();
                    string query = $"select * from roomuserinfo where roomnum = (select roomnum from roomuserinfo where clientid = '{ClientList[client]}')";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    MySqlDataReader table = command.ExecuteReader();

                    string save_info="";
                    while (table.Read())
                    {
                        save_info += $"{table[1].ToString()}_";
                        save_info += $"{table[2].ToString()}_";
                    }
                    table.Close();

                    byte[] flag = Encoding.UTF8.GetBytes("CurentScore");
                    stream.Write(flag, 0, flag.Length);
                    
                    Thread.Sleep(200);

                    Console.WriteLine(save_info);
                    byte[] save_Info = Encoding.UTF8.GetBytes(save_info);
                    stream.Write(save_Info, 0, save_Info.Length);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ResetScore_func(TcpClient client)
        {
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"select roomnum from roominfo where clientid = '{ClientList[client]}'";
                    Console.WriteLine(query);
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    MySqlDataReader table = command.ExecuteReader();

                    table.Read();
                    string roomnum = table[0].ToString();
                    table.Close();

                    query = $"update roomuserinfo set clientscore = '0' where roomnum ='{roomnum}'";
                    MySqlCommand command2 = new MySqlCommand(query, mysql);
                    if (command2.ExecuteNonQuery() != 1)
                        Console.WriteLine($"Failed .. {query}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void EnterOutMsg_func(TcpClient client,NetworkStream stream, string input, string flag)
        {
            byte[] flag_ = Encoding.UTF8.GetBytes($"{flag}");

            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"select clientid from roomuserinfo where roomnum = (select roomnum from roomuserinfo where clientid = '{ClientList[client]}')";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    MySqlDataReader table = command.ExecuteReader();

                    while (table.Read())
                    {
                        NetworkStream msgstream = ClientList.FirstOrDefault(x => x.Value == table[0].ToString()).Key.GetStream();

                        if(input == "입장")
                        {
                            msgstream.Write(flag_, 0, flag_.Length);

                            string userid = $"< {ClientList[client]} > 님이 {input}했습니다.";
                            byte[] packet = Encoding.UTF8.GetBytes(userid);
                            msgstream.Write(packet, 0, packet.Length);
                        }
                        else
                        {
                            if (stream != msgstream)
                            {
                                msgstream.Write(flag_, 0, flag_.Length);

                                string userid = $"< {ClientList[client]} > 님이 {input}했습니다.";
                                byte[] packet = Encoding.UTF8.GetBytes(userid);
                                msgstream.Write(packet, 0, packet.Length);
                            }
                        }

                    }
                    table.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void notifyturn_func(TcpClient client, NetworkStream stream)
        {
            byte[] flag_ = Encoding.UTF8.GetBytes("NotYourTurn");
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"select clientid from roomuserinfo where roomnum = (select roomnum from roomuserinfo where clientid = '{ClientList[client]}')";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    MySqlDataReader table = command.ExecuteReader();
                    Thread.Sleep(200);

                    while (table.Read())
                    {
                        NetworkStream drawmsgstream = ClientList.FirstOrDefault(x => x.Value == table[0].ToString()).Key.GetStream();
                        drawmsgstream.Write(flag_, 0, flag_.Length);
                    }
                    table.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Thread.Sleep(200);
            byte[] flag = Encoding.UTF8.GetBytes("YourTurn");
            stream.Write(flag, 0, flag.Length);

        }

        private void notify_func(TcpClient client, string notify,string flag_)
        {
            byte[] flag = Encoding.UTF8.GetBytes(flag_);
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"select clientid from roomuserinfo where roomnum = (select roomnum from roomuserinfo where clientid = '{ClientList[client]}')";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    MySqlDataReader table = command.ExecuteReader();
                    Thread.Sleep(200);
                    while (table.Read())
                    {

                        NetworkStream drawmsgstream = ClientList.FirstOrDefault(x => x.Value == table[0].ToString()).Key.GetStream();
                        drawmsgstream.Write(flag, 0, flag.Length);

                        byte[] packet = Encoding.UTF8.GetBytes(notify);
                        drawmsgstream.Write(packet, 0, packet.Length);


                    }
                    table.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AnswerKey_func(TcpClient client, NetworkStream stream)
        {

            Random rand = new Random();
            int randint = rand.Next(45);
            Answer = AnswerKey[randint];
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"select clientid from roomuserinfo where roomnum = (select roomnum from roomuserinfo where clientid = '{ClientList[client]}')";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    MySqlDataReader table = command.ExecuteReader();

                    byte[] flag = Encoding.UTF8.GetBytes("AnswerKey");
                    Thread.Sleep(200);
                    while (table.Read())
                    {
                        NetworkStream drawstream = ClientList.FirstOrDefault(x => x.Value == table[0].ToString()).Key.GetStream();
                        drawstream.Write(flag, 0, flag.Length);

                        byte[] packet = Encoding.UTF8.GetBytes(AnswerKey[randint]);
                        drawstream.Write(packet, 0, packet.Length);
                    }
                    table.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Deleteuserinfo_func(TcpClient client)
        {
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"delete from userinfo where nickname = '{ClientList[client]}'";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    if (command.ExecuteNonQuery() != 1)
                        Console.WriteLine($"Failed .. {query}");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }



        private void DrawRoomMsg_func(TcpClient client, NetworkStream stream)
        {
            byte[] msg = new byte[1024];
            int len = stream.Read(msg, 0, msg.Length);
            string clientmsg = Encoding.UTF8.GetString(msg, 0, len);

            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"select * from roomuserinfo where roomnum = (select roomnum from roomuserinfo where clientid = '{ClientList[client]}')";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    //MySqlDataReader table = command.ExecuteReader();

                    if (Answer == clientmsg)
                    {
                        notifyturn_func(client, stream);

                        string query_ = $"update roomuserinfo set clientscore = clientscore+1 where clientid = '{ClientList[client]}'";
                        MySqlCommand command2 = new MySqlCommand(query_, mysql);
                        if (command2.ExecuteNonQuery() != 1)
                            Console.WriteLine($"Failed .. {query_}");

                        notify_func(client, $"  ---------- < {ClientList[client]}님 정답! > ----------- ","Notify");

                        query_ = $"select clientscore from roomuserinfo where clientid = '{ClientList[client]}'";
                        MySqlCommand command3 = new MySqlCommand(query_, mysql);
                        MySqlDataReader table_ = command3.ExecuteReader();

                        table_.Read();
                        int myscore = Convert.ToInt32(table_[0].ToString());
                        table_.Close();

                        if (myscore < 2)
                            AnswerKey_func(client, stream);
                        else
                        {
                            Thread.Sleep(200);
                            MySqlDataReader table = command.ExecuteReader();
                            byte[] flag = Encoding.UTF8.GetBytes("Win");
                            while (table.Read())
                            {
                                NetworkStream drawmsgstream = ClientList.FirstOrDefault(x => x.Value == table[1].ToString()).Key.GetStream();
                                drawmsgstream.Write(flag, 0, flag.Length);

                                byte[] packet = Encoding.UTF8.GetBytes($"{ClientList[client]} Win!");
                                drawmsgstream.Write(packet, 0, packet.Length);

                            }
                            table.Close();




                        }

                    }
                    else
                    {
                        string msgplusnickname = $"[{ClientList[client]}] : {clientmsg}";
                        byte[] flag = Encoding.UTF8.GetBytes("DrawRoomMsg");
                        MySqlDataReader table = command.ExecuteReader();
                        while (table.Read())
                        {
                            NetworkStream drawmsgstream = ClientList.FirstOrDefault(x => x.Value == table[1].ToString()).Key.GetStream();
                            drawmsgstream.Write(flag, 0, flag.Length);

                            byte[] packet = Encoding.UTF8.GetBytes(msgplusnickname);
                            drawmsgstream.Write(packet, 0, packet.Length);
                        }
                        table.Close();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void RoomEnter_func(TcpClient client, NetworkStream stream)
        {
            Console.WriteLine("RoomEnter 들어옴");
            byte[] makeroomID_roomnum = new byte[200];
            int len = stream.Read(makeroomID_roomnum, 0, makeroomID_roomnum.Length);
            string[] splitinfo = Encoding.UTF8.GetString(makeroomID_roomnum, 0, len).Split("_");
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"update roominfo set clientcount = clientcount +1 where clientid = '{splitinfo[0]}'";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    if (command.ExecuteNonQuery() != 1)
                        Console.WriteLine($"Failed .. {query}");

                    query = $"insert into roomuserinfo values ('{splitinfo[1]}','{ClientList[client]}','0')";
                    MySqlCommand command2 = new MySqlCommand(query, mysql);
                    if (command2.ExecuteNonQuery() != 1)
                        Console.WriteLine($"Failed .. {query}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Rooms_func(TcpClient client)
        {
            NetworkStream stream = client.GetStream();

            using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
            {
                mysql.Open();
                string query = "select * from roominfo";
                MySqlCommand command = new MySqlCommand(query, mysql);
                MySqlDataReader table = command.ExecuteReader();


                string RoomInfo = "^";
                while (table.Read())
                {
                    /*string roomname = table[0].ToString();
                    string roompw = table[1].ToString();
                    string clientcount = table[2].ToString();
                    string clientid = table[3].ToString();
                    string roomnumber = table[4].ToString();
                    */
                    RoomInfo += table[0].ToString();
                    RoomInfo += $"_{table[1].ToString()}";
                    RoomInfo += $"_{table[2].ToString()}";
                    RoomInfo += $"_{table[3].ToString()}";
                    RoomInfo += $"_{table[4].ToString()}^";

                }
                table.Close();

                Console.WriteLine("방정보     " + RoomInfo);
                byte[] roominfopacket = Encoding.UTF8.GetBytes(RoomInfo);
                byte[] flag = Encoding.UTF8.GetBytes("SearchRoom");

                stream.Write(flag, 0, flag.Length);
                Thread.Sleep(200);
                stream.Write(roominfopacket, 0, roominfopacket.Length);

                //byte[] flag = Encoding.UTF8.GetBytes("EndroomInfo");
                //stream.Write(flag, 0, flag.Length);


            }

        }

        private void Makeroom_func(NetworkStream stream, TcpClient client)
        {
            Console.WriteLine("makeroom 들어옴");
            try
            {
                byte[] packet = new byte[256];
                int len = stream.Read(packet, 0, packet.Length);

                string[] split_packet = Encoding.UTF8.GetString(packet, 0, len).Split("_");

                string query = $"insert into roominfo values('{split_packet[0]}', '{split_packet[1]}', '1', '{ClientList[client]}', '{split_packet[2]}')";
                Console.WriteLine(query);
                Console.WriteLine(Convert.ToInt32(split_packet[2]));
                if (Convert.ToInt32(split_packet[2]) < 6)
                {
                    using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                    {
                        mysql.Open();


                        MySqlCommand command = new MySqlCommand(query, mysql);
                        if (command.ExecuteNonQuery() != 1)
                            Console.WriteLine($"Failed .. {query}");

                        query = $"select roomnum from roominfo where clientid='{ClientList[client]}'";
                        Console.WriteLine(query);

                        Console.WriteLine($"{query} at {DateTime.Now}");

                        MySqlCommand command2 = new MySqlCommand(query, mysql);
                        MySqlDataReader table = command2.ExecuteReader();

                        table.Read();
                        string roomnum = table[0].ToString();
                        table.Close();

                        query = $"insert into roomuserinfo values('{roomnum}','{ClientList[client]}','0')";
                        Console.WriteLine($"{query} at {DateTime.Now}");

                        MySqlCommand command3 = new MySqlCommand(query, mysql);
                        if (command3.ExecuteNonQuery() != 1)
                            Console.WriteLine($"Failed .. {query}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private void Refresh_func(TcpClient client)
        {
            Console.WriteLine("Refresh_func 들어옴");
            byte[] flag = Encoding.UTF8.GetBytes("Refresh");
            string ID_data = $"{ClientList[client]}_";

            foreach (var user in ClientList)
                if (user.Value != ClientList[client])
                    ID_data += $"{user.Value}_";

            NetworkStream stream = client.GetStream();
            stream.Write(flag, 0, flag.Length);

            byte[] ID_data_encode = Encoding.UTF8.GetBytes(ID_data);
            stream.Write(ID_data_encode, 0, ID_data_encode.Length);

        }

        private void WaitRoomMsg_func(NetworkStream stream, TcpClient client)
        {
            byte[] msg = new byte[1024];
            byte[] flag = Encoding.UTF8.GetBytes("WaitRoomMsg");
            int len = stream.Read(msg, 0, msg.Length);


            string msgplusnickname = $"[{ClientList[client]}] : { Encoding.UTF8.GetString(msg, 0, len)}";


            foreach (var temp in ClientList)
            {
                NetworkStream tempstream = temp.Key.GetStream();
                tempstream.Write(flag, 0, flag.Length);

                byte[] packet = Encoding.UTF8.GetBytes(msgplusnickname);
                tempstream.Write(packet, 0, packet.Length);
            }


        }

        private void Login_func(NetworkStream stream, TcpClient client)
        {
            try
            {
                byte[] packet = new byte[256];
                int len = stream.Read(packet, 0, packet.Length);
                string ID = Encoding.UTF8.GetString(packet, 0, len);

                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"select count(*) from userinfo where nickname = '{ID}'";
                    Console.WriteLine($"{query} at {DateTime.Now}");


                    MySqlCommand command = new MySqlCommand(query, mysql);
                    MySqlDataReader table = command.ExecuteReader();

                    table.Read();
                    string loginResult = table[0].ToString();
                    table.Close();

                    Array.Clear(packet, 0, packet.Length);
                    packet = Encoding.UTF8.GetBytes(loginResult);
                    stream.Write(packet, 0, packet.Length);
                    Console.WriteLine($"{loginResult} at {DateTime.Now}");

                    if (loginResult == "0")
                    {
                        TcpClient subclient = subservsock.AcceptTcpClient();
                        Console.WriteLine("클라이언트 접속 : {0}",
                        ((IPEndPoint)subclient.Client.RemoteEndPoint).ToString());
                        Task subt1 = new Task(() => DI.DrawHandleProcess(subclient, ID));
                        subt1.Start();

                        query = $"insert into userinfo values('{ID}')";
                        MySqlCommand command2 = new MySqlCommand(query, mysql);
                        if (command2.ExecuteNonQuery() != 1)
                            Console.WriteLine($"Failed .. {query}");
                        ClientList.Add(client, ID);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void End_func(NetworkStream stream, TcpClient client)
        {
            Console.WriteLine("END_func 들어옴");
            byte[] packet = Encoding.UTF8.GetBytes("EndTask");
            stream.Write(packet, 0, packet.Length);

        }

        private void Draw_func(TcpClient client, byte[] packet)
        {
            try
            {
                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();
                    string query = $"select clientid from roomuserinfo where roomnum = (select roomnum from roomuserinfo where clientid = '{ClientList[client]}')";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    MySqlDataReader table = command.ExecuteReader();

                    while (table.Read())
                    {
                        NetworkStream drawmsgstream = ClientList.FirstOrDefault(x => x.Value == table[0].ToString()).Key.GetStream();
                        drawmsgstream.Write(packet, 0, packet.Length);
                    }
                    table.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void EndDraw_func(TcpClient client, NetworkStream stream)
        {
            Console.WriteLine("ENDDraw_func 들어옴");
            EnterOutMsg_func(client,stream, "퇴장", "OutMsg");
            Thread.Sleep(200);

            byte[] packet = Encoding.UTF8.GetBytes("EndTask");
            stream.Write(packet, 0, packet.Length);

            subEndDraw_func(client);
          
        }
        private void subEndDraw_func(TcpClient client)
        {
            try
            {

                using (MySqlConnection mysql = new MySqlConnection(connectionAddress))
                {
                    mysql.Open();

                    string query = $"select roomnum from roomuserinfo where clientid = '{ClientList[client]}'";
                    MySqlCommand command5 = new MySqlCommand(query, mysql);
                    MySqlDataReader table_ = command5.ExecuteReader();
                    table_.Read();
                    string roomnum_ = table_[0].ToString();
                    table_.Close();

                    query = $"delete from roomuserinfo where clientid = '{ClientList[client]}'";
                    MySqlCommand command = new MySqlCommand(query, mysql);
                    if (command.ExecuteNonQuery() != 1)
                        Console.WriteLine($"Failed .. {query}");

                    query = $"update roominfo set clientcount = clientcount -1 where roomnum = '{roomnum_}'";
                    MySqlCommand command2 = new MySqlCommand(query, mysql);
                    if (command2.ExecuteNonQuery() != 1)
                        Console.WriteLine($"Failed .. {query}");

                    query = $"select clientcount from roominfo where  roomnum = '{roomnum_}'";
                    MySqlCommand command3 = new MySqlCommand(query, mysql);
                    MySqlDataReader table = command3.ExecuteReader();
                    table.Read();
                    string clientcount = table[0].ToString();
                    table.Close();

                    if (clientcount == "0")
                    {
                        query = $"delete from roominfo where  roomnum = '{roomnum_}'";
                        MySqlCommand command4 = new MySqlCommand(query, mysql);
                        if (command4.ExecuteNonQuery() != 1)
                            Console.WriteLine($"Failed .. {query}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}

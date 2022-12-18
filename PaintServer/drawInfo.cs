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
    public class drawInfo
    {
        Dictionary<TcpClient, string> ClientList = new Dictionary<TcpClient, string>();
        string _server = "localhost";
        int _port = 3306;
        string _database = "paint";
        string _id = "root";
        string _pw = "1234";
        string connectionAddress;

        public drawInfo()
        {
            connectionAddress = string.Format("Server={0};Port={1};Database={2};Uid={3};Pwd={4}", _server, _port, _database, _id, _pw);
        }

        public void DrawHandleProcess(TcpClient subclient, string ID)
        {
            ClientList.Add(subclient, ID);
            int MAX_SIZE = 50000;  // 가정
            NetworkStream substream = subclient.GetStream();

            try
            {
                while (true)
                {
                    byte[] buff = new byte[MAX_SIZE];
                    int nbytes = substream.Read(buff, 0, buff.Length);

                    if (nbytes > 0)
                    {
                        if (nbytes < 1500)
                        {
                            string msg = Encoding.UTF8.GetString(buff, 0, nbytes);

                            if (msg == "ENDDRAW")
                                EndDraw_func(subclient, substream);
                        }
                        else
                        {
                            Task t = new Task(() => Draw_func(subclient, substream, buff));
                            t.Start();
                        }
                    }
                    else
                        break;
                }

            }
            catch
            {
                Console.WriteLine("sub오류로인한 클라이언트 종료");
               
            }
            finally
            {
                Console.WriteLine("sub클라이언트 종료");
                substream.Close();
                subclient.Close();
                ClientList.Remove(subclient);
            }

        }


        private void Draw_func(TcpClient client, NetworkStream substream, byte[] packet)
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
                        if (drawmsgstream != substream)
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
        private void EndDraw_func(TcpClient client, NetworkStream substream)
        {
            Console.WriteLine("sub ENDDraw_func 들어옴");
            byte[] packet = Encoding.UTF8.GetBytes("EndTask");
            substream.Write(packet, 0, packet.Length);
        }
    }
}

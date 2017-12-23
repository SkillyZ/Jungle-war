using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Common;
using MySql.Data.MySqlClient;
using GameServer.Tool;

namespace GameServer.Servers
{
    class Client
    {
        private Socket client;
        private Server server;
        private Message msg = new Message();
        private MySqlConnection mysqlConn;

        public Client() { }

        public Client(Socket client, Server server)
        {
            this.client = client;
            this.server = server;
            mysqlConn = ConnHelper.Connect();
        }
        
        public void Start()
        {
            client.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int count = client.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }

                msg.ReadMessage(count, OnPrecessMessage);
                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        private void OnPrecessMessage(RequestCode requestCode, ActionCode actionCode, string data)
        {
            server.HandleRequest(requestCode, actionCode, data, this);
        }

        public void Send(RequestCode requestCode, string data)
        {
            byte[] bytes = Message.PackData(requestCode, data);
            client.Send(bytes);
        }

        private void Close()
        {
            if (client != null)
            {
                ConnHelper.CloseConnection(mysqlConn);
                client.Close();
                server.RemoveCient(this);
            }
        }
    }
}

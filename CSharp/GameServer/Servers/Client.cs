using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace GameServer.Servers
{
    class Client
    {
        private Socket client;
        private Server server;
        private Message msg = new Message();

        public Client() { }

        public Client(Socket client, Server server)
        {
            this.client = client;
            this.server = server;
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

                msg.ReadMessage(count);

                Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        private void Close()
        {
            if (client != null)
            {
                client.Close();
                server.RemoveCient(this);
            }
        }
    }
}

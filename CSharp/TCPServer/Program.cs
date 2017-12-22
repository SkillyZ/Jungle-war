using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCPServer
{
    class Program
    {
        private static byte[] dataBuffer = new byte[1024];
        private static Message msg = new Message();
        static void Main(string[] args)
        {
            StartServerAsync();
            Console.ReadKey();
        }

        static void StartServerAsync()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //IPAddress iPAddress = new IPAddress(new byte[] { 127, 0, 0, 1});
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 88);
            serverSocket.Bind(iPEndPoint);
            serverSocket.Listen(0);
            //Socket client = serverSocket.Accept();
            serverSocket.BeginAccept(AcceptCallBack, serverSocket);

        }

        static void AcceptCallBack(IAsyncResult ar)
        {
            Socket server = ar.AsyncState as Socket;
            Socket client = server.EndAccept(ar);
            string msgStr = "Hello client ! 你好 ....";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msgStr);
            client.Send(data);
            client.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, client);
            server.BeginAccept(AcceptCallBack, server);
        }

        static void ReceiveCallBack(IAsyncResult ar)
        {
            Socket client = null;
            try
            {
                client = ar.AsyncState as Socket;
                int count = client.EndReceive(ar);
                if (count == 0)
                {
                    client.Close();
                    return;
                }
                msg.AddCount(count);
                //string msgStr = Encoding.UTF8.GetString(dataBuffer, 0, count);
                //Console.WriteLine("从客户端接收数据 :" + "--"  + msgStr);
                //client.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, client);
                msg.ReadMessage();
                client.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, client);
            }
            catch (Exception e)
            {
                Console.Write(e);
                if (client != null)
                {
                    client.Close();
                }
            }
            finally
            {
            }
        }

        void StartServerSync()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //IPAddress iPAddress = new IPAddress(new byte[] { 127, 0, 0, 1});
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, 88);
            serverSocket.Bind(iPEndPoint);
            serverSocket.Listen(100);
            Socket client = serverSocket.Accept();

            string msg = "Hello client ! 你好 ....";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(msg);
            client.Send(data);

            byte[] dataBuffer = new byte[1024];
            int count = client.Receive(dataBuffer);
            string msgReceive = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine(msgReceive);

            Console.ReadKey();
            client.Close();
            serverSocket.Close();
        }
    }
}

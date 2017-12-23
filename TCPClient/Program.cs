using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 88));
            byte[] data = new byte[1024];
            int count = client.Receive(data);
            string msg = Encoding.UTF8.GetString(data, 0, count);
            Console.Write(msg);

            //while (true)
            //{
            //    string s = Console.ReadLine();
            //    if (s == "c")
            //    {
            //        client.Close(); return;
            //    }
            //    Console.Write(s);
            //    client.Send(Encoding.UTF8.GetBytes(s));
            //}

            for (int i = 0; i < 100; i++)
            {
                client.Send(Message.GetBytes(i.ToString()));
            }

            Console.ReadKey();
            client.Close();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using GameServer.Controller;
using Common;

namespace GameServer.Servers
{
    class Server
    {
        private IPEndPoint iPEndPoint;
        private Socket server;
        private List<Client> clientList;
        private ControllerManager controllerManager;

        public Server(){}
        public Server(string ip, int port)
        {
            controllerManager = new ControllerManager(this); 
            SetIpAndPort(ip, port);
        }

        public void SetIpAndPort(string ip, int port)
        {
            iPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public void Start()
        {
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(iPEndPoint);
            server.Listen(0);
            server.BeginAccept(AcceptCallBack, null);
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = server.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            clientList.Add(client);
        }

        public void RemoveCient(Client client)
        {
            lock(clientList)
            {
                clientList.Remove(client);
            }
        }

        public void SendResponse(Client client, ActionCode actionCode, string data)
        {
            client.Send(actionCode, data);
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.HandleRequest(requestCode, actionCode, data, client);
        }

    }
}

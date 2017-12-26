using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.Reflection;
using GameServer.Servers;

namespace GameServer.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;

        public ControllerManager (Server server)
        {
            this.server = server;
            Init();
        }

        private void Init()
        {
            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, defaultController);
            controllerDict.Add(RequestCode.User, new UserController());
        }

        public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            BaseController controller;
            bool isGet = controllerDict.TryGetValue(requestCode, out controller);
            if (isGet == false)
            {
                Console.WriteLine("无法得到:" + requestCode+"所对应controller");
                return;
            }
            string methodName = Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi = controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("在controller["+controller.GetType()+"]没有对应的方法:["+methodName+"]"); return;
            }

            object[] parameters = new object[] { data, client, server};
            object o = mi.Invoke(controller, parameters);
            if (o == null || string.IsNullOrEmpty(o as string))
            {
                return;
            }
            server.SendResponse(client, actionCode, o as string);
        }

    }
}

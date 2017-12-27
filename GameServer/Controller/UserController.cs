using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.DAO;
using GameServer.Model;

namespace GameServer.Controller
{
    class UserController : BaseController
    {
        private UserDAO userDAO = new UserDAO();
        public UserController ()
        {
            requestCode = RequestCode.User;
        }

        public string Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user =  userDAO.VerifyUser(client.MysqlConn, strs[0], strs[1]);
            if (user == null)
            {
                //Enum.GetName(typeof(ReturnCode), ReturnCode.Fail);
                return ((int)ReturnCode.Fail).ToString();
            } else
            {
                return ((int)ReturnCode.Successs).ToString();
            }
        }

        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            bool flag = userDAO.GetUserByUsername(client.MysqlConn, strs[0]);
            if (flag)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                userDAO.AddUser(client.MysqlConn, strs[0], strs[1]);
                return ((int)ReturnCode.Successs).ToString();
            }
        }

    }
}

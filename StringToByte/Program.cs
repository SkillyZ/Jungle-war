using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringToByte
{
    class Program
    {
        static void Main(string[] args)
        {
            //byte[] data = Encoding.UTF8.GetBytes("");
            byte[] data = BitConverter.GetBytes(1);
            foreach (byte b in data)
            {
                Console.WriteLine(b + ":");
            }

            Console.ReadKey();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace DvTicTacToe
{
    public class SocketManagement
    {
        private IPAddress _IP;
        private int _PORT;
        private TcpListener _TCP;
        private TcpClient _CLIENT;
        private NetworkStream _STREAM;

        public SocketManagement(String ip, int port)
        {
            _IP = IPAddress.Parse(ip);//parse string into IPAddress
            _PORT = port;
        }

        public bool StartAsServer()
        {
            try
            {
                _TCP = new TcpListener(_IP, _PORT);
                _TCP.Start();
                _CLIENT = GetTcpClient();
                _STREAM = _CLIENT.GetStream();
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); return false; }
            return true;
        }

        public TcpClient GetTcpClient()
        {
            return _TCP.AcceptTcpClient();
        }

        public bool StartAsClient()
        {
            try
            {
                _CLIENT = new TcpClient();
                _CLIENT.Connect(_IP, _PORT);
                _STREAM = _CLIENT.GetStream();
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); return false; }
            return true;
        }

        public bool sendBoard(int[][] obj)
        {
            try
            {
                string temp = "";
                for (int y = 0; y < 3; y++)
                    for (int x = 0; x < 3; x++)
                        temp += obj[y][x];

                byte[] bytes = new byte[255];
                bytes = new ASCIIEncoding().GetBytes(temp);
                _STREAM.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); return false; }
            return true;
        }

        public int[][] getBoard()
        {

            byte[] bytes = new byte[255];
            _STREAM.Read(bytes, 0, bytes.Length);
            string temp = new ASCIIEncoding().GetString(bytes);
            char[] charOfTemp = temp.ToCharArray();
            int[][] obj = { new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 }, new int[] { 0, 0, 0 } };
            for (int y = 0; y < 3; y++)
                for (int x = 0; x < 3; x++)
                    obj[y][x] = Int32.Parse("" + charOfTemp[(y * 3) + x]);
            return obj;
        }
    }
}


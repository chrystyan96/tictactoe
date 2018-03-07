using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DvTicTacToe
{
    public partial class Main : Form
    {
        private bool isServer = false;
        private SocketManagement con;// object for connecting 

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //make the form can be moved with FormBorderStyle.None
            new ResizeForBorderlessForm(this).AllowResizeAll = false;
        }

        private bool checkIPandPort(string ip, string port)
        {
            //Check the ip and port is in valid format
            if (Regex.IsMatch(ip, @"^[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}$")
                && Regex.IsMatch(port, "^[0-9]{1,6}$"))
            {
                string[] temp = ip.Split('.');
                foreach (string q in temp)
                {
                    try
                    {
                        if (Int32.Parse(q) > 255) return false;
                    }
                    catch (Exception) { return false; }
                }
                return true;
            }
            return false;
        }

        private void ConnectAsServer(string ip, int port)
        {
            con = new SocketManagement(ip, port);
            if (con.StartAsServer()) GameStart();
        }
        private void ConnectAsClient(string ip, int port)
        {
            con = new SocketManagement(ip, port);
            if (con.StartAsClient()) GameStart();
        }

        private void EnableAll()
        {
            IpBox.Enabled = true;
            PortBox.Enabled = true;
            StartAsClientBtn.Enabled = true;
            StartAsServerBtn.Enabled = true;
        }
        private void DisableAll()
        {
            IpBox.Enabled = false;
            PortBox.Enabled = false;
            StartAsClientBtn.Enabled = false;
            StartAsServerBtn.Enabled = false;
        }

        private void GameStart()
        {
            this.Hide();
            new GameForm(this, isServer, con).Show();
        }
        private void StartAsServerBtn_Click(object sender, EventArgs e)
        {
            DisableAll();
            if (checkIPandPort(IpBox.Text, PortBox.Text))
            {
                isServer = true;
                ConnectAsServer(IpBox.Text, Int32.Parse(PortBox.Text));
            }
            else EnableAll();
        }

        private void StartAsClientBtn_Click(object sender, EventArgs e)
        {
            DisableAll();
            if (checkIPandPort(IpBox.Text, PortBox.Text))
            {
                ConnectAsClient(IpBox.Text, Int32.Parse(PortBox.Text));
            }
            else EnableAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

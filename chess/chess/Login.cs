using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SQLite;
using System.Net.Sockets;
using System.Net;

namespace chess
{
    public partial class Login : Form
    {
        private TCPClient client = new TCPClient("127.0.0.1", 8080);
        private bool islogin = false;
        public Login()
        {
            
            InitializeComponent();
            //MessageBox.Show("connected");
        }
        /*public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }*/

        private async void btn_login_Click(object sender, EventArgs e)
        {
            
           
        }

        private void btn_tk_TextChanged(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private async void btn_login_Click_1(object sender, EventArgs e)
        {
            
            string username = txt_tk.Text;
            string password = txt_mk.Text;
            string respone = await client.LoginAsync(username, password);
            if (respone.StartsWith("SUCCESS"))
            {
                MessageBox.Show("Đăng nhập thành công!");
                MatchGame matchGame = new MatchGame(client);
                this.Hide();
                matchGame.ShowDialog();
            }
            else
                MessageBox.Show(respone);
        }
    }
}

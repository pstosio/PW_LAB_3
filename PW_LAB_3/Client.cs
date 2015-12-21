using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;

namespace PW_LAB_3
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string host = textBox1.Text;
            int port = System.Convert.ToInt16(numericUpDown1.Value);
            try
            {
                TcpClient klient = new TcpClient(host, port);
                if (klient.Connected)
                    listBox1.Items.Add("Nawiązano połączenie z " + host + " na porcie:" + port);

                // Rozłączenie połączenia
                klient.Close();
            }
            catch(Exception ex)
            {
                listBox1.Items.Add(String.Format("Błąd: Nie udało się nawiązać połączenia :(, bo: {0}", ex.ToString()));
            }
        }
    }
}

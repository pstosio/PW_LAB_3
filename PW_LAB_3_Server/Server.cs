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
using System.Net;
using System.Threading;

namespace PW_LAB_3_Server
{
    public partial class Server : Form
    {
        //pola prywatne
        private TcpListener serwer;
        private TcpClient klient;

        public Server()
        {
            //CheckForIllegalCrossThreadCalls = false;// !!
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPAddress adresIP;
            try
            {
                adresIP = IPAddress.Parse(textBox1.Text);
            }
            catch
            {
                listBox1.Items.Add("Błędny format adresu IP!");
                textBox1.Text = String.Empty;
                return;
            }

            int port = System.Convert.ToInt16(numericUpDown1.Value);
            try
            {
                listBox1.Items.Add("Serwer uruchomiony, oczekuje na połączenie...");
                button1.Enabled = false;

                serwer = new TcpListener(adresIP, port);
                serwer.Start();

                serwer.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientCallback), serwer);
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(String.Format("Błąd inicjacji serwera: {0}", ex.ToString()));
            }
        }

        private void AcceptTcpClientCallback(IAsyncResult asyncResult)
        {
            TcpListener s = (TcpListener)asyncResult.AsyncState;
            klient = s.EndAcceptTcpClient(asyncResult);
            SetListBoxText("Połączenie się powiodło!");

            klient.Close();
            serwer.Stop();
        }

        /// <summary>
        /// Obsługa list Boxa z poziomu wątku
        /// </summary>
        /// <param name="tekst"></param>
        private delegate void SetTextCallBack(string tekst);
        private void SetListBoxText(string tekst)
        {
            if (listBox1.InvokeRequired)
            {
                SetTextCallBack f = new SetTextCallBack(SetListBoxText);
                this.Invoke(f, new object[] { tekst });
            }
            else
            {
                listBox1.Items.Add(tekst);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                serwer.Stop();
                klient.Close();
                listBox1.Items.Add("Zakończono pracę serwera ...");
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.ToString());
            }
        }
    }
}

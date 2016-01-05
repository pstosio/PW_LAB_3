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
using System.IO;

namespace PW_LAB_3_Server
{
    public partial class Server : Form
    {
        //pola prywatne
        private TcpListener serwer;
        private TcpClient client;
        private BinaryReader reader;
        private BinaryWriter writer;

        private Thread readerThread;
        private Thread writerThread;
        public Server()
        {
            //CheckForIllegalCrossThreadCalls = false;// !!
            InitializeComponent();

            // Adres ip v4 hosta
            IPHostEntry adresyIP = Dns.GetHostEntry(Dns.GetHostName());
            label3.Text = adresyIP.AddressList[1].ToString();
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

                // Połączenie TCP
                readerThread = new Thread(getThreadStart(false));
                readerThread.Start();

            }
            catch (Exception ex)
            {
                listBox1.Items.Add(String.Format("Błąd inicjacji serwera: {0}", ex.ToString()));
            }
        }

        private ThreadStart getThreadStart(bool readerFALSE_writerTRUE)
        {
            ThreadStart ts;

            if (readerFALSE_writerTRUE == false)
            {
                ts = delegate()
                {
                    client = serwer.AcceptTcpClient();
                    SetListBoxText("Połączenie się powiodło!");

                    NetworkStream ns = client.GetStream();
                    writer = new BinaryWriter(ns);
                    reader = new BinaryReader(ns);
                    // Odczyt w pętli nieskończonej
                    while (true) SetListBoxText(reader.ReadString());
                };
            }
            else
            {
                ts = delegate()
                {
                    writer.Write(textBox2.Text);

                };
            }

            return ts;
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
                //klient.Close();
                SetListBoxText("Zakończono pracę serwera ...");
            }
            catch (Exception ex)
            {
                listBox1.Items.Add(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SetListBoxText("[JA]: " + textBox2.Text);
            writer.Write(textBox2.Text);
            textBox2.Text = "";
            //writerThread.Start();
        }
    }
}

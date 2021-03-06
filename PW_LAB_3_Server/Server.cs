﻿using System;
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

        private int[,] readedTab, readedTab_2, retTab;
        string s, lastString;

        public Server()
        {
            //CheckForIllegalCrossThreadCalls = false;// !!
            InitializeComponent();

            // Adres ip v4 hosta
            IPHostEntry adresyIP = Dns.GetHostEntry(Dns.GetHostName());
            label3.Text = adresyIP.AddressList[2].ToString();

            readedTab = new int[1024, 1024];
        }

        #region Start servera
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
        #endregion
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
                    while (true)
                    {
                        lastString = reader.ReadString();
                        /*

                        if (lastString.Substring(0, 3) == "0 W")
                        {
                            SetListBoxText(reader.ReadString());
                            
                            // "Rozkładanie" string do tablicy - przesłanej ćwiartki
                            for(int i=0, j=0; i<s.Length; i++)
                            {
                                if (j == 256) j = 0;
                                readedTab[i, j] = Convert.ToInt32( s.Substring(i, 1) );
                                j++;
                            }
                        }

                        else if (lastString.Substring(0, 3) == "1 W")
                        {
                            SetListBoxText(lastString);

                            // "Rozkładanie" string do tablicy - przesłanej ćwiartki
                            for (int i = 0, j = 0; i < s.Length; i++)
                            {
                                if ( i % 256 == 0) j ++;
                                readedTab_2[i, j] = Convert.ToInt32(s.Substring(i, 1));
                            }
                        }

                        if (lastString.Substring(0,4) == "Licz")
                        {
                            for(int i=0; i < 1024; i++)
                            {
                                for(int j=0; j<256; j++)
                                {
                                    for(int k=0; k < 1024; k++)
                                    {
                                        retTab[i,j] += readedTab[i, k] * readedTab_2[k, j];
                                    }
                                }
                            }

                            writer.Write(String.Format("Zwracam: {0}", this.getStringFromTab(retTab, false)));
                        }
                        */
                        SetListBoxText(lastString);
                    }
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
        }

        private string getStringFromTab(int[,] tab, bool poziomaTruePionowaFalse)
        {
            string ret = "";

            if (poziomaTruePionowaFalse == false)
            {
                for (int i = 0; i < 1024; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        ret += tab[i, j].ToString();
                    }
                }
            }
            else
            {
                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 1024; j++)
                    {
                        ret += tab[i, j].ToString();
                    }
                }
            }

            return ret;
        }
    }
}

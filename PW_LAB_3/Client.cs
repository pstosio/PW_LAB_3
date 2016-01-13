using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace PW_LAB_3
{
    public partial class Client : Form
    {
        #region variables
        private Thread readerThread_1;
        private Thread readerThread_2;
        private Thread readerThread_3;
        private Thread readerThread_4;

        private Thread writerThread_1;
        private Thread writerThread_2;
        private Thread writerThread_3;
        private Thread writerThread_4;

        private TcpClient client_1;
        private TcpClient client_2;
        private TcpClient client_3;
        private TcpClient client_4;

        private BinaryReader reader_1;
        private BinaryReader reader_2;
        private BinaryReader reader_3;
        private BinaryReader reader_4;

        private BinaryWriter writer_1;
        private BinaryWriter writer_2;
        private BinaryWriter writer_3;
        private BinaryWriter writer_4;

        private NetworkStream ns_1;
        private NetworkStream ns_2;
        private NetworkStream ns_3;
        private NetworkStream ns_4;

        private Matrix matrix_1;
        private Matrix matrix_2;
        private Matrix matrix_3;

        private string ret;
        #endregion

        public Client()
        {
            InitializeComponent();
        }

        #region Połączenia
        private void button1_Click(object sender, EventArgs e)
        {
            string host = textBox1.Text;
            int port = System.Convert.ToInt16(numericUpDown1.Value);

            try
            {
                client_1 = new TcpClient(host, port);
                if (client_1.Connected)
                    SetListBoxText("Nawiązano połączenie z " + host + " na porcie:" + port, 1);

                ns_1 = client_1.GetStream();

                readerThread_1 = new Thread(getThreadStart(false, 1));
                writerThread_1 = new Thread(getThreadStart(true, 1));
                
                readerThread_1.Start();
                writerThread_1.Start();
            }
            catch(Exception ex)
            {
                SetListBoxText(String.Format("Błąd: Nie udało się nawiązać połączenia :(, bo: {0}", ex.ToString()), 1);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string host = textBox4.Text;
            int port = System.Convert.ToInt16(numericUpDown2.Value);

            try
            {
                client_2 = new TcpClient(host, port);
                if (client_2.Connected)
                    SetListBoxText("Nawiązano połączenie z " + host + " na porcie:" + port, 2);

                ns_2 = client_2.GetStream();

                readerThread_2 = new Thread(getThreadStart(false, 2));
                writerThread_2 = new Thread(getThreadStart(true, 2));

                readerThread_2.Start();
                writerThread_2.Start();
            }
            catch (Exception ex)
            {
                SetListBoxText(String.Format("Błąd: Nie udało się nawiązać połączenia :(, bo: {0}", ex.ToString()), 2);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string host = textBox6.Text;
            int port = System.Convert.ToInt16(numericUpDown3.Value);

            try
            {
                client_3 = new TcpClient(host, port);
                if (client_3.Connected)
                    SetListBoxText("Nawiązano połączenie z " + host + " na porcie:" + port, 3);

                ns_3 = client_3.GetStream();

                readerThread_3 = new Thread(getThreadStart(false, 3));
                writerThread_3 = new Thread(getThreadStart(true, 3));

                readerThread_3.Start();
                writerThread_3.Start();
            }
            catch (Exception ex)
            {
                SetListBoxText(String.Format("Błąd: Nie udało się nawiązać połączenia :(, bo: {0}", ex.ToString()), 3);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string host = textBox8.Text;
            int port = System.Convert.ToInt16(numericUpDown4.Value);

            try
            {
                client_4 = new TcpClient(host, port);
                if (client_4.Connected)
                    SetListBoxText("Nawiązano połączenie z " + host + " na porcie:" + port, 4);

                ns_4 = client_4.GetStream();

                readerThread_4 = new Thread(getThreadStart(false, 4));
                writerThread_4 = new Thread(getThreadStart(true, 4));

                readerThread_4.Start();
                writerThread_4.Start();
            }
            catch (Exception ex)
            {
                SetListBoxText(String.Format("Błąd: Nie udało się nawiązać połączenia :(, bo: {0}", ex.ToString()), 4);
            }
        }
        #endregion

        #region Rozłączenia
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Rozłączenie połączenia
                readerThread_1.Abort();
                if (client_1.Connected) client_1.Close();
                SetListBoxText("Rozłączono.", 1);
            }
            catch (Exception ex)
            {
                SetListBoxText(ex.ToString(), 1);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                // Rozłączenie połączenia
                readerThread_2.Abort();
                if (client_2.Connected) client_2.Close();
            }
            catch (Exception ex)
            {
                SetListBoxText(ex.ToString(), 2);
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                // Rozłączenie połączenia
                readerThread_3.Abort();
                if (client_3.Connected) client_3.Close();
            }
            catch (Exception ex)
            {
                SetListBoxText(ex.ToString(), 3);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                // Rozłączenie połączenia
                readerThread_4.Abort();
                if (client_4.Connected) client_4.Close();
            }
            catch (Exception ex)
            {
                SetListBoxText(ex.ToString(), 4);
            }
        }
        #endregion

        #region Thread
        private ThreadStart getThreadStart(bool readerFALSE_writerTRUE, int serverNum)
        {
            ThreadStart ts;

            if (readerFALSE_writerTRUE == false)
            {
                ts = delegate()
                {
                    switch (serverNum)
                    {
                        case 1:
                            using (reader_1 = new BinaryReader(ns_1))
                            {
                                // Odczyt w pętli nieskończonej
                                while (true) SetListBoxText(reader_1.ReadString(), 1);
                            }

                        case 2:
                            using (reader_2 = new BinaryReader(ns_2))
                            {
                                // Odczyt w pętli nieskończonej
                                while (true) SetListBoxText(reader_2.ReadString(), 2);
                            }

                        case 3:
                            using (reader_3 = new BinaryReader(ns_3))
                            {
                                // Odczyt w pętli nieskończonej
                                while (true) SetListBoxText(reader_3.ReadString(), 3);
                            }
                        case 4:
                            using (reader_4 = new BinaryReader(ns_4))
                            {
                                // Odczyt w pętli nieskończonej
                                while (true) SetListBoxText(reader_4.ReadString(), 4);
                            }
                    }

                };
            }
            else
            {
                ts = delegate()
                {
                    switch (serverNum)
                    {
                        case 1:
                            writer_1 = new BinaryWriter(ns_1);
                            break;

                        case 2:
                            writer_2 = new BinaryWriter(ns_2);
                            break;

                        case 3:
                            writer_3 = new BinaryWriter(ns_3);
                            break;

                        case 4:
                            writer_4 = new BinaryWriter(ns_4);
                            break;
                    }

                };
            }

            return ts;
        }
        #endregion

        #region Obsługa List Box'ów
        /// <summary>
        /// Obsługa list Boxa z poziomu wątku
        /// </summary>
        /// <param name="tekst"></param>
        private delegate void SetTextCallBack(string tekst, int listBoxNum);
        private void SetListBoxText(string tekst, int listBoxNum)
        {
            switch (listBoxNum)
            {
                case 1:
                    if (listBox1.InvokeRequired)
                    {
                        SetTextCallBack f = new SetTextCallBack(SetListBoxText);
                        this.Invoke(f, new object[] { tekst, listBoxNum });
                    }
                    else
                    {
                        listBox1.Items.Add(tekst);
                    }
                    break;

                case 2:
                    if (listBox2.InvokeRequired)
                    {
                        SetTextCallBack f = new SetTextCallBack(SetListBoxText);
                        this.Invoke(f, new object[] { tekst, listBoxNum });
                    }
                    else
                    {
                        listBox2.Items.Add(tekst);
                    }
                    break;

                case 3:
                    if (listBox3.InvokeRequired)
                    {
                        SetTextCallBack f = new SetTextCallBack(SetListBoxText);
                        this.Invoke(f, new object[] { tekst, listBoxNum });
                    }
                    else
                    {
                        listBox3.Items.Add(tekst);
                    }
                    break;

                case 4:
                    if (listBox4.InvokeRequired)
                    {
                        SetTextCallBack f = new SetTextCallBack(SetListBoxText);
                        this.Invoke(f, new object[] { tekst, listBoxNum });
                    }
                    else
                    {
                        listBox4.Items.Add(tekst);
                    }
                    break;

                case 5:
                    if (listBox5.InvokeRequired)
                    {
                        SetTextCallBack f = new SetTextCallBack(SetListBoxText);
                        this.Invoke(f, new object[] { tekst, listBoxNum });
                    }
                    else
                    {
                        listBox5.Items.Add(tekst);
                    }
                    break;
            }
            
        }
        #endregion

        #region Wysyłka komunikatów
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                SetListBoxText("[JA]: " + textBox2.Text, 1);
                writer_1.Write(textBox2.Text);
                textBox2.Text = "";
            }
            catch (Exception ex)
            {
                SetListBoxText(ex.ToString(), 1);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                SetListBoxText("[JA]: " + textBox3.Text, 2);
                writer_2.Write(textBox3.Text);
                textBox3.Text = "";
            }
            catch (Exception ex)
            {
                SetListBoxText(ex.ToString(), 2);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                SetListBoxText("[JA]: " + textBox5.Text, 3);
                writer_3.Write(textBox5.Text);
                textBox5.Text = "";
            }
            catch (Exception ex)
            {
                SetListBoxText(ex.ToString(), 3);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                SetListBoxText("[JA]: " + textBox7.Text, 4);
                writer_4.Write(textBox7.Text);
                textBox7.Text = "";
            }
            catch (Exception ex)
            {
                SetListBoxText(ex.ToString(), 4);
            }
        }
        #endregion

        #region Utworzenie macierzy
        /// <summary>
        /// Inicjalizacja i wypełnienie macierzy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            matrix_1 = new Matrix();
            matrix_2 = new Matrix();
            matrix_3 = new Matrix();

            matrix_1.drawNumbers();
            matrix_2.drawNumbers();
            matrix_3.drawNumbers();

            SetListBoxText("Macierze wygenerowane !", 5);

        }
        #endregion

        private void button11_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            double[,] tmpMatrix1 = new double[1024, 1024];
            double[,] tmpMatrix2 = new double[1024, 1024];

            SetListBoxText("Mnożenie lokalnie..", 5);
            sw.Start();

            for( int i=0; i < 1024; i++)
            {
                for (int j=0; j < 1024; j++)
                {
                    tmpMatrix1[i, j] = matrix_1.matrix[j, i] * matrix_2.matrix[i, j]; 
                }
            }

            for (int i = 0; i < 1024; i++)
            {
                for (int j = 0; j < 1024; j++)
                {
                    tmpMatrix2[i, j] = tmpMatrix1[j, i] * matrix_3.matrix[i, j];
                }
            }

            sw.Stop();
            SetListBoxText("Policzone w czasie: " + sw.ElapsedMilliseconds + " ms.", 5);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            /*
             * 1. Dzielę macierze 1 i 2 na 4 części
             * 2. Wysyłam po jednej cześci z każdej macierzy na serwer
             * 3. Oczekuję na wyniki
             * 4. Składam to co dostałem w macierz
             * 5. Dzielę macierze wynikową i 3 na 4 części
             * 6. Wysyłam po jeden częsci z każdej macierzy na serwer
             * 7. Oczekuję na wyniki
             * 8. Składam w całośći. 
             * 9. Po drodze mierzę czasy komunikacji
             */

            double[,] m1_1 = new double[512, 512];
            double[,] m1_2 = new double[512, 512];
            double[,] m1_3 = new double[512, 512];
            double[,] m1_4 = new double[512, 512];

            double[,] m2_1 = new double[512, 512];
            double[,] m2_2 = new double[512, 512];
            double[,] m2_3 = new double[512, 512];
            double[,] m2_4 = new double[512, 512];

            Stopwatch sw = new Stopwatch();

            for (int i = 0; i < 1024; i++)
            {
                for (int j = 0; j < 1024; j++)
                {
                    // Pierwsza ćwiartka macierzy
                    if (i < 512 && j < 512)
                    {
                        m1_1[i, j] = matrix_1.matrix[i, j];
                        m2_1[i, j] = matrix_2.matrix[i, j];
                    }
                    // Druga ćwiartka macierzy
                    else if (i >= 512 && j < 512)
                    {
                        m1_2[i - 512, j] = matrix_1.matrix[i, j];
                        m2_2[i - 512, j] = matrix_2.matrix[i, j];
                    }
                    // Trzecia ćwiartka macierzy
                    else if (i < 512 && j >= 512)
                    {
                        m1_3[i, j - 512] = matrix_1.matrix[i, j];
                        m2_3[i, j - 512] = matrix_2.matrix[i, j];
                    }
                    // Czwarta ćwiartka macierzy
                    else
                    {
                        m1_4[i - 512, j - 512] = matrix_1.matrix[i, j];
                        m2_4[i - 512, j - 512] = matrix_2.matrix[i, j];
                    }
                }
            }

            SetListBoxText("Macierz podzielona, zaczynam wysyłać.", 5);
            
            // Wysyłka macierzy na serwery
            sw.Start();
            this.sendMatrixToServer(m1_1, 1);
            this.sendMatrixToServer(m2_1, 1);

            this.sendMatrixToServer(m1_2, 2);
            this.sendMatrixToServer(m2_2, 2);

            this.sendMatrixToServer(m1_3, 3);
            this.sendMatrixToServer(m2_3, 3);

            this.sendMatrixToServer(m1_4, 4);
            this.sendMatrixToServer(m2_4, 4);
            sw.Stop();

            SetListBoxText(String.Format("Do serwera jeden wysłane w czasie: {0}", sw.ElapsedMilliseconds.ToString()), 5);


        }

        private void sendMatrixToServer(double[,] matrix, int serverNum)
        {
            try
            {
                switch(serverNum)
                {
                    case 1:
                        // Serwer 1
                        writer_1.Write("Start wysyłki...");
                        writer_1.Write(getStringFromTab(matrix));
                        writer_1.Write(String.Format("Wysłano {0} elementów.", matrix.Length));
                        break;

                    case 2:
                        // Serwer 2
                        writer_2.Write("Start wysyłki...");
                        writer_2.Write(getStringFromTab(matrix));
                        writer_2.Write(String.Format("Wysłano {0} elementów.", matrix.Length));
                        break;

                    case 3:
                        // Serwer 3
                        writer_3.Write("Start wysyłki...");
                        writer_3.Write(getStringFromTab(matrix));
                        writer_3.Write(String.Format("Wysłano {0} elementów.", matrix.Length));
                        break;

                    case 4:
                        // Serwer 4
                        writer_4.Write("Start wysyłki...");
                        writer_4.Write(getStringFromTab(matrix));
                        writer_4.Write(String.Format("Wysłano {0} elementów.", matrix.Length));
                        break;
                }
                
            }
            catch (Exception ex)
            {
                SetListBoxText(ex.ToString(), 5);
            }
        }

        public string getStringFromTab(double[,] tab)
        {

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 512; j++)
                {
                    ret += tab[i, j].ToString();
                }
            }

            return ret;
        }
    }
}

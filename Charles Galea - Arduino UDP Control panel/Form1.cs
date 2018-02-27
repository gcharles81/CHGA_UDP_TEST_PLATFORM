using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using Ini;
//The source and destination port are the same !!!!

namespace WindowsFormsApplication1
{
    
    public partial class Form1 : Form
    {
        /// <RElated to Ini Config file added 14.02.2018 >
        String newMessage;
        String newMessageA = "TEST1";
        String newMessage1B = "TEST2";
        String newMessageC = "Test SN Number 007";
        /// </summary>

        String FIRST_LETTER;	// First Character of the UDP packet - can be (W) rest unused
        int Second_DIGIT = 0;
        string[] parts = new String[55];

        Boolean Connection_TRY = false;
        Boolean TESTBB = true;
        String ID_Received = ".................";
        Boolean AUTO = false;
        String Connect_phrase = "Z:255";
        String DisConnect_phrase = "Z:1";
        int CHGA = 0;
        UdpClient udpclient;
        IPEndPoint remoteipendpoint;
        IPEndPoint sourceipendpoint;
       // IPAddress ipaddress;
        Thread thread;
        private bool run = false;
        string returnData = "--------";                     //received data from UDP(initial value "????????"
        string remoteIP = string.Empty;                     //remote IP
        byte[] receiveBytes = new byte[32];                 //receive data buffer   
        private bool udpCheck = false;                      //Check Open or Close state of the button


        /// <CHGA_NEW2018>
        /// ///////////////
        // UDP packet to transmit
        int[] data2 = new int[4]{ 0, 0, 0, 0 };
        int[] data3 = new int[2]{ 0, 0 };
        int[] data5 = new int[4]{ 0, 0, 0, 0 };
        int[] Buffer = new int[8]{ 0, 0, 0, 0, 0, 0, 0, 0 };
        int[] table = new int[8]{0, 0, 0, 0, 0, 0, 0, 0};
        String[] HZT = new String[8];
        ///////////********
        Char Timers_Digit_00 = ' ';
        int Timers_Digit_01 = 0;
        int Timers_Digit_02 = 0;
        int Timers_Digit_03 = 0;
        int Timers_Digit_04 = 0;
        int Timers_Digit_05 = 0;
        int Timers_Digit_06 = 0;
        int Timers_Digit_07 = 0;

        Char[] Table2 = new Char[8];


        int value1 = 44;
        int value2 = 180;
        int value3 = 100;
        int value4 = 030;
        int value5 = 220;
        int value6 = 11;
        int Count = 0;

        //////////////******
        Char ASCII_TEST = 'D';
        /// </summary>


        public Form1()
        {
            InitializeComponent();
            FOLDERS();
            Connect_button.Visible = false;
            Disconnect_button.Visible = false;
            FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            
            String path21 = "config/config.ini";
            if (File.Exists(path21))
            {
                Load__config_file();
            }

        }

        private void FOLDERS()
        {

            String path18 = "config";

            if (Directory.Exists(path18))
            {

            }
            else
            {
                DirectoryInfo di = Directory.CreateDirectory(path18);
            }
        }

        private void Generate__config_file()
        {
            ///////GENERATEATE File to store set values to be loaded on startup
            /////// Controller IP Address 
            /////// Port
            /////// ID 


            IniFile ini = new IniFile("config/config.ini");// Create file  CHGA 14/2/2018 

            ini.IniWriteValue("USER_VALUES", "File info ", "//File format created by Charles Galea 2018.");
            ini.IniWriteValue("USER_VALUES", "File Created ", DateTime.Now.ToString());
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            ini.IniWriteValue("ID", "ID", newMessageC);

            //String hourOn1 = comboBox16.Text;
            ini.IniWriteValue("IP", "IP", destipTextBox.Text.ToString());


            // String minOn1 = comboBox13.Text;
            ini.IniWriteValue("PORT", "PORT", portTextBox.Text.ToString());
        }
        private void Load__config_file()
        {
            IniParser parser = new IniParser("config/config.ini");


            //// Load Sunrise Stored values in timers.ini file 



            newMessage = parser.GetSetting("ID", "ID");//// Load Sunrise hourOn1 Stored values in timers.ini file 

            toolStripStatusLabel4.Text = newMessage.ToString();


            newMessage = parser.GetSetting("IP", "IP");//// Load Sunrise minOn1 Stored values in timers.ini file 

            destipTextBox.Text = newMessage.ToString();

            newMessage = parser.GetSetting("PORT", "PORT");//// Load Sunrise minOn1 Stored values in timers.ini file 

            portTextBox.Text = newMessage.ToString();
        }

        //Receiving UDP packets Thread
        private void receiveUDP()
        {

            
            while (run)
            {
                this.Invoke(new EventHandler(statusExpress));
                try
                {

                    receiveBytes = udpclient.Receive(ref remoteipendpoint);
                    remoteIP = remoteipendpoint.Address.ToString();
                    returnData = Encoding.ASCII.GetString(receiveBytes);
                    this.Invoke(new EventHandler(statusExpress));
                }
                catch

                {

                        returnData = "W:--------";
                        remoteIP = string.Empty;
                        this.Invoke(new EventHandler(statusExpress));
                        
                }
            }
        }
        private void UDP_Check_First_Digit()
        {



            if (FIRST_LETTER == "W")
            {

            //    label34.Text = (parts[1]);

            }

            else
            {
                //      richTextBox2.Text += parts[1];
                //     richTextBox2.Text += " \r\n";
            }

        }
        
        private void statusExpress(object sender, EventArgs e)
        {

            string str = returnData;

            char[] seps = { ':' };

            parts = str.Split(seps);

            FIRST_LETTER = (parts[0]);
            Second_DIGIT = Int16.Parse(parts[1]);

            if (FIRST_LETTER == "W") // W ONLY AFTER Connection / disconnection request 
            {
                switch (Second_DIGIT)
                {
                    case 100:// After connection First Reply
                        {
                            System.Console.WriteLine("Low number");
                            break;
                        }
                    case 200:// Board FW Version Received
                        {
                            System.Console.WriteLine("Low number");
                            break;
                        }
                    case 050:// Board FW Programmed date
                        {
                            System.Console.WriteLine("Low number");
                            break;
                        }

                    case 150:// BOARD CPU SN  
                        {
                            System.Console.WriteLine("Medium number");
                            break;
                        }
                    default:// Unknown Commanr received by Controller
                        {
                            System.Console.WriteLine("Other number");
                            break;
                        }
                }



                label34.Text = (parts[1]);
                return;
            }

            else if (FIRST_LETTER == "Z")// Z Aknowledge 
            {
                richTextBox2.Text = parts[1].ToString();
                richTextBox2.Text += " \r\n";
                return;
            }

            else
            {
                return;
            }

        }
        private void sendButton_Click(object sender, EventArgs e)
        {
            try
            {        
                byte[] command = Encoding.ASCII.GetBytes(commandTextBox.Text.ToUpper());      //nevermind lower or upper - case....

                udpclient.Send(command, command.Length, sourceipendpoint);               
            }

            catch
            {
                
            }

        }

        private void Test_connection_Request()
        {
            Connection_TRY = true;//set it true to know status of connection 

        }

        private void openPort_Click(object sender, EventArgs e)
        {
          //  

            if (udpCheck == false) //Check Open or Close state of the button
            {
                try
                {
                    
                    udpclient = new UdpClient(Int32.Parse(portTextBox.Text));
                    IPAddress ipaddress = IPAddress.Parse(destipTextBox.Text);
                    remoteipendpoint = new IPEndPoint(IPAddress.Any,0);                             //remote port IPEndpoint
                    sourceipendpoint = new IPEndPoint(ipaddress, Int32.Parse(portTextBox.Text));    //source port IPEndpoint
                    openPort.Text = "Close Port";
                    run = true;
                    thread = new Thread(new ThreadStart(receiveUDP));
                    thread.IsBackground = true;
                    thread.Start();

                    udpCheck = true;
                    

                }

                catch
                {

                    MessageBox.Show("Cannot bind the socket", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    udpCheck = false;

                    try
                    {
                        if (udpclient == null) { }
                        else { udpclient = null; udpclient.Close(); }
                    }

                    catch
                    {
                    }
                    return;

                    
                }
            }

            else
            {
                try
                {
                    udpclient.Close();
                    udpCheck = false;
                    openPort.Text = "Open Port";
                    recipTextBox.Text = string.Empty;
                    thread = null;
                    run = false;
                    thread.Abort();
                }


                catch
                {
                }
            }

          if (udpCheck == true)

            {
               
                Connect_button.Visible = true;
            }
            
        }


        private void Form1_FormClosing(object sender, EventArgs e)
        {
            if (thread != null && thread.IsAlive)
            {
                thread.Abort();
            }
        }

        void CHARLES_test()
        {
            CHGA++;
            
            try
            {
                byte[] command = Encoding.ASCII.GetBytes("A:1:9:0:5");      //nevermind lower or upper - case....
                udpclient.Send(command, command.Length, sourceipendpoint);
            }

            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AUTO = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(AUTO == true)
            {
                CHARLES_test();
                Application.DoEvents();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AUTO = false;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void CONNECT()
        {
            try
            {
                byte[] command = Encoding.ASCII.GetBytes(Connect_phrase);//nevermind lower or upper - case....
                udpclient.Send(command, command.Length, sourceipendpoint);
            }

            catch
            {
            }
        }
        private void DISCONNECT()
        {
            try
            {
                byte[] command = Encoding.ASCII.GetBytes(DisConnect_phrase);//nevermind lower or upper - case....
                udpclient.Send(command, command.Length, sourceipendpoint);
            }

            catch
            {
            }
        }
        private void Send_data_to_slave_1()
        {

            table[0] = ASCII_TEST; 

                 table[1] = value1;
                 table[2] = value2;
                 table[3] = value3;
                 table[4] = value4;
                 table[5] = value5;
                 table[6] = Count;
                 table[7] = 255;

                                  

            Buffer[0] = table[0];
            Buffer[1] = value1;
            Buffer[2] = value2;
            Buffer[3] = value3;
            Buffer[4] = value4;
            Buffer[5] = value5;
            Buffer[6] = Count;//table[6];
            Buffer[7] = 255;



            Count++;



            ///below only for test 
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                richTextBox2.Clear();
                table[0] = ASCII_TEST;

                table[1] = value1;
                table[2] = value2;
                table[3] = value3;
                table[4] = value4;
                table[5] = value5;
                table[6] = Count;
                table[7] = 255;



                Buffer[0] = table[0];
                Buffer[1] = value1;
                Buffer[2] = value2;
                Buffer[3] = value3;
                Buffer[4] = value4;
                Buffer[5] = value5;
                Buffer[6] = Count;//table[6];
                Buffer[7] = 255;

                // String result = [] Buffer.ToString<int>();
                var builder = new StringBuilder();
                Array.ForEach(Buffer, x => builder.Append(x));
                var res = builder.ToString();
                Count++;
                richTextBox2.Text = res;
                //byte[] command = Encoding.ASCII.GetBytes(commandTextBox.Text.ToUpper());      //nevermind lower or upper - case....
                byte[] command = Encoding.ASCII.GetBytes(res);      //nevermind lower or upper - case....
                udpclient.Send(command, command.Length, sourceipendpoint);
            }

            catch
            {

            }
            */
                
                richTextBox2.Clear();
                table[0] = Timers_Digit_00;

                table[1] = Timers_Digit_01;
                table[2] = Timers_Digit_02;
                table[3] = Timers_Digit_03;
                table[4] = Timers_Digit_04;
                table[5] = Timers_Digit_05;
                table[6] = Timers_Digit_06;
                table[7] = 255;



                Buffer[0] = table[0];
                Buffer[1] = Timers_Digit_01;
                Buffer[2] = Timers_Digit_02;
                Buffer[3] = Timers_Digit_03;
                Buffer[4] = Timers_Digit_04;
                Buffer[5] = Timers_Digit_05;
                Buffer[6] = Timers_Digit_06;
                Buffer[7] = 255;

                char[] C = string.Join(string.Empty, Buffer).ToCharArray();

                // String result = [] Buffer.ToString<int>();
                var builder = new StringBuilder();
                Array.ForEach(Buffer, x => builder.Append(x));
                var res = builder.ToString();
                Count++;
                richTextBox2.Text = res;
               
                char[] array = res.ToCharArray();
                //byte[] command = Encoding.ASCII.GetBytes(commandTextBox.Text.ToUpper());      //nevermind lower or upper - case....
                byte[] command = Encoding.UTF8.GetBytes(C); //res; // Encoding.ASCII.GetBytes(res);      //nevermind lower or upper - case....
                udpclient.Send(command, command.Length, sourceipendpoint);

                label32.Text = C.ToString();
             
                //  udpclient.Send(Buffer, Buffer.Length, sourceipendpoint);
            }

            catch
            {

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Timers_Digit_00 = Char.Parse(comboBox1.SelectedItem.ToString());
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Timers_Digit_01 = int.Parse(comboBox2.SelectedItem.ToString());
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Timers_Digit_02 = int.Parse(comboBox3.SelectedItem.ToString());
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Timers_Digit_03 = int.Parse(comboBox4.SelectedItem.ToString());
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Timers_Digit_04 = int.Parse(comboBox5.SelectedItem.ToString());
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            Timers_Digit_05 = int.Parse(comboBox6.SelectedItem.ToString());
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            Timers_Digit_06 = int.Parse(comboBox7.SelectedItem.ToString());
        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            Timers_Digit_07 = int.Parse(comboBox8.SelectedItem.ToString());
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
            

                richTextBox2.Clear();
                HZT[0] = Timers_Digit_00.ToString();

                HZT[1] = Timers_Digit_01.ToString();
                HZT[2] = Timers_Digit_02.ToString();
                HZT[3] = Timers_Digit_03.ToString();
                HZT[4] = Timers_Digit_04.ToString();
                HZT[5] = Timers_Digit_05.ToString();
                HZT[6] = Timers_Digit_06.ToString();
                HZT[7] = 255.ToString();



               

                
                // String result = [] Buffer.ToString<int>();
                
                string.Join(":", HZT);
                var builder = new StringBuilder();
                Array.ForEach(HZT, x => builder.Append(x));
                var result = String.Join(":", HZT.ToArray());
       //         var res = builder.ToString();
         //       var vfr = result.ToString();
                Count++;
                richTextBox2.Text = result;
                byte[] command = Encoding.UTF8.GetBytes(result); //res; // Encoding.ASCII.GetBytes(res);      //nevermind lower or upper - case....
                udpclient.Send(command, command.Length, sourceipendpoint);
                label32.Text = result.ToString();
               


            }

            catch
            {

            }

        }

        private void Connect_button_Click(object sender, EventArgs e)
        {
                 Connection_TRY = true;
          //  if (Connection_TRY == false)
            //{
                MessageBox.Show("JZTSU");
                Connection_TRY = true;
                
                CONNECT();
                Connect_button.Visible = false;
                Disconnect_button.Visible = true;


            // Thread.Sleep(1000);
            //   Connection_TRY = false;


            //}
            label34.Text = ID_Received;
            Generate__config_file();
            Application.DoEvents();
        }

        private void Disconnect_button_Click(object sender, EventArgs e)
        {
            
            DISCONNECT();
            Disconnect_button.Visible = false;
            Connect_button.Visible = true;
            label34.Text = ".............";
            Application.DoEvents();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ///sjadhfkashdökfhsaöihföisdahföiuhsaduif
        }
    }
}

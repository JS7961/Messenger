
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using WatsonTcp;

namespace Messenger.Data
{
    public static class Client
    {




      



            static WatsonTcpClient Client1,Client2;

            public static string Exepfad = System.Reflection.Assembly.GetExecutingAssembly().Location;

        public static string BinFolder;

            static string Pfad = "";

            public static bool GetFinished,GetPaused;
        public static bool xxx;

        static string DistributionServerIp = "";
        
        static int ServerPort;

        static string OwnServerIP = "";

        public static bool ServerTest;

        public delegate void ClientDelegate();
        public static ClientDelegate MessageReceived,LoginChanged;

       

        private static int wait;

        public static void SetWait(double wert)
        {
            
            
                int test = Convert.ToInt32( wait*wert );

                if (test > 1000)
                {
                    test = 1000;
                }

                if (test < 100)
                {
                    test = 100;
                }

                wait = test; 
            
        }


        static Client()
        {
            ServerTest = true;

            BinFolder = GetBinFolder(Exepfad);            

            Exepfad = Abschneiden(Exepfad);

            Pfad = Exepfad + "IPserver.dat";

            xxx = true;

            if (File.Exists(Pfad))
            {
                StreamReader sr = new StreamReader(Pfad);

                string ip = sr.ReadLine();


                sr.Close();

                DistributionServerIp = ip;
                GetFinished = true;
            }

            OwnServerIP = "2a02:908:950:e6e0:b556:f9ac:1927:2ec7:2000";

            if (ServerTest == true)
            {
                DistributionServerIp = "127.0.0.1";
                ServerPort = 1000;
                OwnServerIP = "127.0.0.1:2000";
            }


            //Client1 = new SimpleTcpClient(DistributionServerIp);

            ClientInit(DistributionServerIp,ServerPort);

            GetFinished = true;
            GetPaused = false;

            wait = 1000;

            //GetNewMessageFromServer();


            //Server.Init(OwnServerIP);
            //Server.Start();

        }


        async public static void GetNewMessageFromServer()
        {           
            while (true) 
            {
                await Task.Delay(wait);

                if (GetFinished == true && GetPaused == false)
                {
                    GetFinished = false;
                    SetWait(2);
                    GetMessage();
                }
            }
        }

       


        public static void Trig()
        {
            xxx = false;
        }


            public static void Connect()
            {
                try
                {
                if (Client1.Connected == false)
                {
                    Client1.Connect();
                }
                   
                }
                catch (Exception e)
                {

                /* Application.Current.Dispatcher.Invoke(() =>
                 {
                     MessageBox.Show("\n Keine Verbindung \n");
                 });*/
   
             
                PopupBox.Show(e.Message);

            }
            }

            public static void Senden(string wort)
            {
                if (Client1.Connected == true)
                {
                    Client1.SendAsync(wort);
                }
                else
                {
                    //MessageBox.Show(" \n Nicht gesendet \n");
                }
            }

            static void ClientInit(string Ip, int Port)
            {
                //Client1 = new SimpleTcpClient("127.0.0.1:9000");

                Client1 = new WatsonTcpClient(Ip,Port);

                Client1.Events.ServerConnected += Events_Connected;
                Client1.Events.MessageReceived += Events_DataReceived;
                Client1.Events.ServerDisconnected += Events_Disconnected;
            }


            static private void Events_Disconnected(object? sender, DisconnectionEventArgs e)
            {

            }

            static private void Events_DataReceived(object? sender, MessageReceivedEventArgs e)
            {
                string Anfrage = "";
                Anfrage = Encoding.UTF8.GetString(e.Data);
                AnfrageErmitteln(Anfrage);

            }

            static private void Events_Connected(object? sender, ConnectionEventArgs e)
            {

            }


        


            



      




        static void AnfrageErmitteln(string Anfrage)
            {

                //MessageBox.Show(Anfrage);

                string zeile = "\n";
                int zeileanfang = 0;
                List<string> Nachricht = new List<string>();

                if (Anfrage[Anfrage.Length - 1] != zeile[0])
                {
                    Anfrage += zeile;
                }

                for (int i = 0; i < Anfrage.Length; i++)
                {

                    if (Anfrage[i] == zeile[0])
                    {
                        Nachricht.Add(Anfrage.Substring(zeileanfang, (i - zeileanfang)));
                        zeileanfang = i + 1;

                    }

                }




                



            if (Nachricht[0] == "GetMessage1414")
            {
                int sendercontactid = 0;
                for (int i = 0; i < ContactsData.ContactsModelList.Count; i++)
                {
                    if (Nachricht[1] == ContactsData.ContactsModelList[i].AccountNumber)
                    {
                        sendercontactid = ContactsData.ContactsModelList[i].Id;
                        break;
                    }
                }

                if (sendercontactid != 0)
                {

                    string sendermessage = "";

                    for (int i = 2; i < Nachricht.Count; i++)
                    {
                        sendermessage += Nachricht[i];
                    }


                    MessagesData.SaveMessagesToDB(sendermessage, 0, sendercontactid);
                    ContactsData.UpdateContactDB(sendermessage, sendercontactid);


                    MessageReceived.Invoke();
                    SetWait(0.25);
                    GetFinished=true;
                }
                
            }


            if (Nachricht[0] == "CreateAccount100")
            {               
                if (Nachricht[1] == ContactsData.CreateAccountNumber)
                {
                    if (Nachricht[2] == "Success")
                    {
                        ContactsData.CreateAccountSuccess();
                    }
                }
                            
            }


            if (Nachricht[0] == "Login101")
            {
                if (Nachricht[1] == ContactsData.LoginAccountNumber)
                {
                    
                    if (Nachricht[2] == "Success")
                    {
                        ContactsData.SetLoginTrue();
                        LoginChanged.Invoke();
                    }
                    else
                    {
                        ContactsData.SetLoginFalse();
                        LoginChanged.Invoke();
                    }
                }
            }


















        }


        public static void SendMessage(string MessageText)
        {
            Connect();
            Senden("SendMessage1313\n" + ContactsData.LoginAccountNumber+"\n"+MessagesData.CurrentAccountnumber+"\n"+ ContactsData.LoginPassword + "\n" + MessageText+"\n");  
        }


        public static void GetMessage()
        {
            Connect();
            Senden("GetMessage1313\n" + ContactsData.LoginAccountNumber + "\n" + ContactsData.LoginPassword + "\n");           
        }

        //"GetMessage1414\n" + MessageList[ReceiverAccountNumber][0].SenderAccountNumber+"\n"+ MessageList[ReceiverAccountNumber][0].TextContent + "\n"

        public static void CreateAccount(string AccountNumber,string Password)
        {         
            Connect();                       
            Senden("CreateAccount100\n" + AccountNumber + "\n" + Password + "\n");
        }

        public static void Login(string AccountNumber, string Password)
        {            
            Connect();                        
            Senden("Login101\n" + AccountNumber + "\n" + Password + "\n");
        }

        public static bool Verbunden()
            {
                return Client1.Connected;
            }



            static string Abschneiden(string wort)
            {
                string sl = @"\";
                for (int i = wort.Length - 1; i > 0; i--)
                {
                    if (wort[i] == sl[0])
                    {
                        return wort.Substring(0, i + 1);
                    }
                }

                return wort;
            }


        static string GetBinFolder(string wort)
        {
            string sl = @"\";
            string folder = "bin";

            for (int i = wort.Length - 1; i > 0; i--)
            {
                if (wort[i] == sl[0])
                {                    

                    if (wort.Substring(i-3,3) == folder)
                    {
                        return wort.Substring(0, i + 1);
                    }

                }
            }

            return wort;
        }





        }


















    }


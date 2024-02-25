using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Messenger.Model;

namespace Messenger.Data
{
    public static class ContactsData
    {

        public static List<ContactsModel> ContactsModelList;
        static BitmapImage Bitmap2;
        public static int ctc = 0;

        public static ContactsModel ContactsModelToAdd;
        public static bool Add,LoggedIn;

        public static int ConstructorCounter;

        public static string MyAccountNumber,Password;

        public static string CreateAccountNumber, CreatePassword;

        public static string LoginAccountNumber, LoginPassword;

        static ContactsData()
        {
            ContactsModelList = new List<ContactsModel>();

           
            Bitmap2 = new BitmapImage();

            Add = false;
            ContactsModelToAdd = new ContactsModel(5,"",Bitmap2,"","","");

            

            Bitmap2.BeginInit();
            Bitmap2.UriSource = new Uri(Client.BinFolder+"profile2.png");
            Bitmap2.EndInit();

            ContactsData.CreateDB();
            ContactsData.GetContactsFromDB();

            LoginAccountNumber = string.Empty;
            LoginPassword = string.Empty;
            CreateAccountNumber = string.Empty;
            CreatePassword = string.Empty;    

            LoggedIn = false;


            ReadSavedLogin();


            ConstructorCounter = 0;
            
        }






        public static void GetContactsFromDB()
        {
            string dbName = "Messenger.db";
            string connectionString = $"Data Source={dbName};Version=3;";

            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            // Daten abrufen
            string selectDataQuery = "SELECT * FROM Contacts;";
            SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection);

            SQLiteDataReader reader = command.ExecuteReader();

            int kk = 0;



            ContactsModelList = new List<ContactsModel>();

            DateTime Date1 ;

            while (reader.Read())
            {
               // MessageBox.Show($"Id: {reader["Contactsid"]}, Name: {reader["Name"]}");

                Date1 = Convert.ToDateTime( reader["LastMessageDate"] );
                             
                ContactsModelList.Add(new ContactsModel(Convert.ToInt32(reader["Contactsid"]), Convert.ToString(reader["Accountnumber"]), Bitmap2,
                    Convert.ToString(reader["Name"]), Convert.ToString(reader["LastMessagetext"]), Date1.ToShortDateString()) );                   //Convert.ToString(reader["LastMessageDate"])


                

            }

            

            connection.Close();
        }



        public static void UpdateContactDB(string sendermessage, int sendercontactid)
        {
            string dbName = "Messenger.db";
            string connectionString = $"Data Source={dbName};Version=3;";

            SQLiteConnection connection = new SQLiteConnection(connectionString);

            string Date1 = DateTime.Now.ToString("yyyy-MM-dd");

            connection.Open();

            string UpdateDataQuery = "update Contacts set LastMessagetext=@Message, LastMessageDate=@Date where Contactsid=@Id;";

            SQLiteCommand command = new SQLiteCommand(UpdateDataQuery, connection);           
            command.Parameters.AddWithValue("@Message", sendermessage);
            command.Parameters.AddWithValue("@Date", Date1);
            command.Parameters.AddWithValue("@Id", sendercontactid);

            command.ExecuteNonQuery();



            connection.Close();

            GetContactsFromDB();
        }


        public static void AddContact()
        {
            if (Add== true)
            {               

                string dbName = "Messenger.db";
                string connectionString = $"Data Source={dbName};Version=3;";

                SQLiteConnection connection = new SQLiteConnection(connectionString);

                connection.Open();

                string insertDataQuery = "INSERT INTO Contacts VALUES (null, @Accountnumber, @Name, 1,@Message,@Datum);";

                SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection);
                command.Parameters.AddWithValue("@Accountnumber", ContactsModelToAdd.AccountNumber);
                command.Parameters.AddWithValue("@Name", ContactsModelToAdd.Name);
                command.Parameters.AddWithValue("@Message", "Nichts");
                command.Parameters.AddWithValue("@Datum", "1990-01-01");

                command.ExecuteNonQuery();



                connection.Close();

                GetContactsFromDB();
            }

            Add = false;

        }

        public static void DeleteContact(int Id)
        {
            

                string dbName = "Messenger.db";
                string connectionString = $"Data Source={dbName};Version=3;";

                SQLiteConnection connection = new SQLiteConnection(connectionString);

                connection.Open();

            string DeleteDataQuery = "Delete from Contacts where Contactsid=@Id; ";
            string DeleteDataQuery2 = "Delete from Messages where fk_Contactsid=@Id; ";

            SQLiteCommand command = new SQLiteCommand(DeleteDataQuery, connection);
            command.Parameters.AddWithValue("@Id", Id);
            command.ExecuteNonQuery();

            SQLiteCommand command2 = new SQLiteCommand(DeleteDataQuery2, connection);
            command2.Parameters.AddWithValue("@Id", Id);
            command2.ExecuteNonQuery();

            connection.Close();

                GetContactsFromDB();
            

        }




        public static void CreateDB()
        {



            string dbName = "Messenger.db";
            string connectionString = $"Data Source={dbName};Version=3;";

            // Datenbank erstellen, wenn sie nicht existiert
            if (!System.IO.File.Exists(dbName))
            {
                SQLiteConnection.CreateFile(dbName);


                // Verbindung zur SQLite-Datenbank herstellen
                SQLiteConnection connection = new SQLiteConnection(connectionString);

                connection.Open();

                // Tabellen erstellen
                string createTableQuery = "CREATE TABLE IF NOT EXISTS Contacts (Contactsid INTEGER PRIMARY KEY ,Accountnumber Text, Name TEXT, fk_Imageid int,LastMessagetext Text, LastMessageDate  Date);";
                string createTableQuery2 = "CREATE TABLE IF NOT EXISTS Messages (Messagesid INTEGER PRIMARY KEY ,Messagetext Text, Self int, fk_Contactsid int);";




                SQLiteCommand command = new SQLiteCommand(createTableQuery, connection);

                command.ExecuteNonQuery();

                command.CommandText = createTableQuery2;
                command.ExecuteNonQuery();



                string inserttestcontacts = "insert into Contacts values (null,\"6457\",\"Test Test\",0,@Message,@Datum)";
                


                command.CommandText = inserttestcontacts;
                command.Parameters.AddWithValue("@Message", "nichts");
                command.Parameters.AddWithValue("@Datum", "2000-01-01");
                command.ExecuteNonQuery();

                

                string inserttestmessage = "insert into Messages values (null,\"Hallo\",0,1)";

                command.CommandText = inserttestmessage;
                command.ExecuteNonQuery();

                connection.Close();


            }


        }


        public static void ReadSavedLogin()
        {
            string Pfad = Client.Exepfad + "/Account.dat";

            string LoginAccountNumber1 = "";
            string LoginPassword1 = "";

            if (File.Exists(Pfad))
            {
                StreamReader sr = new StreamReader(Pfad);
                LoginAccountNumber1 = sr.ReadLine();
                LoginPassword1 = sr.ReadLine();
                sr.Close();

                TryLogin(LoginAccountNumber1 , LoginPassword1);    

            }           
            
        }


        public static void SaveLogin(string AccountNumber, string Password)
        {

            string Pfad = Client.Exepfad + "/Account.dat";
           

            if (File.Exists(Pfad))
            {
                StreamWriter StreamWriter1 = new StreamWriter(Pfad);
                StreamWriter1.WriteLine(AccountNumber);
                StreamWriter1.WriteLine(Password);
                StreamWriter1.Close();
               // MessageBox.Show("hier");
            }
            else
            {
                File.Create(Pfad).Close();

                

                StreamWriter StreamWriter1 = new StreamWriter(Pfad);
                StreamWriter1.WriteLine(AccountNumber);
                StreamWriter1.WriteLine(Password);
                StreamWriter1.Close();

            }
            
        }

        public static void CreateAccount(string accountnumber , string password)
        {
            CreateAccountNumber = accountnumber;
            CreatePassword= password;

            Client.CreateAccount(accountnumber, password);
        }

        public static void CreateAccountSuccess()
        {
            SaveLogin(CreateAccountNumber, CreatePassword); 
            TryLogin(CreateAccountNumber, CreatePassword);
        }


        public static void TryLogin(string accountnumber, string password)
        {
            LoginAccountNumber = accountnumber;
            LoginPassword = password;

            Client.Login(accountnumber, password);
        }


        public static void SetLoginTrue()
        {
            LoggedIn = true;
            SaveLogin(LoginAccountNumber, LoginPassword);
            
        }

        public static void SetLoginFalse()
        {
            LoggedIn = false;
        }
















    }
}

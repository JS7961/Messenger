using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows;
using Messenger.Model;

namespace Messenger.Data
{
    public static class MessagesData
    {

      public static int CurrentContactId, LastContactId;

        public static List<MessagesModel> MessagesModelList;

        public static bool MessagesChanged,AccountOnline;

        public static string CurrentAccountnumber;

        static MessagesData()
        {
            CurrentContactId = 0;
            LastContactId = 0;

            MessagesModelList = new List<MessagesModel>();

            MessagesChanged = false;

            //MessageBox.Show("con");

            AccountOnline = false;  

            //ShowDB();

        }


        

        

        




        public static void GetMessagesFromDB()
        {
            string dbName = "Messenger.db";
            string connectionString = $"Data Source={dbName};Version=3;";

            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();
            
            // Daten abrufen
            string selectDataQuery = "SELECT * FROM Messages where fk_Contactsid="+ CurrentContactId.ToString() +";";
            SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection);

            SQLiteDataReader reader = command.ExecuteReader();

            MessagesModelList.Clear();

            while (reader.Read())
            {
               
                MessagesModelList.Add(new MessagesModel( Convert.ToInt32(reader["Messagesid"]), Convert.ToString(reader["Messagetext"]), Convert.ToInt32(reader["Self"]))   );
            }

            connection.Close();
        }

        //Messagesid INTEGER PRIMARY KEY ,Messagetext Text, Self int, fk_Contactsid int


        public static void SaveMessagesToDB(string Messagetext , int Self , int SaveContactId )
        {
            string dbName = "Messenger.db";
            string connectionString = $"Data Source={dbName};Version=3;";        //SQLITE_THREADSAFE=2

            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            string insertDataQuery = "INSERT INTO Messages VALUES (null, @MessageText, @Self, @CurrentContactId);";

            SQLiteCommand command = new SQLiteCommand(insertDataQuery, connection);
            command.Parameters.AddWithValue("@MessageText", Messagetext);
            command.Parameters.AddWithValue("@Self", Self);
            command.Parameters.AddWithValue("@CurrentContactId", SaveContactId);
         
            command.ExecuteNonQuery();

            

            connection.Close();
        }




        public static void ShowDB()
        {
            string dbName = "Messenger.db";
            string connectionString = $"Data Source={dbName};Version=3;";

            SQLiteConnection connection = new SQLiteConnection(connectionString);

            connection.Open();

            // Daten abrufen
            string selectDataQuery = "SELECT * FROM Contacts;";
           SQLiteCommand command = new SQLiteCommand(selectDataQuery, connection);

            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                MessageBox.Show($"Id: {reader["Contactsid"]}, Name: {reader["Name"]}");
            }

            connection.Close();

        }






        





    }
}

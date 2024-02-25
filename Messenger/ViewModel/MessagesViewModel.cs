using Messenger.Data;
using Messenger.Model;
using Messenger.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Messenger.ViewModel
{
    internal class MessagesViewModel : ViewModelBase
    {

        int DisplayedContactIndex;


        public ObservableCollection<MessagesModel> MessagesModelList { get; set; }


        public MessagesViewModel()
        {

             for (int i = 0; i < ContactsData.ContactsModelList.Count; i++)
             {
                 if (MessagesData.CurrentContactId == ContactsData.ContactsModelList[i].Id)
                 {
                    DisplayedContactIndex = i;
                    break;
                 }
             }

            //ContactsData.ContactsModelList.Find(x => x.Id == MessagesData.CurrentContactId);

            //DisplayedContactIndex = MessagesData.CurrentContactId - 1;

            MessagesData.CurrentAccountnumber = ContactsData.ContactsModelList[DisplayedContactIndex].AccountNumber;

            Name = ContactsData.ContactsModelList[DisplayedContactIndex].Name;
            Image = ContactsData.ContactsModelList[DisplayedContactIndex].ContactImage;


            //MessagesData.GetMessages();

            MessagesData.GetMessagesFromDB();

            MessagesModelList = new ObservableCollection<MessagesModel>();

            for (int i = 0; i < MessagesData.MessagesModelList.Count; i++)
            {
                MessagesModelList.Add( MessagesData.MessagesModelList[i]);
            }


            NewMessage = string.Empty;

            ContactsData.ConstructorCounter++;


            //RefreshMessagesLoop();

            Client.MessageReceived += RefreshMessages;
            //RefreshMessagesLoop();
            //SaveLoop();

            

            

            

        }


        public void RefreshMessages()
        {                        
                 MessagesData.GetMessagesFromDB();
                 RefreshMessagesModelList();                             
        }


        async void RefreshMessagesLoop()
        {
            while (true)
            {
                await Task.Delay(6000);

                if (MessagesData.MessagesChanged == true )
                {
                    MessagesData.GetMessagesFromDB();
                    RefreshMessagesModelList();
                    MessagesData.MessagesChanged = false;   
                }

               /* MessagesData.GetMessagesFromDB();
                RefreshMessagesModelList();
                MessagesData.MessagesChanged = false;*/

            }
        }



        



        void RefreshMessagesModelList2()
        {
            MessagesModelList.Clear();

            for (int i = 0; i < MessagesData.MessagesModelList.Count; i++)
            {
                MessagesModelList.Add(MessagesData.MessagesModelList[i]);
            }
        }

        void RefreshMessagesModelList()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessagesModelList.Clear();

                for (int i = 0; i < MessagesData.MessagesModelList.Count; i++)
                {
                    MessagesModelList.Add(MessagesData.MessagesModelList[i]);
                }
            });
        }






        private BitmapImage image;

        public BitmapImage Image
        {
            get { return image; }
            set 
            { 
                image = value;
                OnPropertyChanged("Image");
            }
        }




        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }


        private string newmessage;

        public string NewMessage
        {
            get { return newmessage; }
            set
            {
                newmessage = value;
                OnPropertyChanged("NewMessage");
            }
        }




        public RelayCommand SendMessageCommand
        {
            get
            {
                return new RelayCommand(SendMessage, canExecute => CanSend());
            }
        }



        public void SendMessage(object parameter)
        {
            

            

            MessagesData.MessagesModelList.Add(new MessagesModel(5, NewMessage, 1));
            Client.SendMessage(NewMessage);
            MessagesData.SaveMessagesToDB(NewMessage, 1, MessagesData.CurrentContactId);
            MessagesData.MessagesChanged = true;

            NewMessage = string.Empty;
            RefreshMessagesModelList();
            
        }

        private bool CanSend()
        {
            return NewMessage != string.Empty ;
        }


        //kontakt 100


        public RelayCommand GetCommand => new RelayCommand(para => Client.GetMessage(), canExecute => true);





    }
}

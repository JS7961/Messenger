using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Messenger.Data;
using Messenger.Model;
using Messenger.MVVM;
using Messenger.View;

namespace Messenger.ViewModel
{
    internal class ContactsViewModel : ViewModelBase
    {


        

        public ObservableCollection<ContactsModel> ContactsModelList { get; set; }


        public ContactsViewModel()
        {
            

            ContactsModelList = new ObservableCollection<ContactsModel>();

            for (int i = 0; i < ContactsData.ContactsModelList.Count; i++)
            {
                ContactsModelList.Add(ContactsData.ContactsModelList[i]);
            }

            //MessageBox.Show(DateTime.Now.ToString("yyyy-MM-dd"));


            Client.MessageReceived += RefreshContactListForeign;
            Client.LoginChanged += ShowLoginStatus;

            

            ShowLoginStatus();

        }



        void RefreshContactListForeign()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ContactsModelList.Clear();

                for (int i = 0; i < ContactsData.ContactsModelList.Count; i++)
                {
                    ContactsModelList.Add(ContactsData.ContactsModelList[i]);
                }
            });
        }




        void RefreshContactsList()
        {
            ContactsModelList.Clear();

            for (int i = 0; i < ContactsData.ContactsModelList.Count; i++)
            {
                ContactsModelList.Add(ContactsData.ContactsModelList[i]);
            }
        }

        void ShowLoginStatus()
        {           
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (ContactsData.LoggedIn == true)
                {
                    LoginText = "Logged In";
                }
                else 
                {
                    LoginText = "Logged Out";
                }
            });
        }


        private string loginText;

        public string LoginText
        {
            get { return loginText; }
            set 
            { 
                loginText = value; 
                OnPropertyChanged(nameof(LoginText));
            }
        }







        public RelayCommand AddContactCommand
        {
            get
            {
                return new RelayCommand(AddContact, canExecute => CanShow2());
            }
        }


        void AddContact(object parameter)
        {
            AddContactWindow AddContactWindow1 = new AddContactWindow();

            AddContactWindow1.ShowDialog();



            RefreshContactsList();
        }


        private bool CanShow2()
        {
            return true;
        }



        public RelayCommand CreateAccountCommand
        {
            get
            {
                return new RelayCommand(CreateAccount, canExecute => CanShow3());
            }
        }


        void CreateAccount(object parameter)
        {
            CreateAccountWindow CreateAccountWindow1 = new CreateAccountWindow();

            CreateAccountWindow1.ShowDialog();

          
        }


        private bool CanShow3()
        {
            return true;
        }


        public RelayCommand DeleteContactCommand
        {
            get
            {
                return new RelayCommand(DeleteContact, canExecute => CanShow4());
            }
        }


        void DeleteContact(object parameter)
        {
            int Id=Convert.ToInt32(parameter);

            ConfirmWindow ConfirmWindow1 = new ConfirmWindow();

            ConfirmWindow1.ShowDialog();

            if (ConfirmWindow1.Confimed == true)
            {
                ContactsData.DeleteContact(Id);
                RefreshContactsList();
            }
           

        }


        private bool CanShow4()
        {
            return true;
        }



        public RelayCommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login, canExecute => CanShow5());
            }
        }


        void Login(object parameter)
        {
            

            LoginWindow LoginWindow1 = new LoginWindow();

            LoginWindow1.ShowDialog();

            if (LoginWindow1.Login == true)
            {
                ContactsData.TryLogin(LoginWindow1.TB_AccountNumber.Text, LoginWindow1.TB_Password.Text);
            }


        }


        private bool CanShow5()
        {
            return true;
        }



















    }
}

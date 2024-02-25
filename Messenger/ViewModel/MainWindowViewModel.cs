using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Messenger.Data;
using Messenger.MVVM;

namespace Messenger.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {


        public MainWindowViewModel()
        {
                SelectedViewModel = new ContactsViewModel();
        }



        public RelayCommand ShowContactsCommand
        {
            get
            {
                return new RelayCommand(ShowContacts, canExecute => CanShow());
            }
        }



        public void ShowContacts(object parameter)
        {
            SelectedViewModel= new ContactsViewModel();
        }

        private bool CanShow()
        {
            return true;
        }


        public RelayCommand ShowMessagesCommand
        {
            get
            {
                return new RelayCommand(ShowMessages, canExecute => CanShow2());
            }
        }



        public void ShowMessages(object parameter)
        {
            MessagesData.CurrentContactId = Convert.ToInt32( parameter);
            SelectedViewModel= new MessagesViewModel();
        }

        private bool CanShow2()
        {
            return true;
        }





        private ViewModelBase selectedViewModel;

        public ViewModelBase SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                selectedViewModel = value;
                OnPropertyChanged("SelectedViewModel");
            }
        }



    }
}

using Messenger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Messenger.View
{
    /// <summary>
    /// Interaktionslogik für AddContactWindow.xaml
    /// </summary>
    public partial class AddContactWindow : Window
    {
        public AddContactWindow()
        {
            InitializeComponent();
        }

        private void BTN_Add_Click(object sender, RoutedEventArgs e)
        {
            ContactsData.Add = true;

            ContactsData.ContactsModelToAdd.Name = TB_Name.Text;
            ContactsData.ContactsModelToAdd.AccountNumber= TB_Account.Text;

            ContactsData.AddContact();

            Close();
        }
    }
}

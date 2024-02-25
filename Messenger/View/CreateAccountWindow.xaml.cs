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
    /// Interaktionslogik für CreateAccountWindow.xaml
    /// </summary>
    public partial class CreateAccountWindow : Window
    {
        public CreateAccountWindow()
        {
            InitializeComponent();
            TBL_LoginAccountNumber.Text = ContactsData.LoginAccountNumber;
        }

        private void BT_Create_Click(object sender, RoutedEventArgs e)
        {
            ContactsData.CreateAccount(TB_AccountNumber.Text,TB_Password.Text);
            Close();
        }
    }
}

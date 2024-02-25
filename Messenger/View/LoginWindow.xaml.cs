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
    /// Interaktionslogik für LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public bool Login;

        public LoginWindow()
        {
            InitializeComponent();
            Login = false;   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TB_AccountNumber.Text != "" && TB_Password.Text != "")
            {
                Login = true;
            }

            Close();
        }
    }
}

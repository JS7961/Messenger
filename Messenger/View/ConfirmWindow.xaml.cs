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
    /// Interaktionslogik für ConfirmWindow.xaml
    /// </summary>
    public partial class ConfirmWindow : Window
    {

        public bool Confimed;

        public ConfirmWindow()
        {
            InitializeComponent();
            Confimed = false;
        }

        private void BT_Yes_Click(object sender, RoutedEventArgs e)
        {
            Confimed=true;
            Close();
        }

        private void BT_No_Click(object sender, RoutedEventArgs e)
        {
            Confimed=false; 
            Close();
        }
    }
}

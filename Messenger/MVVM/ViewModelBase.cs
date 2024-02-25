using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messenger.MVVM
{
    internal class ViewModelBase : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler? PropertyChanged;


        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messenger.Model
{



    public class MessagesModel
    {


        public MessagesModel(int id, string text, int self)
        {
            Id = id;
            Text = text;    
            Self = self;

            if (self == 0) 
            {
                SelfVisibility = Visibility.Collapsed;
                OtherVisibility = Visibility.Visible;
                
            }
            else 
            {
                SelfVisibility = Visibility.Visible;
                OtherVisibility =Visibility.Collapsed;
            }
        }

        public int Id { get; set; } 

        public string Text { get; set; }

        public int Self { get; set; }       // 0 Kontakt      1 Eigene


        private Visibility selfvisibility;

        public Visibility SelfVisibility
        {
            get { return selfvisibility; }
            set { selfvisibility = value; }
        }

        private Visibility othervisibility;

        public Visibility OtherVisibility
        {
            get { return othervisibility; }
            set 
            { 
                othervisibility = value;              
            }
        }





    }



}

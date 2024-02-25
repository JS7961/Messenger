using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Messenger.Model
{


    public class ContactsModel
    {

        public ContactsModel(int id, string accountnumber, BitmapImage contactimage, string name, string lastmessage, string lastmessagedate)
        {
            this.ContactImage = contactimage;
            this.Name = name;
            this.LastMessage = lastmessage;
            this.LastMessageDate = lastmessagedate;
            this.Id = id;
            this.AccountNumber = accountnumber;
        }




        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public string Name { get; set; }

        public BitmapImage ContactImage { get; set; }

        public string LastMessage { get; set; }

        public string LastMessageDate { get; set; }

        










    }





}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Message
    {
        public Message()
        {
        }

        public Message(int? iDUser, int? iDFriend, string messageValue, DateTime messageDate)
        {
            IDUser = iDUser;
            IDFriend = iDFriend;
            MessageValue = messageValue;
            MessageDate = messageDate;
        }

        public Message(Message m)
        {
            IDUser = m.IDUser;
            IDFriend = m.IDFriend;
            MessageValue = m.MessageValue;
            MessageDate = m.MessageDate;
        }

        public int? IDUser { get; set; }

        public int? IDFriend { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Invalid message length")]
        public string MessageValue { get; set; }

        public DateTime MessageDate { get; set; }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}

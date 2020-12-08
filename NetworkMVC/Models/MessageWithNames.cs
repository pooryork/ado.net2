using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetworkMVC.Models
{
    public class MessageWithNames : Message
    {
        public MessageWithNames()
        {
        }

        public MessageWithNames(int? iDUser, int? iDFriend, string messageValue, DateTime messageDate) : base(iDUser, iDFriend, messageValue, messageDate)
        {
        }
        public MessageWithNames(Message m, string to, string from) : base(m)
        {
            To = to;
            From = from;
        }

        public string To { get; set; }
        public string From { get; set; }

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
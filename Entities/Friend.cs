using System;

namespace Entities
{
    public class Friend : UserSearch
    {
        public Friend()
        {
        }

        public Friend(DateTime termOfFriend)
        {
            TermOfFriend = termOfFriend;
        }

        public Friend(int? iDUser, string name, string surname, string patronymic, bool gender, string phoneNumber, int yearOfBirth, string town, DateTime termOfFriend) : base(iDUser, name, surname, patronymic, gender, phoneNumber, yearOfBirth, town)
        {
            TermOfFriend = termOfFriend;
        }

        public DateTime TermOfFriend { get; set; }

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
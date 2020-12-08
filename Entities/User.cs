using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Entities
{
    public class User : UserSearch
    {
        public User()
        {
        }

        public User(int? iDUser, string name, string surname, string patronymic, bool gender, string phoneNumber, int yearOfBirth, string town) : base(iDUser, name, surname, patronymic, gender, phoneNumber, yearOfBirth, town)
        {
        }

        public User(int? iDUser, string name, string surname, string patronymic, bool gender, string phoneNumber, int yearOfBirth, string town, string login, string password) : base(iDUser, name, surname, patronymic, gender, phoneNumber, yearOfBirth, town)
        {
            Login = login;
            Password = password;
        }

        [Required]
        [StringLength(35, MinimumLength = 2, ErrorMessage = "Invalid lastname length")]
        public string Login { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

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


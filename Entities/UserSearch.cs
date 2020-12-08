using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UserSearch
    {
        public UserSearch()
        {
        }

        public UserSearch(int? iDUser, string name, string surname, string patronymic, bool gender, string phoneNumber, int yearOfBirth, string town)
        {
            IDUser = iDUser;
            Name = name;
            Surname = surname;
            Patronymic = patronymic;
            Gender = gender;
            PhoneNumber = phoneNumber;
            YearOfBirth = yearOfBirth;
            Town = town;
        }

        public int? IDUser { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Invalid name length")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Invalid surname length")]
        public string Surname { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Invalid patronymic length")]
        public string Patronymic { get; set; }

        public bool? Gender { get; set; }

        [RegularExpression(@"^\+[2-9]\d{3}\d{3}\d{4}$", ErrorMessage = "Number Format +xxxxxxxxxxx")]
        public String PhoneNumber { get; set; }

        [RangeUntilCurrentYear(1900, ErrorMessage = "Invalid year: min = 1900")]
        public int? YearOfBirth { get; set; }

        [StringLength(35, MinimumLength = 2, ErrorMessage = "Invalid lastname length")]
        public string Town { get; set; }

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

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RangeUntilCurrentYearAttribute : RangeAttribute
    {
        public RangeUntilCurrentYearAttribute(int minimum) : base(minimum, DateTime.Now.Year)
        {
        }
    }
}
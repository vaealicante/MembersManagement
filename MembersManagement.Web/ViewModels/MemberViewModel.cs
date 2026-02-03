using System;

namespace MembersManagement.Web.ViewModels
{
    public class MemberViewModel
    {
        public int MemberID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }

        // --- ADDED AGE  ---
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public string Address { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string ContactNo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // --- ADDED CREATED DATE ---
        public DateTime CreatedDate { get; set; }
    }
}
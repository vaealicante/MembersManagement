using System;

namespace MembersManagement.Web.ViewModels
{
    public class MemberViewModel
    {
        public int MemberID { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public DateTime? BirthDate { get; set; }

        public int Age
        {
            get
            {
                // Update Age calculation to handle null
                if (!BirthDate.HasValue) return 0;

                var today = DateTime.Today;
                var age = today.Year - BirthDate.Value.Year;
                if (BirthDate.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public string? Address { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public string? Branch { get; set; } = string.Empty;
        public string? ContactNo { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // --- ADDED CREATED DATE ---
        public DateTime CreatedDate { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace MembersManagement.Web.VMC.ViewModels
{
    public class MemberViewModel
    {
        public int MemberID { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Birthdate is required")]
        public DateOnly BirthDate { get; set; }

        public string? Address { get; set; }
        public string? Branch { get; set; }
        public string? ContactNo { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace MembersManagement.Web.ViewModels
{
    public class MembershipViewModel
    {
        public int MembershipId { get; set; }

        [Required(ErrorMessage = "Membership Name is required")]
        [StringLength(100, ErrorMessage = "Membership Name cannot exceed 100 characters")]
        public string MembershipName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime DateCreated { get; set; }
    }
}


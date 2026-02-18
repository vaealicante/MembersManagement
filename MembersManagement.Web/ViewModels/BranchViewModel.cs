using System.ComponentModel.DataAnnotations;

namespace MembersManagement.Web.ViewModels
{
    public class BranchViewModel
    {
        public int BranchId { get; set; }

        [Required(ErrorMessage = "Branch Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Branch Name")]
        public string BranchName { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Location cannot exceed 150 characters")]
        public string? Location { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        // Optional: Count of members in this branch for the Index view
        [Display(Name = "Total Members")]
        public int MemberCount { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace MembersManagement.Domain.DomBranchModule.BranchEntities
{
    public class Branch
    {
        public int BranchId { get; set; }

        // Branch Data
        public string BranchName { get; set; } = null!;
        public string? Location { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
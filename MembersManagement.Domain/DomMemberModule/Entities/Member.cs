using MembersManagement.Domain.DomBranchModule.BranchEntities;
using System.ComponentModel.DataAnnotations;

namespace MembersManagement.Domain.DomMemberModule.Entities
{
    public class Member
    {
        public int MemberID { get; set; }

        // Core Member Data
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public string? Address { get; set; }
        public int? BranchId { get; set; }
        public Branch? Branch { get; set; }

        // Additional Fields
        public string? ContactNo { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

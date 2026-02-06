using System.ComponentModel.DataAnnotations;

namespace MembersManagement.Domain.Entities
{
    public class Member
    {
        public int MemberID { get; set; }

        // Core Member Data
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Branch { get; set; }

        // Additional Fields
        public string? ContactNo { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}

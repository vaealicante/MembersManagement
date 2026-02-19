using System.ComponentModel.DataAnnotations;

namespace MembersManagement.Domain.DomMembershipModule.MembershipEntities
{
    public class Membership
    {
        public int MembershipId { get; set; }

        // Membership Data
        public string MembershipName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
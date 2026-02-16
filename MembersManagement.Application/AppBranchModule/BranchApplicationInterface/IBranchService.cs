using MembersManagement.Domain.DomBranchModule.BranchEntities;
using System.Collections.Generic;

namespace MembersManagement.Application.AppBranchModule.BranchApplicationInterface
{
    public interface IBranchService
    {
        IEnumerable<Branch> GetBranch();
        Branch? GetBranch(int id);
        void CreateBranch(Branch branch);
        void UpdateBranch(Branch branch);
        void DeleteBranch(int id);
    }
}

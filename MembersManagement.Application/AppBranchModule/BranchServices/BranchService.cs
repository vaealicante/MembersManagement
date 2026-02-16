using System.Collections.Generic;
using MembersManagement.Application.AppBranchModule.BranchApplicationInterface;
using MembersManagement.Application.AppBranchModule.BranchBusinessLogic;
using MembersManagement.Domain.DomBranchModule.BranchEntities;


namespace MembersManagement.Application.AppBranchModule.BranchServices
{
    public class BranchService(BranchManager manager) : IBranchService
    {
        private readonly BranchManager _manager = manager;

        public IEnumerable<Branch> GetBranch() => _manager.GetBranch();

        public Branch? GetBranch(int id) => _manager.GetBranchById(id);

        public void CreateBranch(Branch branch) => _manager.CreateBranch(branch);

        public void UpdateBranch(Branch branch) => _manager.UpdateBranch(branch);

        public void DeleteBranch(int id) => _manager.DeleteBranch(id);
    }

}

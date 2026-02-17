using MembersManagement.Application.AppBranchModule.BranchApplicationInterface;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomBranchModule.BranchInterfaces; // Ensure this matches your repo namespace

namespace MembersManagement.Application.AppBranchModule.BranchServices
{
    public class BranchService : IBranchService
    {
        // 1. Declare the private field
        private readonly IBranchRepository _branchRepository;

        // 2. Inject the repository through the constructor
        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        // 3. Implement the method using the repository
        public IEnumerable<Branch> GetAllBranches()
        {
            // This now works because _branchRepository is defined above!
            return _branchRepository.GetAll(); 
        }

        public Branch? GetBranch(int id) => _branchRepository.GetById(id);

        public void CreateBranch(Branch branch) => _branchRepository.Add(branch);

        public void UpdateBranch(Branch branch) => _branchRepository.Update(branch);

        public void DeleteBranch(int id) => _branchRepository.Delete(id);
    }
}
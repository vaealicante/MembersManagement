using MembersManagement.Domain.DomBranchModule.BranchEntities;
using System.Collections.Generic;

namespace MembersManagement.Domain.DomBranchModule.Interfaces
{
    public interface IBranchRepository
    {
        IEnumerable<Branch> GetAll();
        Branch? GetById(int id);
        void Add(Branch branch);
        void Update(Branch branch);
        void Delete(int id);
        void SaveChanges();
    }
}

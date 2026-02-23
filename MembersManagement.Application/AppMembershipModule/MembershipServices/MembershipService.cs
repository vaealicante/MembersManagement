using FluentValidation;
using MembersManagement.Application.AppMembershipModule.MembershipApplicationInterface;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using MembersManagement.Domain.DomMembershipModule.MembershipInterface;

namespace MembersManagement.Application.AppMembershipModule.MembershipServices
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _repository;
        private readonly IValidator<Membership> _validator;

        public MembershipService(IMembershipRepository repository, IValidator<Membership> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public void CreateMembership(Membership membership)
        {
            _validator.ValidateAndThrow(membership);

            membership.IsActive = true;
            membership.DateCreated = DateTime.UtcNow;

            _repository.Add(membership);
            _repository.SaveChanges();   // ✅ REQUIRED
        }

        public void UpdateMembership(Membership membership)
        {
            _validator.ValidateAndThrow(membership);

            _repository.Update(membership);
            _repository.SaveChanges();   // ✅ REQUIRED
        }

        public void DeleteMembership(int id)
        {
            var membership = _repository.GetById(id)
                ?? throw new KeyNotFoundException("Membership not found");

            membership.IsActive = false; // soft delete
            _repository.Update(membership);
            _repository.SaveChanges();   // ✅ REQUIRED
        }

        public IEnumerable<Membership> GetAllMemberships()
            => _repository.GetAll();

        public Membership? GetMembership(int id)
            => _repository.GetById(id);
    }
}
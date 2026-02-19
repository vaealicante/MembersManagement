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

        // Inject the Repository and Validator via Constructor
        public MembershipService(IMembershipRepository repository, IValidator<Membership> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public void CreateMembership(Membership membership)
        {
            // 1. Validate the data
            var validationResult = _validator.Validate(membership);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // 2. Set defaults if needed
            membership.DateCreated = DateTime.Now;
            membership.IsActive = true;

            // 3. Save to Database
            _repository.Add(membership);
        }

        public IEnumerable<Membership> GetAllMemberships()
        {
            return _repository.GetAll();
        }

        public Membership? GetMembership(int id)
        {
            return _repository.GetById(id);
        }

        public void UpdateMembership(Membership membership)
        {
            var validationResult = _validator.Validate(membership);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            _repository.Update(membership);
        }

        public void DeleteMembership(int id)
        {
            _repository.Delete(id);
        }
    }
}
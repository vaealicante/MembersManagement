using FluentValidation;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomBranchModule.BranchInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MembersManagement.Application.AppBranchModule.BranchBusinessLogic
{
    // Using C# 12 Primary Constructor syntax
    public class BranchManager(
        IBranchRepository branchRepository,
        IValidator<Branch> validator)
    {
        private readonly IBranchRepository _branchRepository = branchRepository;
        private readonly IValidator<Branch> _validator = validator;

        /// <summary>
        /// Validates and saves a new branch. IsActive defaults to true.
        /// </summary>
        public void CreateBranch(Branch branch)
        {
            // Enforces the rules (Optional Birthdate/Email/Contact are handled here)
            _validator.ValidateAndThrow(branch);

            branch.IsActive = true;
            branch.DateCreated = DateTime.UtcNow;

            _branchRepository.Add(branch);
            _branchRepository.SaveChanges();
        }

        /// <summary>
        /// Returns only branchs where IsActive is true.
        /// </summary>
        public IEnumerable<Branch> GetBranch()
        {
            return _branchRepository
                .GetAll()
                .Where(m => m.IsActive);
        }

        public Branch? GetBranchById(int id)
        {
            return _branchRepository.GetById(id);
        }

        /// <summary>
        /// Validates changes and updates the branch record.
        /// </summary>
        public void UpdateBranch(Branch branch)
        {
            // Re-validates age range and format rules if data was changed
            _validator.ValidateAndThrow(branch);

            _branchRepository.Update(branch);
            _branchRepository.SaveChanges();
        }

        /// <summary>
        /// Performs a Soft Delete by switching the IsActive flag.
        /// </summary>
        public void DeleteBranch(int id)
        {
            var branch = _branchRepository.GetById(id)
                ?? throw new KeyNotFoundException($"Branch with ID {id} was not found.");

            branch.IsActive = false;

            _branchRepository.Update(branch);
            _branchRepository.SaveChanges();
        }

        /// <summary>
        /// Administrative method to see all records regardless of status.
        /// </summary>
        public IEnumerable<Branch> GetAllBranchesRaw()
        {
            return _branchRepository.GetAll();
        }
    }
}
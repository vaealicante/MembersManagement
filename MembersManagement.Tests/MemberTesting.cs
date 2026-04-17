using FluentValidation;
using MembersManagement.Application.AppMemberModule.BusinessLogic;
using MembersManagement.Application.AppMemberModule.Validators;
using MembersManagement.Domain.DomBranchModule.BranchEntities;
using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Domain.DomMemberModule.Interfaces;
using MembersManagement.Domain.DomMembershipModule.MembershipEntities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MemberManagement.Tests
{
    public class MemberManagerTests
    {
        private readonly Mock<IMemberRepository> _mockRepo;
        private readonly MemberValidation _validator;
        private readonly MemberManager _memberManager;
        private readonly Random _random = new Random();

        // Realistic first and last names
        private readonly string[] _firstNames = { "Alice", "Bob", "Charlie", "Diana", "Ethan", "Fiona", "George", "Hannah", "Ian", "Julia", "Kevin", "Laura" };
        private readonly string[] _lastNames = { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Wilson" };

        // Realistic branch locations
        private readonly string[] _locations = { "New York", "Los Angeles", "Chicago", "Houston", "Miami", "Dallas", "Atlanta", "Seattle", "Denver", "Boston" };

        public MemberManagerTests()
        {
            _mockRepo = new Mock<IMemberRepository>();
            _validator = new MemberValidation();
            _memberManager = new MemberManager(_mockRepo.Object, _validator);
        }

        // =========================
        // HELPER METHODS
        // =========================

        private string GetRandomFirstName() => _firstNames[_random.Next(_firstNames.Length)];
        private string GetRandomLastName() => _lastNames[_random.Next(_lastNames.Length)];
        private string GetRandomLocation() => _locations[_random.Next(_locations.Length)];

        private Member CreateTestMember(
            int id = 0,
            string? firstName = null,
            string? lastName = null,
            bool isActive = true,
            Branch? branch = null,
            Membership? membership = null)
        {
            return new Member
            {
                MemberID = id,
                FirstName = firstName ?? GetRandomFirstName(),
                LastName = lastName ?? GetRandomLastName(),
                IsActive = isActive,
                Branch = branch,
                Membership = membership
            };
        }

        private Branch CreateUniqueBranch(int id)
        {
            return new Branch
            {
                BranchId = id,
                BranchName = $"Branch_{Guid.NewGuid():N}",
                Location = GetRandomLocation(),
                IsActive = true,
                DateCreated = DateTime.UtcNow
            };
        }

        private Membership CreateUniqueMembership(int id)
        {
            return new Membership
            {
                MembershipId = id,
                MembershipName = $"Membership_{Guid.NewGuid():N}",
                IsActive = true,
                DateCreated = DateTime.UtcNow
            };
        }

        private List<Member> GenerateUniqueMembers(int count = 200, bool allowNulls = true)
        {
            var members = new List<Member>();
            for (int i = 1; i <= count; i++)
            {
                Branch? branch = allowNulls && i % 5 == 0 ? null : CreateUniqueBranch(i);
                Membership? membership = allowNulls && i % 7 == 0 ? null : CreateUniqueMembership(i);

                members.Add(CreateTestMember(
                    id: i,
                    branch: branch,
                    membership: membership,
                    isActive: _random.Next(0, 2) == 1
                ));
            }
            return members;
        }

        // =========================
        // CREATE TESTS
        // =========================

        [Fact]
        public void CreateMember_ShouldAssignMetadata_AndCallSave()
        {
            var member = CreateTestMember(firstName: "Alice", lastName: "Smith");

            _memberManager.CreateMember(member);

            Assert.True(member.IsActive);
            Assert.True(member.DateCreated > DateTime.UtcNow.AddMinutes(-1));
            _mockRepo.Verify(r => r.Add(member), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateMember_ShouldThrow_WhenValidationFails()
        {
            var member = CreateTestMember(firstName: "", lastName: "");

            Assert.Throws<ValidationException>(() => _memberManager.CreateMember(member));

            _mockRepo.Verify(r => r.Add(It.IsAny<Member>()), Times.Never);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Never);
        }

        [Fact]
        public void CreateMember_ShouldWorkWithUniqueBranchAndMembership()
        {
            var branch = CreateUniqueBranch(1);
            var membership = CreateUniqueMembership(1);

            var member = CreateTestMember(branch: branch, membership: membership);

            _memberManager.CreateMember(member);

            Assert.NotNull(member.Branch);
            Assert.NotNull(member.Membership);
            _mockRepo.Verify(r => r.Add(member), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Member_ShouldAllowNullMembershipAndBranch()
        {
            var member = CreateTestMember(branch: null, membership: null);

            _memberManager.CreateMember(member);

            Assert.Null(member.Branch);
            Assert.Null(member.Membership);
            _mockRepo.Verify(r => r.Add(member), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        // =========================
        // READ TESTS
        // =========================

        [Fact]
        public void GetMembers_ShouldOnlyReturnActiveRecords()
        {
            var members = new List<Member>
            {
                CreateTestMember(id: 1, isActive: true),
                CreateTestMember(id: 2, isActive: false),
                CreateTestMember(id: 3, isActive: true)
            };

            _mockRepo.Setup(r => r.GetAll()).Returns(members);

            var result = _memberManager.GetMembers().ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, m => Assert.True(m.IsActive));
        }

        [Fact]
        public void GetMembers_ShouldHandle200UniqueMembers()
        {
            var members = GenerateUniqueMembers(200, allowNulls: false);
            _mockRepo.Setup(r => r.GetAll()).Returns(members);

            var result = _memberManager.GetMembers().ToList();

            Assert.Equal(200, result.Count);
            // Ensure uniqueness of Branch and Membership names
            Assert.Equal(result.Where(m => m.Branch != null).Select(m => m.Branch!.BranchName).Distinct().Count(),
                         result.Count(m => m.Branch != null));
            Assert.Equal(result.Where(m => m.Membership != null).Select(m => m.Membership!.MembershipName).Distinct().Count(),
                         result.Count(m => m.Membership != null));
            // Check branch locations are valid
            Assert.All(result.Where(m => m.Branch != null), m => Assert.Contains(m.Branch!.Location, _locations));
        }

        // =========================
        // UPDATE TESTS
        // =========================

        [Fact]
        public void UpdateMember_ShouldUpdateMember_AndSave()
        {
            var existingMember = CreateTestMember(id: 1);
            _mockRepo.Setup(r => r.GetById(1)).Returns(existingMember);

            existingMember.FirstName = "Updated";

            _memberManager.UpdateMember(existingMember);

            _mockRepo.Verify(r => r.Update(existingMember), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateMember_ShouldThrow_WhenValidationFails()
        {
            var member = CreateTestMember(id: 1, firstName: "", lastName: "");

            Assert.Throws<ValidationException>(() => _memberManager.UpdateMember(member));

            _mockRepo.Verify(r => r.Update(It.IsAny<Member>()), Times.Never);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Never);
        }

        // =========================
        // DELETE TESTS
        // =========================

        [Fact]
        public void DeleteMember_ShouldSetIsActiveToFalse_WhenMemberExists()
        {
            var existingMember = CreateTestMember(id: 1);
            _mockRepo.Setup(r => r.GetById(1)).Returns(existingMember);

            _memberManager.DeleteMember(1);

            Assert.False(existingMember.IsActive);
            _mockRepo.Verify(r => r.Update(existingMember), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteMember_ShouldThrowKeyNotFound_WhenMemberDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetById(99)).Returns((Member?)null);

            var ex = Assert.Throws<KeyNotFoundException>(() => _memberManager.DeleteMember(99));
            Assert.Equal("Member with ID 99 was not found.", ex.Message);
        }
    }
}

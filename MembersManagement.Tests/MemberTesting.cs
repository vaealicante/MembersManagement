using Xunit;
using Moq;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using MembersManagement.Application.MemberModule.BusinessLogic;
using MembersManagement.Application.MemberModule.Validators;
using MembersManagement.Domain.MemberModule.Entities;
using MembersManagement.Domain.MemberModule.Interfaces;

namespace MemberManagement.Tests
{
    public class MemberManagerTests
    {
        private readonly Mock<IMemberRepository> _mockRepo;
        private readonly MemberValidation _realValidator;
        private readonly MemberManager _memberManager;

        public MemberManagerTests()
        {
            // Initialize the mocked repository and real validator
            _mockRepo = new Mock<IMemberRepository>();
            _realValidator = new MemberValidation();
            // Inject dependencies into MemberManager
            _memberManager = new MemberManager(_mockRepo.Object, _realValidator);
        }

        // ---------------- CREATE ----------------

        [Fact]
        public void CreateMember_ShouldAssignMetadata_AndCallSave()
        {
            // Test that creating a valid member:
            // 1. Sets IsActive to true
            // 2. Sets DateCreated to current time
            // 3. Calls Add() and SaveChanges() on repository
            var member = new Member
            {
                FirstName = "Alice",
                LastName = "Smith"
            };

            _memberManager.CreateMember(member);

            Assert.True(member.IsActive);
            Assert.True(member.DateCreated > DateTime.UtcNow.AddMinutes(-1));
            _mockRepo.Verify(r => r.Add(member), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateMember_ShouldThrow_WhenValidationFails()
        {
            // Test that creating a member with invalid data
            // (empty first name) throws ValidationException
            // and does not call repository methods
            var member = new Member { FirstName = "" };

            Assert.Throws<ValidationException>(() =>
                _memberManager.CreateMember(member));

            _mockRepo.Verify(r => r.Add(It.IsAny<Member>()), Times.Never);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Never);
        }

        // ---------------- READ ----------------

        [Fact]
        public void GetMembers_ShouldOnlyReturnActiveRecords()
        {
            // Test that GetMembers() returns only active members
            // from a mixed list (active and inactive)
            var members = new List<Member>
            {
                new Member { MemberID = 1, IsActive = true },
                new Member { MemberID = 2, IsActive = false },
                new Member { MemberID = 3, IsActive = true }
            };

            _mockRepo.Setup(r => r.GetAll()).Returns(members);

            var result = _memberManager.GetMembers().ToList();

            Assert.Equal(2, result.Count);
            Assert.All(result, m => Assert.True(m.IsActive));
        }

        // ---------------- UPDATE ----------------

        [Fact]
        public void UpdateMember_ShouldUpdateMember_AndSave()
        {
            // Test that updating an existing member:
            // 1. Applies changes to the member object
            // 2. Calls Update() and SaveChanges() on repository
            var existingMember = new Member
            {
                MemberID = 1,
                FirstName = "Old",
                LastName = "Name",
                IsActive = true
            };

            _mockRepo.Setup(r => r.GetById(1)).Returns(existingMember);

            existingMember.FirstName = "New";

            _memberManager.UpdateMember(existingMember);

            _mockRepo.Verify(r => r.Update(existingMember), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateMember_ShouldThrow_WhenValidationFails()
        {
            // Test that updating a member with invalid data
            // (empty first and last name) throws ValidationException
            // and does not call repository methods
            var member = new Member
            {
                MemberID = 1,
                FirstName = "",
                LastName = ""
            };

            Assert.Throws<ValidationException>(() =>
                _memberManager.UpdateMember(member));

            _mockRepo.Verify(r => r.Update(It.IsAny<Member>()), Times.Never);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Never);
        }

        // ---------------- DELETE ----------------

        [Fact]
        public void DeleteMember_ShouldSetIsActiveToFalse_WhenMemberExists()
        {
            // Test that deleting an existing member:
            // 1. Sets IsActive to false (soft delete)
            // 2. Calls Update() and SaveChanges() on repository
            var existingMember = new Member
            {
                MemberID = 1,
                IsActive = true
            };

            _mockRepo.Setup(r => r.GetById(1)).Returns(existingMember);

            _memberManager.DeleteMember(1);

            Assert.False(existingMember.IsActive);
            _mockRepo.Verify(r => r.Update(existingMember), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void DeleteMember_ShouldThrowKeyNotFound_WhenMemberDoesNotExist()
        {
            // Test that deleting a non-existent member
            // throws KeyNotFoundException with correct message
            _mockRepo.Setup(r => r.GetById(99)).Returns((Member?)null);

            var ex = Assert.Throws<KeyNotFoundException>(() =>
                _memberManager.DeleteMember(99));

            Assert.Equal("Member with ID 99 was not found.", ex.Message);
        }
    }
}

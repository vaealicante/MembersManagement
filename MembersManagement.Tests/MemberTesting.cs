using Xunit;
using Moq;
using FluentValidation;
using MembersManagement.Application.BusinessLogic;
using MembersManagement.Application.Validators;
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MemberManagement.Tests
{
    public class MemberTesting
    {
        private readonly Mock<IMemberRepository> _mockRepo;
        private readonly Mock<IValidator<Member>> _mockValidator;
        private readonly MemberManager _memberManager;
        private readonly MemberValidation _realValidator;

        public MemberTesting()
        {
            _mockRepo = new Mock<IMemberRepository>();
            _mockValidator = new Mock<IValidator<Member>>();
            _realValidator = new MemberValidation();

            // We inject the mock validator into the manager for behavior testing
            _memberManager = new MemberManager(_mockRepo.Object, _mockValidator.Object);
        }

        // --- CREATE TESTS ---

        [Fact]
        public void CreateMember_ShouldSucceed_WhenOptionalFieldsAreMissing()
        {
            // Arrange: Only required fields (FirstName, LastName) are provided
            var member = new Member
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = null, // Optional
                Email = null,      // Optional
                ContactNo = null   // Optional
            };

            // Act: Test against the real validation logic
            var result = _realValidator.Validate(member);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void CreateMember_ShouldFail_WhenMemberIsUnder18()
        {
            // Arrange: Member is 17 years old
            var member = new Member
            {
                FirstName = "Jane",
                LastName = "Doe",
                BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-17))
            };

            // Act
            var result = _realValidator.Validate(member);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Member must be at least 18 years old.");
        }

        // --- UPDATE TESTS ---

        [Fact]
        public void UpdateMember_ShouldFail_WhenEmailIsInvalid()
        {
            // Arrange: Providing an incorrectly formatted email
            var member = new Member
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "not-an-email"
            };

            // Act
            var result = _realValidator.Validate(member);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Invalid email format.");
        }

        [Fact]
        public void UpdateMember_ShouldFail_WhenBirthDateIsInFuture()
        {
            // Arrange
            var member = new Member
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };

            // Act
            var result = _realValidator.Validate(member);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Birthdate cannot be in the future.");
        }

        [Fact]
        public void UpdateMember_ShouldSucceed_WhenAgeIsExactly18()
        {
            // Arrange: Set birthdate to exactly 18 years ago today
            var member = new Member
            {
                FirstName = "Adult",
                LastName = "User",
                BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-18))
            };

            // Act
            var result = _realValidator.Validate(member);

            // Assert
            Assert.True(result.IsValid);
        }

        // --- READ TESTS ---

        [Fact]
        public void GetMembers_ShouldReturnOnlyActiveRecords()
        {
            // Arrange
            var memberList = new List<Member>
            {
                new Member { MemberID = 1, IsActive = true },
                new Member { MemberID = 2, IsActive = false }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(memberList);

            // Act
            var result = _memberManager.GetMembers();

            // Assert
            Assert.Single(result);
            Assert.True(result.First().IsActive);
        }

        // --- DELETE TESTS ---

        [Fact]
        public void DeleteMember_ShouldSetIsActiveToFalse_AndCallUpdate()
        {
            // Arrange
            var existingMember = new Member { MemberID = 10, IsActive = true };
            _mockRepo.Setup(r => r.GetById(10)).Returns(existingMember);

            // Act
            _memberManager.DeleteMember(10);

            // Assert
            Assert.False(existingMember.IsActive);
            _mockRepo.Verify(r => r.Update(existingMember), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        // --- BOUNDARY LOGIC (MAX AGE) ---

        [Fact]
        public void AgeValidation_ShouldFail_WhenMemberIsOlderThan65YearsAnd6Months()
        {
            // Arrange: 66 years ago
            var member = new Member
            {
                FirstName = "Senior",
                LastName = "Citizen",
                BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-66))
            };

            // Act
            var result = _realValidator.Validate(member);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorMessage == "Member cannot be older than 65 years, 6 months, and 1 day.");
        }
    }
}
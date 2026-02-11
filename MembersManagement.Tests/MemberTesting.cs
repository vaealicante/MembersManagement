using FluentValidation;
using MembersManagement.Application.BusinessLogic;
using MembersManagement.Application.Validators;
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

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

            _memberManager = new MemberManager(_mockRepo.Object, _mockValidator.Object);
        }

        // --- CREATE ---
        [Fact]
        public void CreateMember_ShouldCallRepositoryAddAndSave()
        {
            var newMember = new Member { FirstName = "John", LastName = "Doe" };

            _memberManager.CreateMember(newMember);

            _mockRepo.Verify(r => r.Add(It.IsAny<Member>()), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        // --- READ (Multiple) ---
        [Fact]
        public void GetMembers_ShouldReturnOnlyActiveRecords()
        {
            var memberList = new List<Member>
            {
                new Member { MemberID = 1, IsActive = true },
                new Member { MemberID = 2, IsActive = false }
            };
            _mockRepo.Setup(r => r.GetAll()).Returns(memberList);

            var result = _memberManager.GetMembers();

            Assert.Single(result); // Should only find 1 active member
            Assert.True(result.First().IsActive);
        }

        // --- READ (Single) ---
        [Fact]
        public void GetMemberById_ShouldReturnCorrectMember()
        {
            var member = new Member { MemberID = 5, FirstName = "Alice" };
            _mockRepo.Setup(r => r.GetById(5)).Returns(member);

            var result = _memberManager.GetMemberById(5);

            Assert.NotNull(result);
            Assert.Equal("Alice", result.FirstName);
        }

        // --- UPDATE ---
        [Fact]
        public void UpdateMember_ShouldCallRepositoryUpdateAndSave()
        {
            var existingMember = new Member { MemberID = 1, FirstName = "Old Name" };

            _memberManager.UpdateMember(existingMember);

            _mockRepo.Verify(r => r.Update(existingMember), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        // --- DELETE (Soft Delete) ---
        [Fact]
        public void DeleteMember_ShouldSetIsActiveToFalse()
        {
            var existingMember = new Member { MemberID = 10, IsActive = true };
            _mockRepo.Setup(r => r.GetById(10)).Returns(existingMember);

            _memberManager.DeleteMember(10);

            Assert.False(existingMember.IsActive);
            _mockRepo.Verify(r => r.Update(existingMember), Times.Once);
            _mockRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateMember_ShouldThrowException_WhenValidationFails()
        {
            // Arrange: Force the mock validator to throw an error
            var invalidMember = new Member();
            _mockValidator.Setup(v => v.Validate(invalidMember))
                .Returns(new FluentValidation.Results.ValidationResult(new[]
                    { new FluentValidation.Results.ValidationFailure("FirstName", "Required") }));

            // Act & Assert
            Assert.Throws<ValidationException>(() => _memberManager.CreateMember(invalidMember));
        }

        // --- VALIDATION (Age Range Theory) ---
        [Theory]
        [InlineData(-17)]
        [InlineData(-66)]
        public void AgeValidation_ShouldFail_IfOutsideAllowedRange(int yearsOffset)
        {
            var member = new Member
            {
                BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(yearsOffset))
            };

            var result = _realValidator.Validate(member);

            Assert.False(result.IsValid);
        }
    }
}
using MembersManagement.Domain.Entities;
using MembersManagement.Domain.Interfaces;
using MembersManagement.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MembersManagement.Infrastructure.RepositoryImplementation
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MemberDbContext _context;

        public MemberRepository(MemberDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Member> GetAll()
    => _context.Members.AsNoTracking().ToList();

        public Member? GetById(int id)
            => _context.Members.Find(id);

        public void Add(Member member)
            => _context.Members.Add(member);

        public void Update(Member member)
            => _context.Members.Update(member);

        public void Delete(int id)
        {
            var member = _context.Members.Find(id);
            if (member != null)
                _context.Members.Remove(member);
        }

        public void SaveChanges()
            => _context.SaveChanges();
    }
}

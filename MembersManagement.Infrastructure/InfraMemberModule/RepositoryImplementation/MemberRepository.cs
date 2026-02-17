using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Domain.DomMemberModule.Interfaces;
using MembersManagement.Infrastructure.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace MembersManagement.Infrastructure.InfraMemberModule.RepositoryImplementation
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public MemberRepository(ApplicationDbContext context)
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

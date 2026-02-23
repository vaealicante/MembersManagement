using MembersManagement.Domain.DomMemberModule.Entities;
using MembersManagement.Domain.DomMemberModule.Interfaces;
using MembersManagement.Infrastructure.AppDbContext; 
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MembersManagement.Infrastructure.InfraMemberModule.RepositoryImplementation
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MemberDbContext _context;

        public MemberRepository(MemberDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Member> GetAll()
            => _context.Members
                       .Include(m => m.Branch)
                       .Include(m => m.Membership)
                       .AsNoTracking()
                       .ToList();

        public Member? GetById(int id)
            => _context.Members
                       .Include(m => m.Branch)
                       .Include(m => m.Membership)
                       .FirstOrDefault(m => m.MemberID == id);

        public void Add(Member member)
        {
            _context.Members.Add(member);
            _context.SaveChanges();
        }

        public void Update(Member member)
        {
            var tracked = _context.Members.Local.FirstOrDefault(m => m.MemberID == member.MemberID);
            if (tracked != null)
                _context.Entry(tracked).CurrentValues.SetValues(member);
            else
                _context.Entry(member).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var member = _context.Members.Find(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                _context.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}

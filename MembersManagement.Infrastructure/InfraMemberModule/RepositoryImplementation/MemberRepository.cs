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
        private readonly ApplicationDbContext _context;

        public MemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Updated with .Include(m => m.Branch)
        public IEnumerable<Member> GetAll()
            => _context.Members
                       .Include(m => m.Branch) // <--- THIS LOADS THE RELATED BRANCH DATA
                       .AsNoTracking()
                       .ToList();

        public Member? GetById(int id)
            => _context.Members
                       .Include(m => m.Branch) // Also recommended here for detail views
                       .FirstOrDefault(m => m.MemberID == id);

        public void Add(Member member)
            => _context.Members.Add(member);

        public void Update(Member member)
        {
            // Check if the entity is already being tracked to avoid "already tracked" errors
            var trackedEntity = _context.Members.Local.FirstOrDefault(m => m.MemberID == member.MemberID);
            if (trackedEntity != null)
            {
                _context.Entry(trackedEntity).CurrentValues.SetValues(member);
            }
            else
            {
                _context.Entry(member).State = EntityState.Modified;
            }
        }
        public void Delete(int id)
        {
            var member = _context.Members.Find(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                // COMMIT THE CHANGE TO THE DATABASE
                _context.SaveChanges();
            }
        }
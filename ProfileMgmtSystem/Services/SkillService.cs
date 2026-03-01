using ProfileMgmtSystem.Data;
using ProfileMgmtSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ProfileMgmtSystem.Services
{
    public class SkillService
    {
        //make context first
        private readonly ProfileDbContext _context;

        //initialise constructor to inject the context
        public SkillService(ProfileDbContext context)
        {
            _context = context;
        }

        //create skill for a person
        public async Task<Skills> CreateAsync(int personId, string name, string category, ProficiencyLevel proficiency)
        {
            var skill = new Skills
            {
                PersonId = personId,
                Name = name,
                Category = category,
                Proficiency = proficiency
            };
            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return skill;
        }

        //read skills for a person
        //it gives whole person list
        public async Task<List<Skills>> GetPersonAsync(int personId) =>
            await _context.Skills.Where(s => s.PersonId == personId).ToListAsync();

        //get skill by id
        public async Task<Skills?> GetByIdAsync(int id) =>
            await _context.Skills.FindAsync(id);

        //update skill by id
        public async Task<bool> UpdateAsync(int id, string? name = null, string? category = null, ProficiencyLevel? proficiency = null)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return false;
            }
            if (name != null) skill.Name = name;
            if (category != null) skill.Category = category;
            if (proficiency != null) skill.Proficiency = proficiency.Value;
            await _context.SaveChangesAsync();
            return true;
        }

        //DELETE skill by id
        public async Task<bool> DeleteAsync(int id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return false;
            }
            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

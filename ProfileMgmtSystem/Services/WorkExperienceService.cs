using Microsoft.EntityFrameworkCore;
using ProfileMgmtSystem.Data;
using ProfileMgmtSystem.Models;

namespace ProfileMgmtSystem.Services
{
    public class WorkExperienceService
    {
        //make context first
        private readonly ProfileDbContext _context;

        //initialise constructor to inject the context
        public WorkExperienceService(ProfileDbContext context)
        {
            _context = context;
        }

        //create work expericen for a person
        public async Task<WorkExperience> CreateAsync(int personId, string companyName, string position, int startYear, int endYear)
        {
            var experience = new WorkExperience
            {
                PersonId = personId,
                CompanyName = companyName,
                Position = position,
                StartYear = startYear,
                EndYear = endYear
            };
            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();
            return experience;
        }

        //read work experience for a person
        public async Task<List<WorkExperience>> GetPersonAsync(int personId) =>
            await _context.Experiences.Where(w => w.PersonId == personId).ToListAsync();

        //get single work experience by id
        public async Task<WorkExperience?> GetByIdAsync(int id) =>
            await _context.Experiences.FindAsync(id);

        //update work experience by id
        public async Task<bool> UpdateAsync(int id, string? companyName = null, string? position = null, int? startYear = null, int? endYear = null)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return false;
            }
            if (companyName != null) experience.CompanyName = companyName;
            if (position != null) experience.Position = position;
            if (startYear != null) experience.StartYear = startYear.Value;
            if (endYear != null) experience.EndYear = endYear.Value;
            await _context.SaveChangesAsync();
            return true;
        }

        //delete workexperience by id
        public async Task<bool> DeleteAsync(int id)
        {
            var experience = await _context.Experiences.FindAsync(id);
            if (experience == null)
            {
                return false;
            }
            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

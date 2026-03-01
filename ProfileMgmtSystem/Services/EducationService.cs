using ProfileMgmtSystem.Data;
using ProfileMgmtSystem.Models;
using Microsoft.EntityFrameworkCore;
using ProfileMgmtSystem.Repositories;

namespace ProfileMgmtSystem.Services
{
    public class EducationService
    {
        //make context first
        //private readonly ProfileDbContext _context;

        ////constructor to inject the context
        //public EducationService(ProfileDbContext context)
        //{
        //    _context = context;
        //}

        //store the unit of work
        private readonly IUnitOfWork _unitOfWork;

        //inject the unit of work into the service through the constructor
        public EducationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //create education for a person
        public async Task<Education> CreateAsync(int personId, string institutionName, string degree, string fieldOfStudy, int startYear, int endYear)
        {
            var education = new Education
            {
                PersonId = personId,
                InstitutionName = institutionName,
                Degree = degree,
                FieldOfStudy = fieldOfStudy,
                StartYear = startYear,
                EndYear = endYear
            };
            //_context.Educations.Add(education);
            //await _context.SaveChangesAsync();

            await _unitOfWork.Educations.AddAsync(education);
            await _unitOfWork.SaveAsync();
            return education;
        }

        //read education by id
        public async Task<List<Education>> GetPersonAsync(int personId) =>
            //await _context.Educations.Where(e => e.PersonId == personId).OrderByDescending(e => e.StartYear).ToListAsync();
            await _unitOfWork.Educations.GetWithIncludesAsync(
                e => e.PersonId == personId,
                e => e.Person
            ) is Education education ? new List<Education> { education } : new List<Education>();
        //get education by id

        public async Task<Education?> GetByIdAsync(int id) =>
             //await _context.Educations.FindAsync(id);
                await _unitOfWork.Educations.GetByIdAsync(id);

        //update education by id
        public async Task<bool> UpdateAsync(int id, string? institutionName = null, string? degree = null, string? fieldOfStudy = null, int? startYear = null, int? endYear = null)
        {
            //var education = await _context.Educations.FindAsync(id);
            var education = await _unitOfWork.Educations.GetByIdAsync(id);
            if (education == null)
            {
                return false;
            }
            if (institutionName != null) education.InstitutionName = institutionName;
            if (degree != null) education.Degree = degree;
            if (fieldOfStudy != null) education.FieldOfStudy = fieldOfStudy;
            if (startYear != null) education.StartYear = startYear.Value;
            if (endYear != null) education.EndYear = endYear.Value;
            //await _context.SaveChangesAsync();
                _unitOfWork.Educations.Update(education);
                await _unitOfWork.SaveAsync();
            return true;
        }

        //DELETE education by id
        public async Task<bool> DeleteAsync(int id)
        {
            //var education = await _context.Educations.FindAsync(id);
            var education = await _unitOfWork.Educations.GetByIdAsync(id);
            if (education == null)
            {
                return false;
            }
            //_context.Educations.Remove(education); //this will only delete the education record, not the person or other related data
            //await _context.SaveChangesAsync();
                _unitOfWork.Educations.Delete(education);
                await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
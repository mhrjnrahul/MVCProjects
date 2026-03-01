using Microsoft.EntityFrameworkCore;
using ProfileMgmtSystem.Models;

namespace ProfileMgmtSystem.Data
{
    public class ProfileDbContext : DbContext
    {
        //dbsets represent tables in the database
        public DbSet<Person> Persons { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<WorkExperience> Experiences { get; set; }

        //constructor to configure the db context options
        public ProfileDbContext(DbContextOptions<ProfileDbContext> options) : base(options)
        {
        }

        //override the OnModelCreating method to configure the model relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //person -> education relationship
            //one to many relationship: one person can have many educations, but each education belongs to one person
            modelBuilder.Entity<Education>()
                .HasOne(p => p.Person)
                .WithMany(e => e.Educations)
                .HasForeignKey(e => e.PersonId)
                .OnDelete(DeleteBehavior.Cascade); //when a person is deleted, their educations will also be deleted

            //person -> skill relationship
            //one to many relationship: one person can have many skills, but each skill belongs to one person
            modelBuilder.Entity<Skills>()
                .HasOne(s => s.Person)
                .WithMany(p => p.Skills)
                .HasForeignKey(s => s.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            //person -> work experience relationship
            //one to many relationship: one person can have many work experiences, but each work experience belongs to one person
            modelBuilder.Entity<WorkExperience>()
                .HasOne(p => p.Person)
                .WithMany(w => w.Experiences)
                .HasForeignKey(w => w.PersonId)
                .OnDelete(DeleteBehavior.Cascade); //when a person is deleted, their work experiences will also be deleted

            //email must be unique for each person
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Email)
                .IsUnique();
        }
    }
}

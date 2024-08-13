using GallUni.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GallUni.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { 
        
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<EventImage> EventImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder) allows to map the model table in EF in your database
            base.OnModelCreating(modelBuilder);

            //Convertion Dateonly to DateTime ApplicationUser

            var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                dateOnly => dateOnly.ToDateTime(new System.TimeOnly(0, 0)),
                dateTime => DateOnly.FromDateTime(dateTime));
            modelBuilder.Entity<ApplicationUser>().Property(e => e.Birthday).HasConversion(dateOnlyConverter);

            //Convertion Dateonly to DateTime Event
            dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
                dateOnly => dateOnly.ToDateTime(new System.TimeOnly(0, 0)),
                dateTime => DateOnly.FromDateTime(dateTime));
            modelBuilder.Entity<Event>().Property(e => e.EventDate).HasConversion(dateOnlyConverter);

            //Populate model

            modelBuilder.Entity<Faculty>().HasData(
                new Faculty { Id = 1, FacultyName = "Faculty of Mathematics and Computer Science", FacultyDean = "" }
                );
        }
    }
}

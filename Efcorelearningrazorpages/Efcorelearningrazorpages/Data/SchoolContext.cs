using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Efcorelearningrazorpages.Models;

namespace Efcorelearningrazorpages.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext (DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Students { get; set; } = default!;
        public DbSet<Course> Courses { get; set; } = default!;
        public DbSet<Enrollment> Enrollments { get; set; } = default!;
        public DbSet<Department> Departments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        //you can use this to override ef convetions
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // this will run each time you will create new dbcontext object!
            // this is called fluant api since it has more than 1 call in 1 statment!
            //this is the convesion that ef core do take two table names and merge them 
            // this will run at each instance of dbcontext so ef core will know how to map whenever you want to get a table from this context!
            modelBuilder.Entity<Course>().ToTable("Course")
                .HasMany(c => c.Instructors).WithMany(i=>i.Courses);
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Instructor>().ToTable(nameof(Instructor));
            modelBuilder.Entity<Department>()
            .HasOne(d => d.Administrator)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<Department>().Property(d=>d.token).IsConcurrencyToken();
            modelBuilder.Entity<Department>().Property(d=>d.token)
                .IsConcurrencyToken().ValueGeneratedOnAddOrUpdate()
                .HasColumnType("rowversion");
        }
    }
}

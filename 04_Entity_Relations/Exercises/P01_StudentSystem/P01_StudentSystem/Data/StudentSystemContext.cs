using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data;

public class StudentSystemContext : DbContext
{
    public StudentSystemContext()
    {
    }

    public StudentSystemContext(DbContextOptions<StudentSystemContext> options)
        : base(options)
    {

    }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<StudentCourse> StudentsCourses { get; set; }

    public virtual DbSet<Resource> Resources { get; set; }

    public virtual DbSet<Homework> Homeworks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Config.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>(e =>
        {
            e.Property(s => s.Name)
            .HasMaxLength(100)
            .IsUnicode(true);

            e.Property(e => e.PhoneNumber)
            .HasMaxLength(10)
            .IsFixedLength(true);
        });

        modelBuilder.Entity<Course>(e =>
        {
            e.Property(c => c.Name)
            .HasMaxLength(80)
            .IsUnicode(true);

            e.Property(c => c.Description)
            .IsUnicode(true);
        });

        modelBuilder.Entity<StudentCourse>(e =>
        {
            e.HasKey(e => new { e.StudentId, e.CourseId });

            e.HasOne(e => e.Student)
           .WithMany(c => c.StudentsCourses)
           .HasForeignKey(e => e.StudentId);

            e.HasOne(e => e.Course)
            .WithMany(c => c.StudentsCourses)
            .HasForeignKey(e => e.CourseId);
        });


        modelBuilder.Entity<Resource>(e =>
        {
            e.Property(r => r.Name)
            .HasMaxLength(50)
            .IsUnicode(true);

            e.HasOne(e => e.Course)
            .WithMany(c => c.Resources)
            .HasForeignKey(e => e.CourseId);
        });

        modelBuilder.Entity<Homework>(e =>
        {
            e.HasOne(e => e.Student)
            .WithMany(s => s.Homeworks)
            .HasForeignKey(e => e.StudentId);

            e.HasOne(e => e.Course)
            .WithMany(s => s.Homeworks)
            .HasForeignKey(e => e.CourseId);
        });
    }
}
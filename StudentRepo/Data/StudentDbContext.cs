using Microsoft.EntityFrameworkCore;
using StudentRepo.Models;
using StudentRepo.models;

public class StudentDbContext : DbContext
{
    public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Courses> Courses { get; set; }
    public DbSet<AppUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Student>().HasKey(s => s.Id);
        modelBuilder.Entity<Courses>().HasKey(c => c.CourseId);
        modelBuilder.Entity<AppUser>().HasKey(u => u.Id);
        modelBuilder.Entity<AppUser>().Property(u => u.UserName).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<AppUser>().Property(u => u.PasswordHash).IsRequired().HasColumnName("Password");
        modelBuilder.Entity<AppUser>().Property(u => u.Role).IsRequired().HasMaxLength(20);
        modelBuilder.Entity<AppUser>().HasIndex(u => u.UserName).IsUnique();
        modelBuilder.Entity<AppUser>()
            .ToTable("Users", t => t.HasCheckConstraint("CK_Users_Role", "[Role] IN ('Admin','User')"));
    }
}

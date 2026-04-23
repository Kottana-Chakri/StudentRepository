// Migrate database and execute stored procedures
using Microsoft.EntityFrameworkCore;
public static class DbInitializer
{
    public static void Initialize(StudentDbContext context)
    {
        context.Database.Migrate();
        ExecuteStoredProcedures(context);
        SeedUsers(context);
    }

    private static void ExecuteStoredProcedures(StudentDbContext context)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "Sql");
        var directory = new DirectoryInfo(path);

        foreach (var file in directory.GetFiles("*.sql").OrderBy(f => f.Name))
        {
            var sql = File.ReadAllText(file.FullName);
            context.Database.ExecuteSqlRaw(sql);
        }
    }

    private static void SeedUsers(StudentDbContext context)
    {
        if (context.Users.Any())
        {
            return;
        }

        context.Users.AddRange(
            new StudentRepo.models.AppUser
            {
                UserName = "admin",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                Role = "Admin",
                IsActive = true
            },
            new StudentRepo.models.AppUser
            {
                UserName = "user",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
                Role = "User",
                IsActive = true
            }
            
        );

        context.SaveChanges();
    }
}

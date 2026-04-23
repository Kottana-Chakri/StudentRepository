namespace StudentRepo.models;

public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
    public bool IsActive { get; set; } = true;
}

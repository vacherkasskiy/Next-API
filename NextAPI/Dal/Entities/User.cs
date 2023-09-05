using System.ComponentModel.DataAnnotations;

namespace NextAPI.Dal.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string? Image { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? Website { get; set; }
    public string? City { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
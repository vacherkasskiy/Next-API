using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextAPI.Dal.Entities;

public class Post
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Author")]
    public int AuthorId { get; set; }
    
    [ForeignKey("Receiver")]
    public int ReceiverId { get; set; }

    public string Text { get; set; } = "";

    public User? Author { get; set; }
    public User? Receiver { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
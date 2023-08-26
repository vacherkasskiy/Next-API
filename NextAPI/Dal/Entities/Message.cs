using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextAPI.Dal.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("Author")]
    public int AuthorId { get; set; }
    
    [ForeignKey("Receiver")]
    public int ReceiverId { get; set; }

    public string Text { get; set; }

    public User? Author { get; set; }
    public User? Receiver { get; set; }
}
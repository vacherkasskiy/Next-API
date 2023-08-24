using Microsoft.EntityFrameworkCore;
using NextAPI.Dal.Entities;

namespace NextAPI.Dal;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
}
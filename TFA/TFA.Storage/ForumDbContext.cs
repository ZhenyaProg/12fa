using Microsoft.EntityFrameworkCore;

namespace TFA.Storage;

public class ForumDbContext : DbContext
{
    public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
    public DbSet<ForumEntity> Forums { get; set; }
    public DbSet<TopicEntity> Topics { get; set; }
    public DbSet<Comment> Comments { get; set; }
}
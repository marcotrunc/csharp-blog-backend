using Microsoft.EntityFrameworkCore;
namespace csharp_blog_backend.Models
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }
        public DbSet<Post> Posts { get; set; }
    }
}


    
    


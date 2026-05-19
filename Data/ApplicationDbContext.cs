using Microsoft.EntityFrameworkCore;
using School_Management.Entities;

namespace School_Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        {
        }
        public DbSet<Student> Students { get; set; }
    }
}

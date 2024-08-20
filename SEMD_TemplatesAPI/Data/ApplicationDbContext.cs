using Microsoft.EntityFrameworkCore;
using TemplatesWebsite.Models;

namespace TemplatesAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Template> Templates { get; set; }
    }
}

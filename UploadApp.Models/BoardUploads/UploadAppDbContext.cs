using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace UploadApp.Models.BUploads
{
    /// <summary>
    /// [5] DbContext Class
    /// </summary>
    public class UploadAppDbContext : DbContext
    {
        // SqlServer, InMemory, ConfigurationManager
        public UploadAppDbContext()
        {
            // Empty
        }

        public UploadAppDbContext(DbContextOptions<UploadAppDbContext> options) : base(options)
        {
            // 공식같은 코드 

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = ConfigurationManager.ConnectionStrings[
                    "ConnectionString"].ConnectionString;
                optionsBuilder.UseSqlServer(connectionString); 
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoardUploads>().Property(m => m.Created).HasDefaultValueSql("GetDate()"); 
        }

        public DbSet<BoardUploads> BoardUploads { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    internal class LibDbContext : DbContext
    {
        private string connString = "Data Source=Data/WinSd.db;";
        public DbSet<Trustee> Trustees { get; set; }
        public DbSet<AdSchemaGuid> AdSchemaGuids { get; set; }

        public LibDbContext() { }
        public LibDbContext(string ConnString)
        {
            connString = ConnString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trustee>().ToTable("Trustee").HasKey(t => t.Sid);
            modelBuilder.Entity<AdSchemaGuid>().ToTable("AdSchemaGuid").HasKey(t => t.SchemaIdGuid);
        }
    }
}

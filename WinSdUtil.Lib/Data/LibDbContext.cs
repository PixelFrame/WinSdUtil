﻿using Microsoft.EntityFrameworkCore;
using WinSdUtil.Lib.Model;

namespace WinSdUtil.Lib.Data
{
    internal class LibDbContext : DbContext
    {
        public DbSet<Trustee> Trustees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Data/WinSd.db;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trustee>().ToTable("Trustee").HasKey(t => t.Sid);
        }
    }
}
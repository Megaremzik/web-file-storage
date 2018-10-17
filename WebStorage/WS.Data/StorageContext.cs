using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WS.Data
{
    public class StorageContext : DbContext
    {
        public DbSet<Document> Document { get; set; }
        public DbSet<DocumentLink> DocumentLink { get; set; }
        public DbSet<UserDocument> UserDocument { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDocument>().HasKey(u => new { u.IdUser, u.IdDocument });
        }
        public StorageContext(DbContextOptions<StorageContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}

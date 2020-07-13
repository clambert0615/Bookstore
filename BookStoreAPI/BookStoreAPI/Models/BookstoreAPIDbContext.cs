using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace BookStoreAPI.Models
{
    public partial class BookstoreAPIDbContext : DbContext
    {
        
        public BookstoreAPIDbContext()
        {
        }
        private readonly string Connection;

        public BookstoreAPIDbContext(DbContextOptions<BookstoreAPIDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Connection = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
        }

        public virtual DbSet<Books> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Connection);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>(entity =>
            {
                entity.Property(e => e.Author).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Genre).HasMaxLength(30);

                entity.Property(e => e.Image).HasMaxLength(300);

                entity.Property(e => e.Price).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.PublishDate).HasColumnType("date");

                entity.Property(e => e.Title).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

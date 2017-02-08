using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VoteApp.Models;

namespace VoteApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Poll> Polls { get; set; }
        public DbSet<Option> Options { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Poll>(entity =>
            {
                entity.Property(e => e.Question).IsRequired();

                entity.HasOne(p => p.User)
                      .WithMany(u => u.Polls)
                      .HasForeignKey(p => p.ApplicationUserId)
                      .HasConstraintName("FK_Polls_Users");
            });

            builder.Entity<Option>(entity =>
            {
                entity.Property(e => e.Value).IsRequired();
                entity.Property(e => e.TimesSelected).HasDefaultValue(0);

                entity.HasOne(o => o.Poll)
                      .WithMany(p => p.Options)
                      .HasForeignKey(o => o.PollId)
                      .HasConstraintName("FK_Options_Polls");
            });

            base.OnModelCreating(builder);
        }
    }
}

using BP_OnlineDOD.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BP_OnlineDOD.Server.Data
{
    public class OnlineDODContext : DbContext
    {
        public OnlineDODContext(DbContextOptions<OnlineDODContext> opt) : base(opt)
        {

        }

        public DbSet<Message> Messages { get; set; }

        //public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasMany(m => m.ChildMessages)
                .WithOne(c => c.ParentMessage);
        }

    }
}
using BP_OnlineDOD.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;

namespace BP_OnlineDOD.Server.Data
{
    public class OnlineDODContext : ApiAuthorizationDbContext<IdentityUser>
    {
        public OnlineDODContext(DbContextOptions<OnlineDODContext> opt, 
            IOptions<OperationalStoreOptions> operationalStoreOptions) 
            : base(opt, operationalStoreOptions)
        {

        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Log> Logs { get; set; }

        public DbSet<BlockedIP> BlockedIPs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasMany(m => m.ChildMessages)
                .WithOne(c => c.ParentMessage);

            modelBuilder.Entity<Message>()
                .HasMany(m => m.Attachments)
                .WithOne(c => c.Message);

            modelBuilder.Entity<Log>().ToTable("Logs");

            base.OnModelCreating(modelBuilder);
        }

    }
}
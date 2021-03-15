using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Server.Data
{
    public class SampleData
    {
        private OnlineDODContext _context;

        public SampleData(OnlineDODContext context)
        {
            _context = context;
        }

        public async Task SeedAdminUser(string login = "admin", string password = "Pa$$w0rd")
        {
            var user = new IdentityUser
            {
                UserName = login,
                NormalizedUserName = login.ToUpper(),
                Email = login,
                NormalizedEmail = login.ToUpper(),
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var roleStore = new RoleStore<IdentityRole>(_context);

            if (!_context.Roles.Any(r => r.Name == "Admin"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "Admin" });
            }

            if (!_context.Roles.Any(r => r.Name == "Editor"))
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Editor", NormalizedName = "Editor" });
            }

            if (!_context.Users.Any(u => u.UserName == user.UserName))
            {
                var passwordH = new PasswordHasher<IdentityUser>();
                var hashed = passwordH.HashPassword(user, password);
                user.PasswordHash = hashed;
                var userStore = new UserStore<IdentityUser>(_context);
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "Admin");
                await userStore.AddToRoleAsync(user, "Editor");
            }

            await _context.SaveChangesAsync();
        }
    }
}

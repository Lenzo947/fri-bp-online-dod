using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Server.Data
{
    public static class PrepDB
    {
        public static void PrepDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                MigrateDB(serviceScope.ServiceProvider.GetService<OnlineDODContext>());
            }
        }

        public static void MigrateDB(OnlineDODContext context)
        {
            context.Database.Migrate();

            context.SaveChanges();
        }
    }
}

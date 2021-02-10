using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameInspectorWeb.Data
{
    public static class ApplicationDbContextSeeder
    {
        public async static Task SeedRolesAndUsersAsync(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            var roleName = "admin";
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            var userName = "admin@baboost.com";
            if (!await userManager.Users.AnyAsync(x => x.UserName == userName))
            {
                var user = new ApplicationUser()
                {
                    UserName = userName,
                    Email = userName,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                await userManager.CreateAsync(user, "Ankara1.");
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
        public static async Task<IHost> SeedAsync(this IHost host)
        {

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var env = serviceProvider.GetRequiredService<IHostEnvironment>();
                var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
                await SeedRolesAndUsersAsync(roleManager, userManager);

                if (env.IsDevelopment())
                {
                    //SeedGames();
                }
            }

            return host;
        }

        //private static void SeedGames(ApplicationDbContext db)
        //{
        //    if (!db.Categories.Any())
        //    {
        //        db.Categories.Add(new Category()
        //        {
        //            CategoryName = "Aksiyon"
        //        });
        //    }


        //    if (!db.Articles.Any())
        //    {

        //    }
        //}
    }
}

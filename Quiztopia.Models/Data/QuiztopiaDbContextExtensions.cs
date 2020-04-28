using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiztopia.Models.Data
{
    public static class QuiztopiaDbContextExtensions
    {
        private static List<Difficulty> _difficulties = new List<Difficulty>
        {
            new Difficulty
            {
                Id = 1,
                Name = "Easy"
            },
            new Difficulty
            {
                Id = 2,
                Name = "Intermediate"
            },
            new Difficulty
            {
                Id = 3,
                Name = "Hard"
            }
        };

        public async static Task SeedRoles(RoleManager<IdentityRole> RoleMgr)
        {
            IdentityResult roleResult;
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                // Only add if not exist
                var roleExist = await RoleMgr.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleMgr.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public async static Task SeedUsers(UserManager<IdentityUser> userMgr)
        {
            // ---------- 1. Admin aanmaken ----------

            if (await userMgr.FindByNameAsync("Docent@MCT") == null)        
            {
                var user = new IdentityUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "Docent@MCT",
                    Email = "docent@howest.be"
                };

                var userResult = await userMgr.CreateAsync(user, "Docent@1");
                var roleResult = await userMgr.AddToRoleAsync(user, "Admin");

                if (!userResult.Succeeded || !roleResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }

            // ---------- 2. meerdere users  aanmaken ----------

            var nmbrStudents = 5;

            for (var i = 1; i <= nmbrStudents; i++)
            {
                if (userMgr.FindByNameAsync("User@" + i).Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = ("User@" + i).Trim(),
                        Email = "User" + i + "@student.howest.be",                  
                    };

                    var userResult = await userMgr.CreateAsync(user, "User@" + i);
                    var roleResult = await userMgr.AddToRoleAsync(user, "User");

                    if (!userResult.Succeeded || !roleResult.Succeeded)
                    {
                        throw new InvalidOperationException("Failed to build " + user.UserName);
                    }
                }
            }
        }
    }
}

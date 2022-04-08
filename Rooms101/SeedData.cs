using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Rooms101.Areas.Identity.Data;
using Rooms101.Data;
using Rooms101.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rooms101
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await EnsureRolesAsync(roleManager);

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            await EnsureTestAdminAsync(userManager);

            var dbcontext = services.GetRequiredService<ApplicationDbContext>();

            await SeedTestRoomsAsync(dbcontext);
        }

        private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var alreadyExists = await roleManager.RoleExistsAsync(Constants.AdministratorRole);

            if (alreadyExists) return;

            await roleManager.CreateAsync(new IdentityRole(Constants.AdministratorRole));
        }

        private static async Task EnsureTestAdminAsync(UserManager<ApplicationUser> userManager)
        {
            var testAdmin = userManager.Users.Where(x => x.UserName == "admin@todo.local").SingleOrDefault();

            if (testAdmin != null) return;

            testAdmin = new ApplicationUser
            {
                UserName = "admin@project.local",
                Email = "admin@project.local",
                FullName = "Admin User",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(testAdmin, "NotSecure123!!");

            await userManager.AddToRoleAsync(testAdmin, Constants.AdministratorRole);

            List<ApplicationUser> users = new List<ApplicationUser>();

            users.Add(new ApplicationUser
            {
                UserName = "bob.smith@project.local",
                Email = "bob.smith@project.local",
                FullName = "Bob Smith",
                EmailConfirmed = true
            });
            users.Add(new ApplicationUser
            {
                UserName = "jane.jones@project.local",
                Email = "jane.jones@project.local",
                FullName = "Jane Jones",
                EmailConfirmed = true
            });
            users.Add(new ApplicationUser
            {
                UserName = "bert.baker@project.local",
                Email = "bert.baker@project.local",
                FullName = "Bert Baker",
                EmailConfirmed = true
            });
            users.Add(new ApplicationUser
            {
                UserName = "sarah.cook@project.local",
                Email = "sarah.cook@project.local",
                FullName = "Sarah Cook",
                EmailConfirmed = true
            });

            foreach (var u in users)
            {
                await userManager.CreateAsync(u, "NotSecure123!!");
            }

        }

        private static async Task SeedTestRoomsAsync(ApplicationDbContext dbcontext)
        {
            var seededroom = dbcontext.Rooms.Where(r => r.MeetingRoomName == "Room 101").SingleOrDefault();
            if (seededroom != null) return;

            List<Room> rooms = new List<Room>();

            rooms.Add(new Room
            {
                MeetingRoomName = "Room 101",
                MeetingRoomDescription = "Room 101 : First Floor, East Wing, Holds 8 people.",
                Active = true
            });
            rooms.Add(new Room
            {
                MeetingRoomName = "Room 102",
                MeetingRoomDescription = "Room 102 : First Floor, East Wing, Holds 4 people.",
                Active = true
            });
            rooms.Add(new Room
            {
                MeetingRoomName = "Room 201",
                MeetingRoomDescription = "Room 201 : Second Floor, East Wing, Holds 4 people.",
                Active = true
            });
            rooms.Add(new Room
            {
                MeetingRoomName = "Room 202",
                MeetingRoomDescription = "Room 202 : Second Floor, East Wing, Holds 4 people.",
                Active = true
            });

            // Insert Rooms
            foreach (var room in rooms)
            {
                // System.Console.WriteLine(room.MeetingRoomName);

                dbcontext.Add(room);
                await dbcontext.SaveChangesAsync();
            }



        }

    }
}
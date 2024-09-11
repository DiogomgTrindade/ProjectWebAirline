using ProjectWebAirlineMVC.Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using ProjectWebAirlineMVC.Helpers;
using Microsoft.AspNetCore.Identity;

namespace ProjectWebAirlineMVC.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();


            await _userHelper.CheckRolesAsync("Admin");
            await _userHelper.CheckRolesAsync("Customer");


            var user = await _userHelper.GetUserByEmailAsync("diogovsky1904@gmail.com");
            if(user == null)
            {
                user = new User
                {
                    FirstName = "Diogo",
                    LastName = "Trindade",
                    Email = "diogovsky1904@gmail.com",
                    UserName = "diogovsky1904@gmail.com",
                    PhoneNumber = "123456789"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                };


                await _userHelper.AddUserToRoleAsync(user, "Admin");

            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if(!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }


            if (!_context.Aircrafts.Any())
            {
                AddAircraft("Boeing 737", user);
                AddAircraft("Airbus A310", user);
                AddAircraft("Cessna 120", user);
                AddAircraft("Saab 2000", user);
                await _context.SaveChangesAsync();
            }
        }


        /// <summary>
        /// an Aircraft when Db is empty
        /// </summary>
        /// <param name="name">Aircraft name</param>
        private void AddAircraft(string name, User user)
        {
            _context.Aircrafts.Add(new Aircraft
            {
                Name = name,
                Capacity = _random.Next(100, 500),
                User = user
            });
        }
    }
}


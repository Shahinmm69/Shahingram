﻿using Common.Utilities;
using Data;
using Data.Contract;
using Data.Repositories;
using Entities;
using Entities.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Services.Services;
using System.Linq;
using System.Threading;

namespace Services.DataInitializer
{
    public class UserDataInitializer : IDataInitializer
    {
        private readonly IUserRepository userRepository;
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> userManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        protected readonly ApplicationDbContext DbContext;

        public UserDataInitializer(IUserRepository userRepository, Microsoft.AspNetCore.Identity.UserManager<User> userManager
            , Microsoft.AspNetCore.Identity.RoleManager<Role> roleManager, SignInManager<User> signInManager, ApplicationDbContext dbContext)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            DbContext = dbContext;
        }

        public async void InitializeData()
        {
            if (!userRepository.TableNoTracking.Any(p => p.UserName == "Admin"))
            {
                userRepository.Add(new User
                {
                    UserName = "Admin",
                    Email = "s.maboudi69@gmail.com",
                    FirstName = "Shahin",
                    LastName = "Maboudi Moghaddam",
                    Biography = "Expert developer",
                    BirthCountryId = 1,
                    LifeCountryId = 2,
                    CategoryId = 1,
                    PhoneNumber = "09121111111",
                    CrationDate = DateTime.Now,
                    PasswordHash = SecurityHelper.GetSha256Hash("shahin1369")
                });
                //var user = new User
                //{
                //    UserName = "Admin",
                //    Email = "s.maboudi69@gmail.com",
                //    FirstName = "Shahin",
                //    LastName = "Maboudi Moghaddam",
                //    Biography = "Expert developer",
                //    BirthCountryId = 1,
                //    LifeCountryId = 1,
                //    CategoryId = 1,
                //    PhoneNumber = "09121111111",
                //    CreationDate = DateTime.Now
                //};
                ////_ =  userManager.CreateAsync(user, "shahin1369");
                //var result = roleManager.CreateAsync(new Role
                //{
                //    Name = "Admin",
                //    Description = "admin role"
                //});
                //var result1 = userManager.AddToRoleAsync(user, "Admin");
                //userRepository.Add(user);
            }
        }
    }
}

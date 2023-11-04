//using Data.Contract;
//using Entities;
//using Entities.Models;
//using Microsoft.AspNet.Identity;
//using Microsoft.AspNetCore.Identity;
//using Services.Services;
//using System.Linq;
//using System.Threading;

//namespace Services.DataInitializer
//{
//    public class UserDataInitializer : IDataInitializer
//    {
//        private readonly IUserRepository userRepository;
//        private readonly Microsoft.AspNetCore.Identity.UserManager<User> userManager;
//        private readonly Microsoft.AspNetCore.Identity.RoleManager<Role> roleManager;
//        private readonly SignInManager<User> signInManager;

//        public UserDataInitializer(IUserRepository userRepository, Microsoft.AspNetCore.Identity.UserManager<User> userManager
//            , Microsoft.AspNetCore.Identity.RoleManager<Role> roleManager, SignInManager<User> signInManager)
//        {
//            this.userRepository = userRepository;
//            this.userManager = userManager;
//            this.roleManager = roleManager;
//            this.signInManager = signInManager;
//        }

//        public void InitializeData()
//        {
//            if (!userRepository.TableNoTracking.Any(p => p.UserName == "Admin"))
//            {
//                userRepository.Add(new User
//                {
//                    UserName = "Admin",
//                    Email = "s.maboudi69@gmail.com",
//                    FirstName = "Shahin",
//                    LastName = "Maboudi Moghaddam",
//                    Biography = "Expert developer",
//                    BirthCountryId = 1,
//                    LifeCountryId = 2,
//                    CategoryId = 1,
//                    PhoneNumber = "09121111111",
//                    CrationDate = DateTime.Now
//                });
//                //var user = new User
//                //{
//                //    UserName = "Admin",
//                //    Email = "s.maboudi69@gmail.com",
//                //    FirstName = "Shahin",
//                //    LastName = "Maboudi Moghaddam",
//                //    Biography = "Expert developer",
//                //    BirthCountryId = 1,
//                //    LifeCountryId = 1,
//                //    CategoryId = 1,
//                //    PhoneNumber = "09121111111",
//                //    CrationDate = DateTime.Now
//                //};
//                //_ = userManager.CreateAsync(user, "shahin1369");
//                //_ = roleManager.CreateAsync(new Role
//                //{
//                //    Name = "Admin",
//                //    Description = "admin role"
//                //});
//                //_ = userManager.AddToRoleAsync(user, "Admin");
//            }
//        }
//    }
//}

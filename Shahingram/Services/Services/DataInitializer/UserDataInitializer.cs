using Data.Contract;
using Data.Repositories;
using Entities;
using Entities.Models;
using System.Linq;

namespace Services.DataInitializer
{
    public class UserDataInitializer : IDataInitializer
    {
        private readonly IUserRepository userRepository;

        public UserDataInitializer(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void InitializeData()
        {
            if (!userRepository.TableNoTracking.Any(p => p.UserName == "Admin"))
            {
                userRepository.Add(new User
                {
                    UserName = "Admin",
                    Email = "s.maboudi69@gmail.com",
                    FirstName = "Shahin",
                    MiddleName = "Maboudi",
                    LastName = "Moghaddam",
                    Biography = "Expert developer",
                    PhoneNumber = "09121111111",
                    PasswordHash = "shahin1369"
                });
            }
        }
    }
}

using Entities.Models;
using WebFramework.Api;

namespace Shahingram.Models
{
    public class UserDto 
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Biography { get; set; }
        public int BirthCountryId { get; set; }
        public int LifeCountryId { get; set; }
        public int CategoryId { get; set; }
        public string Password { get; set; }
    }

    public class UserSelectDto 
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Biography { get; set; }
        public string BirthCountryTitle { get; set; }
        public string LifeCountryTitle { get; set; }
        public string CategoryTitle { get; set; }
        public string CrationDate { get; set; }
        public string ModificationDate { get; set; }
        public string IsDeleted { get; set; }
        public string DeletionDate { get; set; }
        public string PhotoAddress { get; set; }
        public int? FollowersCount { get; set; }
        public int? FollowingsCount { get; set; }
        public int PostsCount { get; set; }
        public List<int> PostsId { get; set; }
        public string UserCraetionName { get; set; }
        public string UserModificationName { get; set; }
        public string UserDeletionName { get; set; }
    }
}

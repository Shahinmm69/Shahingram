using Entities.Common;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Numerics;
using System.Reflection;

namespace Entities.Models
{
    public class User : IdentityUser<int>, IDeletion, IEntity
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }

        public DateTime CrationDate { get; set; }
        public int UserCraetionId { get; set; }

        public DateTime? ModificationDate { get; set; }
        public int? UserModificationId { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int? UserDeletionId { get; set; }

        public int BirthCountryId { get; set; }
        public int LifeCountryId { get; set; }
        public int CategoryId { get; set; }

        public virtual Country Country { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<UserPhoto>? UserPhotos { get; set; }
        public virtual ICollection<Follow>? Follows { get; set; }
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<Direct>? Directs { get; set; }

        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                builder.Property(p => p.MiddleName).HasMaxLength(100);
                builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);
                builder.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(10);
                builder.Property(p => p.Email).IsRequired().HasMaxLength(10);
                builder.Property(p => p.Biography).IsRequired().HasMaxLength(500);
                builder.Property(p => p.PasswordHash).IsRequired().HasMaxLength(10);
                builder.HasOne(p => p.Country).WithMany(c => c.Users).HasForeignKey(p => p.BirthCountryId);
                builder.HasOne(p => p.Country).WithMany(c => c.Users).HasForeignKey(p => p.LifeCountryId);
                builder.HasOne(p => p.Category).WithMany(c => c.Users).HasForeignKey(p => p.CategoryId);
            }
        }
    }
}
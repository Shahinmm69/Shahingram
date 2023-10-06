using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Numerics;
using System.Reflection;

namespace Entittis
{
    public class User : Modification, IDeletion
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Biography { get; set; }
        public string PasswordHash { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DeletionDate { get; set; }
        public int UserDeletionId { get; set; }

        public int BirthCountryId { get; set; }
        public int LifeCountryId { get; set; }
        public int CategoryId { get; set; }

        public virtual Country Country { get; set; } = null!;
        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<UserPhoto> UserPhotos { get; set; }
        public virtual ICollection<Follow> Follows { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Direct> Directs { get; set; }

        public class UserConfiguration : IEntityTypeConfiguration<User>
        {
            public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                builder.Property(p => p.MiddleName).HasMaxLength(100);
                builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
                builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);
                builder.Property(p => p.Mobile).IsRequired().HasMaxLength(10);
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
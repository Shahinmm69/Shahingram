using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Follow : Craetion, IDeletion
    {
        public bool IsDeleted { get; set; }
        public DateTime DeletionDate { get; set; }
        public int UserDeletionId { get; set; }

        public int UserId { get; set; }
        public int FollowId { get; set; }

        public virtual User User { get; set; } = null!;

        public class FollowConfiguration : IEntityTypeConfiguration<Follow>
        {
            public void Configure(EntityTypeBuilder<Follow> builder)
            {
                builder.HasOne(p => p.User).WithMany(c => c.Follows).HasForeignKey(p => p.UserId);
                builder.HasOne(p => p.User).WithMany(c => c.Follows).HasForeignKey(p => p.FollowId);
            }
        }
    }
}
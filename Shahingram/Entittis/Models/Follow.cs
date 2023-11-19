using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Follow : Creation, IDeletion
    {
        public bool? IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int? UserDeletionId { get; set; }

        public int UserFollowId { get; set; }

        public virtual User UserCreation { get; set; } = null!;
        public virtual User UserFollow { get; set; } = null!;

        public class FollowConfiguration : IEntityTypeConfiguration<Follow>
        {
            public void Configure(EntityTypeBuilder<Follow> builder)
            {
                builder.HasOne(p => p.UserCreation).WithMany().HasForeignKey(p => p.UserCreationId).OnDelete(DeleteBehavior.NoAction);
                builder.HasOne(p => p.UserFollow).WithMany().HasForeignKey(p => p.UserFollowId).OnDelete(DeleteBehavior.NoAction);
            }
        }
    }
}
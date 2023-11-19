using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Like : Creation, IDeletion
    {
        public bool? IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int? UserDeletionId { get; set; }

        public int PostId { get; set; }

        public virtual User UserCreation { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;

        public class LikeConfiguration : IEntityTypeConfiguration<Like>
        {
            public void Configure(EntityTypeBuilder<Like> builder)
            {
                builder.HasOne(p => p.UserCreation).WithMany(c => c.Likes).HasForeignKey(p => p.UserCreationId);
                builder.HasOne(p => p.Post).WithMany(c => c.Likes).HasForeignKey(p => p.PostId);
            }
        }
    }
}
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entittis
{
    public class Like : Craetion, IDeletion
    {
        public bool IsDeleted { get; set; }
        public DateTime DeletionDate { get; set; }
        public int UserDeletionId { get; set; }

        public int UserId { get; set; }
        public int PostId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;

        public class LikeConfiguration : IEntityTypeConfiguration<Like>
        {
            public void Configure(EntityTypeBuilder<Like> builder)
            {
                builder.HasOne(p => p.User).WithMany(c => c.Likes).HasForeignKey(p => p.UserId);
                builder.HasOne(p => p.Post).WithMany(c => c.Likes).HasForeignKey(p => p.PostId);
            }
        }
    }
}
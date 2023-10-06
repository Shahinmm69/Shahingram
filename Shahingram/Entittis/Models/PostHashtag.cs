using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class PostHashtag : BaseEntity
    {
        public int PostId { get; set; }
        public int HashtagId { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual Hashtag Hashtag { get; set; } = null!;

        public class PostHashtagConfiguration : IEntityTypeConfiguration<PostHashtag>
        {
            public void Configure(EntityTypeBuilder<PostHashtag> builder)
            {
                builder.HasOne(p => p.Post).WithMany(c => c.PostHashtags).HasForeignKey(p => p.PostId);
                builder.HasOne(p => p.Hashtag).WithMany(c => c.PostHashtags).HasForeignKey(p => p.HashtagId);
            }
        }
    }
}
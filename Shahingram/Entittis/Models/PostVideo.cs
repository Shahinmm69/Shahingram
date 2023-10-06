using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class PostVideo : BaseEntity
    {
        public int PostId { get; set; }
        public int VideoId { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual Video Video { get; set; } = null!;

        public class PostVideoConfiguration : IEntityTypeConfiguration<PostVideo>
        {
            public void Configure(EntityTypeBuilder<PostVideo> builder)
            {
                builder.HasOne(p => p.Post).WithMany(c => c.PostVideos).HasForeignKey(p => p.PostId);
                builder.HasOne(p => p.Video).WithMany(c => c.PostVideos).HasForeignKey(p => p.VideoId);
            }
        }
    }
}
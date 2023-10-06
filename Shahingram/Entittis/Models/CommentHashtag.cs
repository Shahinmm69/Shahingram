using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class CommentHashtag : BaseEntity
    {
        public int CommentId { get; set; }
        public int HashtagId { get; set; }

        public virtual Comment Comment { get; set; } = null!;
        public virtual Hashtag Hashtag { get; set; } = null!;

        public class CommentHashtagConfiguration : IEntityTypeConfiguration<CommentHashtag>
        {
            public void Configure(EntityTypeBuilder<CommentHashtag> builder)
            {
                builder.HasOne(p => p.Comment).WithMany(c => c.CommentHashtags).HasForeignKey(p => p.CommentId);
                builder.HasOne(p => p.Hashtag).WithMany(c => c.CommentHashtags).HasForeignKey(p => p.HashtagId);
            }
        }
    }
}
using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class PostPhoto : BaseEntity
    {
        public int PostId { get; set; }
        public int PhotoId { get; set; }

        public virtual Post Post { get; set; } = null!;
        public virtual Photo Photo { get; set; } = null!;

        public class PostPhotoConfiguration : IEntityTypeConfiguration<PostPhoto>
        {
            public void Configure(EntityTypeBuilder<PostPhoto> builder)
            {
                builder.HasOne(p => p.Post).WithMany(c => c.PostPhotos).HasForeignKey(p => p.PostId);
                builder.HasOne(p => p.Photo).WithMany(c => c.PostPhotos).HasForeignKey(p => p.PhotoId);
            }
        }
    }
}
using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class DirectPhoto : BaseEntity
    {
        public int DirectId { get; set; }
        public int PhotoId { get; set; }

        public virtual Direct Direct { get; set; } = null!;
        public virtual Photo Photo { get; set; } = null!;

        public class DirectPhotoConfiguration : IEntityTypeConfiguration<DirectPhoto>
        {
            public void Configure(EntityTypeBuilder<DirectPhoto> builder)
            {
                builder.HasOne(p => p.Direct).WithMany(c => c.DirectPhotos).HasForeignKey(p => p.DirectId);
                builder.HasOne(p => p.Photo).WithMany(c => c.DirectPhotos).HasForeignKey(p => p.PhotoId);
            }
        }
    }
}
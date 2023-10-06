using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entittis
{
    public class DirectVideo : BaseEntity
    {
        public int DirectId { get; set; }
        public int VideoId { get; set; }

        public virtual Direct Direct { get; set; } = null!;
        public virtual Video Video { get; set; } = null!;

        public class DirectVideoConfiguration : IEntityTypeConfiguration<DirectVideo>
        {
            public void Configure(EntityTypeBuilder<DirectVideo> builder)
            {
                builder.HasOne(p => p.Direct).WithMany(c => c.DirectVideos).HasForeignKey(p => p.DirectId);
                builder.HasOne(p => p.Video).WithMany(c => c.DirectVideos).HasForeignKey(p => p.VideoId);
            }
        }
    }
}
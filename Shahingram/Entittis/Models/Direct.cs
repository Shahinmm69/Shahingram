using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Direct : Modification, IDeletion
    {
        [Required]
        public string Text { get; set; }

        public bool SenderIsDeleted { get; set; }
        public bool ReceiverIsDeleted { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DeletionDate { get; set; }
        public int UserDeletionId { get; set; }

        public int UserSenderId { get; set; }
        public int UserReceiverId { get; set; }
        public int PostId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
        public virtual ICollection<DirectPhoto> DirectPhotos { get; set; }
        public virtual ICollection<DirectVideo> DirectVideos { get; set; }

        public class DirectConfiguration : IEntityTypeConfiguration<Direct>
        {
            public void Configure(EntityTypeBuilder<Direct> builder)
            {
                builder.HasOne(p => p.User).WithMany(c => c.Directs).HasForeignKey(p => p.UserSenderId);
                builder.HasOne(p => p.User).WithMany(c => c.Directs).HasForeignKey(p => p.UserReceiverId);
                builder.HasOne(p => p.Post).WithMany(c => c.Directs).HasForeignKey(p => p.PostId);
            }
        }
    }
}
using Entities.Common;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Comment : Modification, IDeletion
    {
        [Required]
        public string Text { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int? UserDeletionId { get; set; }

        public int UserId { get; set; }
        public int PostId { get; set; }
        public int? ReplyId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
        public virtual Comment? Reply { get; set; } = null!;
        public virtual ICollection<CommentHashtag>? CommentHashtags { get; set; }
        public virtual ICollection<Comment>? Children { get; set; }

        public class CommentConfiguration : IEntityTypeConfiguration<Comment>
        {
            public void Configure(EntityTypeBuilder<Comment> builder)
            {
                builder.HasOne(p => p.User).WithMany(c => c.Comments).HasForeignKey(p => p.UserId);
                builder.HasOne(p => p.Post).WithMany(c => c.Comments).HasForeignKey(p => p.PostId);
                builder.HasIndex(e => e.ReplyId);
                builder.HasOne(d => d.Reply).WithMany(p => p.Children).HasForeignKey(d => d.ReplyId);
            }
        }
    }
}
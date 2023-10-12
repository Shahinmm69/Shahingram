using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class Post : Modification, IDeletion
    {
        public string? Text { get; set; }


        public bool? IsDeleted { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int? UserDeletionId { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual ICollection<Like>? Likes { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
        public virtual ICollection<PostPhoto>? PostPhotos { get; set; }
        public virtual ICollection<PostVideo>? PostVideos { get; set; }
        public virtual ICollection<PostHashtag>? PostHashtags { get; set; }
        public virtual ICollection<Direct>? Directs { get; set; }

        public class PostConfiguration : IEntityTypeConfiguration<Post>
        {
            public void Configure(EntityTypeBuilder<Post> builder)
            {
                builder.HasOne(p => p.User).WithMany(c => c.Posts).HasForeignKey(p => p.UserId);
            }
        }
    }
}
﻿using Entities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entittis
{
    public class UserPhoto : BaseEntity
    {
        public int UserId { get; set; }
        public int PhotoId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual Photo Photo { get; set; } = null!;

        public class UserPhotoConfiguration : IEntityTypeConfiguration<UserPhoto>
        {
            public void Configure(EntityTypeBuilder<UserPhoto> builder)
            {
                builder.HasOne(p => p.User).WithMany(c => c.UserPhotos).HasForeignKey(p => p.UserId);
                builder.HasOne(p => p.Photo).WithMany(c => c.UserPhotos).HasForeignKey(p => p.PhotoId);
            }
        }
    }
}
using KLCN_TH051_Website.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLCN_TH051_Website.Common.Configurations
{
    public class VideoContentConfiguration : IEntityTypeConfiguration<VideoContent>
    {
        public void Configure(EntityTypeBuilder<VideoContent> builder)
        {
            // Table name
            builder.ToTable("VideoContents");

            // Primary Key
            builder.HasKey(v => v.Id);

            // Relationship: Lesson 1 - n VideoContents
            builder.HasOne(v => v.Lesson)
                   .WithMany(l => l.VideoContents)
                   .HasForeignKey(v => v.LessonId)
                   .OnDelete(DeleteBehavior.Cascade);

            // VideoUrl (bắt buộc)
            builder.Property(v => v.VideoUrl)
                   .HasMaxLength(500)
                   .IsRequired();

            // Quality (bắt buộc)
            builder.Property(v => v.Quality)
                   .HasMaxLength(50)
                   .IsRequired();

            // Subtitle (optional)
            builder.Property(v => v.Subtitle)
                   .HasMaxLength(500)
                   .IsRequired(false);

            // DurationSeconds
            builder.Property(v => v.DurationSeconds)
                   .IsRequired();

            // ----------------------------
            // Cấu hình các cột từ BaseEntity
            // ----------------------------

            builder.Property(c => c.CreatedDate)
                   .HasDefaultValueSql("GETDATE()"); // default ngày tạo

            builder.Property(c => c.LastUpdatedDate)
                   .IsRequired(false); // optional

            builder.Property(c => c.DeletedTime)
                   .IsRequired(false); // optional

            builder.Property(c => c.IsDeleted)
                   .HasDefaultValue(false); // mặc định false

            builder.Property(c => c.CreatedBy)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(c => c.LastUpdatedBy)
                   .HasMaxLength(50)
                   .IsRequired(false);

            builder.Property(c => c.DeletedBy)
                   .HasMaxLength(50)
                   .IsRequired(false);
        }
    }
}

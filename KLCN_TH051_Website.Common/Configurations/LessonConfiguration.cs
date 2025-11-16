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
    public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.ToTable("Lessons");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Title)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(l => l.Description)
                   .HasMaxLength(1000);

            builder.Property(l => l.OrderNumber)
                   .IsRequired();

            builder.Property(l => l.DurationMinutes)
                   .IsRequired();

            builder.Property(l => l.IsFree)
                   .IsRequired();

            // ----------------------------
            // Thêm trường Type
            // ----------------------------
            builder.Property(l => l.Type)
                   .IsRequired()
                   .HasConversion<int>(); // Content=1, Video=2, Quiz=3

            // Liên kết với Chapter
            builder.HasOne(l => l.Chapter)
                   .WithMany(c => c.Lessons)
                   .HasForeignKey(l => l.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);

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

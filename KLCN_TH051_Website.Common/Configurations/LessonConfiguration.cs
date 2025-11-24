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

            builder.Property(l => l.OrderNumber)
                .IsRequired();

            builder.Property(l => l.DurationMinutes)
                .IsRequired();

            builder.Property(l => l.IsFree)
                .HasDefaultValue(false);

            // ----------------------------
            // Enum Type
            // ----------------------------
            builder.Property(l => l.Type)
                .IsRequired()
                .HasConversion<int>()
                .HasComment("1 = Content, 2 = Video, 3 = Quiz");

            // ----------------------------
            // Content: JSON từ Tiptap
            // ----------------------------
            builder.Property(l => l.Content)
                .HasColumnType("nvarchar(max)")
                .IsRequired(false);

            // ----------------------------
            // VideoUrl: link youtube
            // ----------------------------
            builder.Property(l => l.VideoUrl)
                .HasMaxLength(1000)
                .IsRequired(false);

            // ----------------------------
            // Relation: Chapter - Lesson
            // ----------------------------
            builder.HasOne(l => l.Chapter)
                .WithMany(c => c.Lessons)
                .HasForeignKey(l => l.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----------------------------
            // Index tối ưu load bài học
            // ----------------------------
            builder.HasIndex(l => new { l.ChapterId, l.OrderNumber });

            // ----------------------------
            // BaseEntity fields
            // ----------------------------
            builder.Property(l => l.CreatedDate)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(l => l.LastUpdatedDate)
                .IsRequired(false);

            builder.Property(l => l.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(l => l.DeletedTime)
                .IsRequired(false);

            builder.Property(l => l.CreatedBy)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(l => l.LastUpdatedBy)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(l => l.DeletedBy)
                .HasMaxLength(50)
                .IsRequired(false);
        }
    }
}

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
    public class ContentBlockConfiguration : IEntityTypeConfiguration<ContentBlock>
    {
        public void Configure(EntityTypeBuilder<ContentBlock> builder)
        {
            // Table name
            builder.ToTable("ContentBlocks");

            // Primary key
            builder.HasKey(cb => cb.Id);

            // Relationship: Lesson 1 - n ContentBlocks
            builder.HasOne(cb => cb.Lesson)
                   .WithMany(l => l.ContentBlocks)
                   .HasForeignKey(cb => cb.LessonId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Enum mapping (stored as int in DB)
            builder.Property(cb => cb.Type)
                   .HasConversion<int>()
                   .IsRequired();

            // Optional fields
            builder.Property(cb => cb.TextContent)
                   .HasColumnType("nvarchar(max)")
                   .IsRequired(false);

            builder.Property(cb => cb.ImageUrl)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(cb => cb.ImageCaption)
                   .HasMaxLength(300)
                   .IsRequired(false);

            builder.Property(cb => cb.Format)
                   .HasMaxLength(50)
                   .IsRequired(false);

            // Order number (default = 0)
            builder.Property(cb => cb.Order)
                   .HasDefaultValue(0);

            //// Audit fields (từ BaseEntity)
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

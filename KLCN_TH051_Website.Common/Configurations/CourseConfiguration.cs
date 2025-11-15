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
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            // Table
            builder.ToTable("Courses");

            // Primary key
            builder.HasKey(c => c.Id);

            // Properties
            builder.Property(c => c.Code)
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasIndex(c => c.Code)
                   .IsUnique();

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Description)
                   .HasMaxLength(1000);

            builder.Property(c => c.Thumbnail)
                   .HasMaxLength(500);

            builder.Property(c => c.Price)
                   .HasColumnType("decimal(18,2)");

            builder.Property(c => c.StartDate);
            builder.Property(c => c.EndDate);

            builder.Property(c => c.Status)
                   .HasConversion<int>()
                   .IsRequired();

            // Relationships
            builder.HasOne(c => c.Subject)
                   .WithMany(s => s.Courses)
                   .HasForeignKey(c => c.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(c => c.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(c => c.Chapters)
            //       .WithOne(ch => ch.Course)
            //       .HasForeignKey(ch => ch.CourseId)
            //       .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(c => c.Enrollments)
            //       .WithOne(e => e.Course)
            //       .HasForeignKey(e => e.CourseId)
            //       .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(c => c.CourseRatings)
            //       .WithOne(r => r.Course)
            //       .HasForeignKey(r => r.CourseId)
            //       .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(c => c.Payments)
            //       .WithOne(p => p.Course)
            //       .HasForeignKey(p => p.CourseId)
            //       .OnDelete(DeleteBehavior.Restrict);

            // Audit fields (từ BaseEntity)
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

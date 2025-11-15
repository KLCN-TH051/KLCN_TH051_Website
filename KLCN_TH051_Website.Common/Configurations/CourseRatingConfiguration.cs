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
    public class CourseRatingConfiguration : IEntityTypeConfiguration<CourseRating>
    {
        public void Configure(EntityTypeBuilder<CourseRating> builder)
        {
            // Tên bảng
            builder.ToTable("CourseRatings");

            // Khóa chính
            builder.HasKey(cr => cr.Id);

            // Rating
            builder.Property(cr => cr.Rating)
                   .IsRequired();

            // Comment
            builder.Property(cr => cr.Comment)
                   .HasMaxLength(1000);

            // Học viên
            builder.HasOne(cr => cr.Student)
                   .WithMany()  // ApplicationUser không có danh sách Rating
                   .HasForeignKey(cr => cr.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Khóa học
            builder.HasOne(cr => cr.Course)
                   .WithMany(c => c.CourseRatings)    
                   .HasForeignKey(cr => cr.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Không cho 1 học viên đánh giá 1 khóa nhiều lần
            builder.HasIndex(cr => new { cr.StudentId, cr.CourseId })
                   .IsUnique();

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

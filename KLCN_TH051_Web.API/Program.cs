using Google;
using KLCN_TH051_Web.Repositories.Data;
using KLCN_TH051_Web.Services.Models;
using KLCN_TH051_Web.Services.Services;
using KLCN_TH051_Website.Common.Entities;
using KLCN_TH051_Website.Common.Helpers;
using KLCN_TH051_Website.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// 🔸 Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
// 🔸 Gọi DependencyInjection để đăng ký DbContext
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}
builder.Services.AddRepository(connectionString);

// 🔸 Cấu hình Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
//  Cấu hình JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),

        // 👇 Thêm 2 dòng này
        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role
    };
});
// 🔸 Đăng ký dịch vụ Email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
// 🔸 Đăng ký dịch vụ Account
builder.Services.AddScoped<IAccountService, AccountService>();
// 🔸 Đăng ký JwtHelper
builder.Services.AddScoped<JwtHelper>();
// Đăng ký GoogleAuthService
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
// Đăng ký SubjectService
builder.Services.AddScoped<ISubjectService, SubjectService>();
// Đăng ký CourseService
builder.Services.AddScoped<ICourseService, CourseService>();
// Đăng ký ChapterService
builder.Services.AddScoped<IChapterService, ChapterService>();
// Đăng ký LessonService
builder.Services.AddScoped<ILessonService, LessonService>();
// Đăng ký ContentBlockService
builder.Services.AddScoped<IContentBlockService, ContentBlockService>();
// Đăng ký VideoContentService
builder.Services.AddScoped<IVideoContentService, VideoContentService>();
// Đăng ký QuizService
builder.Services.AddScoped<IQuizService, QuizService>();
// Đăng ký QuestionService
builder.Services.AddScoped<IQuestionService, QuestionService>();
// Đăng ký EnrollmentService
builder.Services.AddScoped<IAnswerService, AnswerService>();
// Đăng ký EnrollmentService
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
// Đăng ký PaymentService
builder.Services.AddScoped<ILessonProgressService, LessonProgressService>();
// Đăng ký PaymentService
builder.Services.AddScoped<ILessonCommentService, LessonCommentService>();
// Đăng ký PaymentService
builder.Services.AddScoped<ICourseRatingService, CourseRatingService>();
// Đăng ký PaymentService
builder.Services.AddScoped<IQuizAttemptService, QuizAttemptService>();
// Đăng ký PaymentService
builder.Services.AddScoped<IBannerService, BannerService>();
// 🔸 Cấu hình Authorization với Policy
builder.Services.AddAuthorization(options =>
{
    // Tạo Policy cho từng quyền – sau này chỉ cần dùng [Authorize(Policy = "Subject.Delete")]
    options.AddPolicy("Subject.Create", policy => policy.RequireClaim("Permission", "Subject.Create"));
    options.AddPolicy("Subject.Edit", policy => policy.RequireClaim("Permission", "Subject.Edit"));
    options.AddPolicy("Subject.Delete", policy => policy.RequireClaim("Permission", "Subject.Delete"));
    options.AddPolicy("Subject.View", policy => policy.RequireClaim("Permission", "Subject.View"));

    options.AddPolicy("Course.Create", policy => policy.RequireClaim("Permission", "Course.Create"));
    options.AddPolicy("Course.Edit", policy => policy.RequireClaim("Permission", "Course.Edit"));
    options.AddPolicy("Course.Delete", policy => policy.RequireClaim("Permission", "Course.Delete"));
    options.AddPolicy("Course.View", policy => policy.RequireClaim("Permission", "Course.View"));

    options.AddPolicy("User.Manage", policy => policy.RequireClaim("Permission", "User.Create", "User.Edit", "User.Delete"));
    options.AddPolicy("Role.Manage", policy => policy.RequireClaim("Permission", "Role.Manage"));
    // Thêm thoải mái ở đây nếu cần
});
// Đăng ký TeacherAssignmentService
builder.Services.AddScoped<ITeacherAssignmentService, TeacherAssignmentService>();



// 🔸 Cấu hình Swagger để hỗ trợ xác thực JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Thêm cấu hình bảo mật JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập 'Bearer' + token JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
// ✅ Cấu hình CORS để cho phép frontend truy cập API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7267", "http://localhost:5103")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});




builder.Services.AddControllers()
.AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
 });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();







var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.Migrate(); // Tự động tạo DB + migrate

        await SeedData.Initialize(services); // ← Seed Role + Permission + Admin
        Console.WriteLine("Seed dữ liệu thành công!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi seed dữ liệu");
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// ✅ Áp dụng CORS
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

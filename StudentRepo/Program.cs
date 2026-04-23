using Microsoft.EntityFrameworkCore;
using StudentRepo.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// Auth
// var jwtKey = builder.Configuration["JwtSettings:SecretKey"];
// builder.Services
//     .AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
//     .AddJwtBearer(options =>
//     {
//         options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//         {
//             ValidateIssuer = false,
//             ValidateAudience = false,
//             ValidateIssuerSigningKey = true,
//             IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(jwtKey ?? string.Empty))
//         };
//     });

// builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB
builder.Services.AddDbContext<StudentDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped <IStudentManager, StudentManager>();
builder.Services.AddHttpClient();

var app = builder.Build();

// Seed DB
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<StudentDbContext>();
    DbInitializer.Initialize(context);
}

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// app.UseMiddleware<CustomExceptionMiddleware>(); // optional but recommended

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
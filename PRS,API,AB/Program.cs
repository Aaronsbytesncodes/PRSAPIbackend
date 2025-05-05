using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PRS_API_AB;
using PRS_API_AB.Services;
// Add this using directive for SwaggerUI extensions
// Ensure the following NuGet package is installed in your project:
// Microsoft.AspNetCore.Authentication.JwtBearer

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PRS API", Version = "v1" });
});
builder.Services.AddDbContext<PrsDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<AuthService>();

// Configure authentication and authorization

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Users/Login"; // Path to your login endpoint
        options.LogoutPath = "/Users/Logout"; // Path to your logout endpoint
        options.AccessDeniedPath = "/Users/AccessDenied"; // Optional: Path for access denied
        options.Cookie.Name = "AuthCookie"; // Name of the cookie
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Cookie expiration time
        options.SlidingExpiration = true; // Renew cookie on activity
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Ensure Swashbuckle.AspNetCore is installed
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
 

//Register Context 1 
using Microsoft.EntityFrameworkCore;
using csharp_blog_backend.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("https://localhost:3000").AllowAnyHeader().AllowAnyMethod();
        });
});
builder.Services.AddControllers();

//Register Context 2
builder.Services.AddDbContext<BlogContext>(options =>
options.UseInMemoryDatabase("Posts"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; // Required for OpenApiInfo
using MiniTwitter.Model;
using MiniTwitter.Repository;
using MiniTwitter.Service;
using MiniTwitter.Service.impl;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<UserService, UserServiceImpl>();
builder.Services.AddScoped<PostService, PostServiceImpl>();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .WithOrigins("http://localhost:4200") // Angular app URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});


builder.Services.AddDbContext<MiniTwitterDb>(options =>
    options.UseInMemoryDatabase("MiniTwitterDB"));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MiniTwitter API",
        Description = "An ASP.NET Core Web API for a simple Twitter clone.",

    });
});

var app = builder.Build();


using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<MiniTwitterDb>();
db.Database.EnsureCreated();

db.Users.AddRange(
    new User("user"),
    new User("user2")
);

// Add some posts
db.Posts.AddRange(
    new Post("Hello, this is my first post, hope you like it", 1),
    new Post("Second post, im still experimenting", 1),
    new Post("Hello everyone, hope everyone is doing okay :)", 2)
);

db.SaveChanges();

app.UseSwagger();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI(options =>
    {
        // Specify the Swagger JSON endpoint.
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniTwitter API V1");

        // Optional: To serve the Swagger UI at the app's root URL (e.g., https://localhost:5001/)
        // options.RoutePrefix = string.Empty; 
    });
}

app.UseCors("AllowAngularApp");

app.MapControllers();

app.Run();

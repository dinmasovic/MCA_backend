using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models; 
using MiniTwitter.Model;
using MiniTwitter.Repository;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


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

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
);

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

// --- USERS ---
db.Users.AddRange(
    new User("user"),   // ova e logiraniot user
    new User("user2"),
    new User("petar"),
    new User("nikola"),
    new User("stefan"),
     new User("coffee_addict"),
    new User("travel_bug"),
    new User("bookworm"),
    new User("gamer_x"),
    new User("fitness_freak"),
    new User("foodie_queen"),
    new User("movie_buff")
);

db.Posts.AddRange(
    // user (ID = 1)
    new Post("Hello, this is my first post, hope you like it!", 1),
    new Post("Second post — I'm still experimenting with this app.", 1),
    new Post("Just added a new feature to MiniTwitter! 🚀", 1),
    new Post("What's everyone working on today?", 1),
    new Post("Can't believe how easy it is to share thoughts here 😄", 1),

 
    new Post("Hello everyone, hope you're all doing great :)", 2),
    new Post("Trying to figure out how hashtags might work here...", 2),
    new Post("Enjoying the simplicity of this app.", 2),

    new Post("Just read about CQRS — pretty cool design pattern for clean architecture.", 3),
    new Post("Anyone here using .NET with Angular? Would love to exchange ideas.", 3),
    new Post("AI-generated content is fascinating but tricky to regulate.", 3),

    new Post("The sunset today was absolutely breathtaking 🌅", 4),
    new Post("Anyone else loves hiking as much as I do?", 4),
    new Post("Nature is the best therapy 🌲", 4),

    new Post("Listening to some chill lo-fi beats while coding 🎧", 5),
    new Post("Music makes everything better.", 5),
    new Post("Drop your favorite artists below!", 5),

    new Post("Coffee count today: 3 cups ☕☕☕", 6),
    new Post("Monday mornings hit harder without coffee.", 6),
    new Post("Brewing a new espresso blend today!", 6),
    new Post("Late-night debugging with a cup of coffee = productivity!", 6),
    new Post("Coffee is not just a drink, it’s a lifestyle.", 6),

    new Post("Just booked a trip to Italy 🇮🇹 Can’t wait!", 7),
    new Post("Airports give me mixed feelings — excitement and stress 😅", 7),
    new Post("Travel tip: always pack an extra pair of socks.", 7),
    new Post("Exploring new cities is my favorite way to learn.", 7),
    new Post("Dreaming of visiting Japan someday 🇯🇵", 7),

    new Post("Currently reading 'Atomic Habits' — really inspiring!", 8),
    new Post("Books are like time machines. 📚", 8),
    new Post("Just finished a mystery novel — didn’t see that twist coming!", 8),
    new Post("Reading before bed is the best form of relaxation.", 8),
    new Post("Need book recommendations for the weekend!", 8),

    new Post("Just hit level 50 in my favorite game 🔥", 9),
    new Post("Anyone else into strategy games?", 9),
    new Post("Playing late-night matches again 😅", 9),
    new Post("Can’t believe I just lost because of lag 😭", 9),
    new Post("Teamwork makes the dream work 💪 #gaming", 9),

    new Post("Morning run complete ✅ Feeling energized!", 10),
    new Post("Consistency beats motivation every time.", 10),
    new Post("Don’t wait for Monday to start your fitness journey!", 10),
    new Post("Protein shake after workout = perfection 💪", 10),
    new Post("Gym time is my therapy.", 10),

    new Post("Tried making homemade sushi — success! 🍣", 11),
    new Post("Pasta night is the best night.", 11),
    new Post("Who else loves breakfast for dinner? 🥞", 11),
    new Post("Food photography is harder than it looks 😂", 11),
    new Post("Spicy ramen challenge accepted 🔥", 11),

    new Post("Just watched 'Inception' again — never gets old!", 12),
    new Post("Movie night recommendations?", 12),
    new Post("Christopher Nolan really knows how to mess with your mind.", 12),
    new Post("Watched a documentary on AI — super interesting!", 12),
    new Post("Popcorn + movie = perfect combo 🍿", 12)


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

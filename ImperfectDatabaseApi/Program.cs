using ImperfectDatabaseApi.Models;
using Microsoft.EntityFrameworkCore;

const string CorsPolicyName = "__corspolicy";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// todo: Get the correct connection info,
// this is for local CosmosDB Emulator
builder.Services.AddDbContext<ImperfectDataContext>(options =>
    options.UseCosmos(
        "https://localhost:8081",
        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
        databaseName: "ImperfectDb")
);

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName, policy =>
    {
        policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    // Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    using (var db = scope.ServiceProvider.GetRequiredService<ImperfectDataContext>())
    {
        // be careful with this one:
        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();

        // Create some default data
        var user = new User
        {
            Name = "demo",
            Profile = new User.UserProfile
            {
                FirstName = "Demi",
                LastName = "Demo",
                DateOfBirth = new DateTime(2000, 1, 1)
            }
        };
        var posts = new List<Post>()
        {
            new Post { Author = user.ToAuthor(), Text = "My very first post", Created = new DateTime(2022,09,14)},
            new Post { Author = user.ToAuthor(), Text = "My very second post", Created = new DateTime(2022,10,15)},
        };

        await db.Users.AddAsync(user);
        await db.Posts.AddRangeAsync(posts);
        await db.SaveChangesAsync();
    }
}

app.UseCors(CorsPolicyName);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

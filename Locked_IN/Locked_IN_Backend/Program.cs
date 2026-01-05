using Locked_IN_Backend.Controllers;
using Microsoft.EntityFrameworkCore;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Services;
using Locked_IN_Backend.Hubs;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Repositories;
using Locked_IN_Backend.Mappings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secret.json", optional: true, reloadOnChange: true);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequireDigit = true;
        
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<TeamSettings>(
    builder.Configuration.GetSection(TeamSettings.SectionName));

builder.Services.AddControllers();
builder.Services.AddAutoMapper(cfg => {}, typeof(Program));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSignalR", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins(
                    "http://localhost:3000",
                    "http://localhost:5173",
                    "http://localhost:8080",
                    "http://127.0.0.1:3000",
                    "http://127.0.0.1:5173",
                    "http://127.0.0.1:8080"
                  )
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
        else
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamJoinService, TeamJoinService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IFriendshipService, FriendshipService>();
builder.Services.AddScoped<IPreferanceTagsService, PreferanceTagsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IGameProfileService, GameProfileService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<SqlConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection")!;
    return new SqlConnection(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSignalR");

app.UseAuthentication();
app.UseAuthorization();

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();
app.UseCors("AllowFrontend");


app.MapHub<ChatHub>("/chathub");

app.Run();
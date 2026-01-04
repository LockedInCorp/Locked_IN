using Locked_IN_Backend.Controllers;
using Microsoft.EntityFrameworkCore;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Services;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Hubs;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.Configure<TeamSettings>(
    builder.Configuration.GetSection(TeamSettings.SectionName));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SignalR Configuration
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
});

// CORS Configuration for SignalR
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSignalR", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // In development, allow common localhost origins
            // Note: AllowAnyOrigin() cannot be used with AllowCredentials()
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
            // In production, restrict to specific origins
            policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamJoinService, TeamJoinService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IFriendshipService, FriendshipService>();
builder.Services.AddScoped<ITagService, UserTagService>();
builder.Services.AddScoped<IPreferanceTagsService, PreferanceTagsService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameProfileService, GameProfileService>();

// Chat Service
builder.Services.AddScoped<IChatService, ChatService>();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITagService, TagService>();

builder.Services.AddScoped<IGameProfileService, GameProfileService>();

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

// CORS must be enabled before other middleware
app.UseCors("AllowSignalR");

// Skip HTTPS redirection in development to allow HTTP SignalR connections
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();
app.UseCors("AllowFrontend");


// SignalR Hub endpoint
app.MapHub<ChatHub>("/chathub");

app.Run();
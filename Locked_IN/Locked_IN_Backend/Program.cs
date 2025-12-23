using Locked_IN_Backend.Controllers;
using Microsoft.EntityFrameworkCore;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Services;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.Configure<TeamSettings>(
    builder.Configuration.GetSection(TeamSettings.SectionName));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamJoinService, TeamJoinService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IFriendshipService, FriendshipService>();
builder.Services.AddScoped<ITagService, UserTagService>();
builder.Services.AddScoped<IPreferanceTagsService, PreferanceTagsService>();
builder.Services.AddScoped<IUserService, UserService>();
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

app.UseHttpsRedirection();
app.MapControllers();
app.UseCors("AllowFrontend");


app.Run();
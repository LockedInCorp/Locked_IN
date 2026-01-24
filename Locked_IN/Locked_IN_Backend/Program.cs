
using Microsoft.EntityFrameworkCore;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Locked_IN_Backend.Interfaces;
using FluentValidation;
using Locked_IN_Backend.DTOs.Friendship;
using Locked_IN_Backend.DTOs.Team;
using Locked_IN_Backend.DTOs;
using Locked_IN_Backend.DTOs.User;
using Locked_IN_Backend.Services;
using Locked_IN_Backend.Hubs;
using Locked_IN_Backend.Interfaces.Repositories;
using Locked_IN_Backend.Interfaces.Services;
using Locked_IN_Backend.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Data.SqlClient;
using Minio;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secret.json", optional: true, reloadOnChange: true);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequireDigit = true;
        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!))
        };
    });


builder.Services.AddControllers();

RegisterValidationServices();

builder.Services.AddAutoMapper(cfg => {}, typeof(Program));

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


RegisterServices();

builder.Services.AddMinio(configureSource => configureSource
    .WithEndpoint(builder.Configuration["Minio:Endpoint"])
    .WithCredentials(builder.Configuration["Minio:AccessKey"], builder.Configuration["Minio:SecretKey"])
    .WithSSL(bool.Parse(builder.Configuration["Minio:Secure"] ?? "false"))
    .Build());

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
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.UseCors("AllowSignalR");

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.MapHub<ChatHub>("/chathub");

app.Run();


void RegisterValidationServices()
{
    builder.Services.AddScoped<IValidator<SendFriendRequestDto>, SendFriendRequestValidator>();
    builder.Services.AddScoped<IValidator<FriendshipActionDto>, FriendshipActionValidator>();
    builder.Services.AddScoped<IValidator<BlockUserDto>, BlockUserValidator>();
    builder.Services.AddScoped<IValidator<UnblockUserDto>, UnblockUserValidator>();
    builder.Services.AddScoped<IValidator<AdvancedSearchDto>, AdvancedSearchDtoValidator>();
    builder.Services.AddScoped<IValidator<JoinRequestDto>, JoinRequestDtoValidator>();
    builder.Services.AddScoped<IValidator<RegisterDto>, RegisterDtoValidator>();
    builder.Services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
    builder.Services.AddScoped<IValidator<UpdateUserProfileDto>, UpdateUserProfileDtoValidator>();
    builder.Services.AddScoped<IValidator<UpdateAvailabilityDto>, UpdateAvailabilityDtoValidator>();
}

void RegisterServices()
{
    builder.Services.AddScoped<ITeamRepository, TeamRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITeamMemberRepository, TeamMemberRepository>();
    builder.Services.AddScoped<ITeamService, TeamService>();
    builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
    builder.Services.AddScoped<IGameService, GameService>();
    builder.Services.AddScoped<IFriendshipService, FriendshipService>();
    builder.Services.AddScoped<IPreferanceTagsService, PreferanceTagsService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<ITagService, TagService>();
    builder.Services.AddScoped<IGameProfileService, GameProfileService>();
    builder.Services.AddScoped<IChatService, ChatService>();
    builder.Services.AddScoped<IFileUploadService, MinioFileUploadService>();
    builder.Services.AddScoped<IJwtService, JwtService>();
}
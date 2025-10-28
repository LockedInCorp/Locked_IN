using Microsoft.EntityFrameworkCore;
using Locked_IN_Backend.Data;
using Locked_IN_Backend.Data.Entities;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<SqlConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection")!;
    return new SqlConnection(connectionString);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        
        context.Database.Migrate(); 

        if (!context.MemberStatuses.Any())
        {
            context.MemberStatuses.AddRange(
                new MemberStatus { Id = 1, Statusname = "Leader" },
                new MemberStatus { Id = 2, Statusname = "Member" },
                new MemberStatus { Id = 3, Statusname = "Pending" } 
            );
            context.SaveChanges();
            Console.WriteLine("MemberStatus table seeded.");
        }
        else
        {
            Console.WriteLine("MemberStatus table already contains data.");
        }
    }
    catch (Exception ex) 
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // default at /swagger
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
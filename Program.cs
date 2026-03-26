using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Data;
using MovieTheaterWS_v2.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext
builder.Services.AddDbContext<MovietheaterContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddEntityFrameworkStores<MovietheaterContext>()
    .AddDefaultTokenProviders() // Necessary for password recovery
                                //.AddDefaultUI(); // This adds the Login/Register pages if you need them, not necessary if my Web API (without Microsoft login pages)
    ;


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCredentials", corsPolicyBuilder =>
    {
        //corsPolicyBuilder.WithOrigins("http://localhost:3000")
        corsPolicyBuilder.WithOrigins("https://localhost:3000")
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader();
    }
    );
});



var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Run seeder
        await DbSeeder.SeedRolesAndAdminAsync(services);
    }
    catch(Exception ex)
    {
        //Console.WriteLine($"Error in the Seeder: {ex.Message}");
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred when seeding the database.");
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowCredentials");

// Set requests pipeline
app.UseAuthentication();
app.UseAuthorization();

// Add routes and controllers
app.MapControllers();


app.Run();

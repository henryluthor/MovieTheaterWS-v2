using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTheaterWS_v2.Classes;
using MovieTheaterWS_v2.Data;
using MovieTheaterWS_v2.Models;
using System.Security.Claims;
using System.Text;


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


//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = "Cookies";
//    options.DefaultChallengeScheme = "Cookies";
//});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
{
    // JWT validation setting (SecretKey, etc.)
    options.TokenValidationParameters = new TokenValidationParameters 
    {
        // Validate the token signature
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtBearerData:SecretKey"])),

        // Validate who issued the token (my backend)
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JwtBearerData:Issuer"],

        // Validate who is my token for (my frontend)
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JwtBearerData:Audience"],

        // Validate the token is not expired
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero, // Elimina el margen de 5 min por defecto para pruebas exactas

        // ¡VITAL FOR ADMIN ROLE! 
        // Map the Microsoft long claim name to the Roles system
        //RoleClaimType = "http://microsoft.com" // Short claim name, gives me trouble
        RoleClaimType = ClaimTypes.Role, // This is equivalent to "http://microsoft.com"
        NameClaimType = ClaimTypes.Name
    };

    // The "bridge" to get the token from the cookie
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["token"];
            return Task.CompletedTask;
        }
    };
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

builder.Services.AddAuthorization(options =>
{
    // Creating a policy that demands the "Admin" role
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});


builder.Services.AddScoped<LoginTokenGenerator>();


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

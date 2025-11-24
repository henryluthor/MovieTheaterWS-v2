using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTheaterWS_v2.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//new
builder.Services.AddDbContext<MovietheaterContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = "JwtBearer";
//    options.DefaultChallengeScheme = "JwtBearer";
//})
//.AddJwtBearer("JwtBearer", options =>
//{
//    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = "myIssuer",
//        ValidAudience = "myAudience",
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretkey"))
//    };
//});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
//.AddCookie("Cookies", options =>
//{
//    options.Cookie.HttpOnly = true;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
//    //options.Cookie.SameSite = SameSiteMode.None;
//})
;

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


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Set requests pipeline
app.UseAuthentication();
app.UseAuthorization();

// Add routes and controllers
app.MapControllers();


//app.UseCors(policy => policy.AllowAnyOrigin()
//    .AllowAnyHeader());

app.UseCors("AllowCredentials");

// New line
//app.Use(async (contex, next) =>
//{
//    contex.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
//    contex.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
//    await next();
//});


app.Run();

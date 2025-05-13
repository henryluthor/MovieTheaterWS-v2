using Microsoft.EntityFrameworkCore;
using MovieTheaterWS_v2.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//new
//builder.Services.AddDbContext<MovietheaterContext>(options => options.UseSqlServer("Server=LAPTOP-R601H3RA\\SQLEXPRESS;Database=movietheater;Trusted_Connection=true;TrustedServerCertificate=true;Persist Security Info=true"));
builder.Services.AddDbContext<MovietheaterContext>(options => options.UseSqlServer("Server=LAPTOP-R601H3RA;Database=movietheater;Trusted_Connection=true;TrustedServerCertificate=true;Persist Security Info=true"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//new
app.UseCors(policy => policy.AllowAnyOrigin()
    .AllowAnyHeader());

app.Run();

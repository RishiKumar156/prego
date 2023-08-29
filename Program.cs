using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using MongoDB.Driver;
using System.Net.Sockets;

internal class Program
{
    private static void Main(string[] args)
    {
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IMongoClient, MongoClient>(s =>
{
     var uri = s.GetRequiredService<IConfiguration>()["cluster"];
     return new MongoClient(uri);
});

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        }).AddCookie().
        AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
        {
            options.ClientId = "1067401461794-bg90hatjo67u7m2vn8a9fmv07f86026c.apps.googleusercontent.com";
            options.ClientSecret = "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";
            options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
    }
}
using Company.SampleApi.Api.Endpoints;
using Company.SampleApi.Api.Pages;
using Company.SampleApi.Database;
using Company.SampleApi.OAuthServer;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSampleApiDbContext();
builder.Services.AddOAuthServer();
builder.Services.AddUsers();
builder.Services.AddCors();
builder.Services.AddAntiforgery();
builder.Services.AddAuthentication().AddCookie("oauth_cookie", c => c.LoginPath = "/Login");
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(policy =>
{
    policy.AllowAnyOrigin();
    policy.AllowAnyHeader();
    policy.AllowAnyMethod();
});
app.UseOAuthServer();
app.UseAntiforgery();

app.MapUsers();
app.MapPost("signin", ([FromForm] LoginModel model) => 
{
});
app.MapRazorPages();

app.Run();
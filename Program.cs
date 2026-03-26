using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MunchrBackendV2.Context;
using MunchrBackendV2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<BusinessServices>();
builder.Services.AddScoped<ReviewServices>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");
builder.Services.AddDbContext<DataContext>(Options => Options.UseSqlServer(connectionString));

var secretKey = builder.Configuration["JWT:Key"] ?? "superdupersecret696967@supersecrettopsecretciatypebeat";
var signingCredentials = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
// Add authentication services to the app
builder.Services.AddAuthentication(options =>
{
    // Set the default authentication scheme/ behaviour to JWT Bearer
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    // Set the default challenge scheme (what to use when authentication fails)
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Configure JWT Bearer authentication options
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Check if the token's issuer is valid
        ValidateAudience = true, // Check if the token's audience is valid
        ValidateLifetime = true, // Ensure the token hasn't expired
        ValidateIssuerSigningKey = true, // Check the token's signature is valid

        // The expected issuer (the API that created the token)
        ValidIssuer = "https://csa-2526-munchr-a8dbh8ckfddrewh7.westus3-01.azurewebsites.net/",

        // The expected audience (who the token is intended for)
        ValidAudience = "https://csa-2526-munchr-a8dbh8ckfddrewh7.westus3-01.azurewebsites.net/",

        // The key used to sign the token (must match the one used to create it)
        IssuerSigningKey = signingCredentials
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();

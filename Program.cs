using Microsoft.EntityFrameworkCore;
using CropDeal.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// 1. Add Database support
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Add Controller support (This finds your AuthController)
builder.Services.AddControllers();

// 3. Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
            ValidateAudience = true,
            ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
            ValidateLifetime = true
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();   
    app.UseSwaggerUI();

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();


app.Run();

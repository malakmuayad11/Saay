using Microsoft.OpenApi.Models;
using Saay.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVaultIfConfigured();

builder.Services.AddSaayAuth(builder.Configuration);

builder.Services.AddSaayPolicies();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your JWT token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddSaayRateLimiting();

builder.Services.AddSaayPersistence(builder.Configuration);

builder.Services.AddSaayRepositories();
builder.Services.AddSaayServices();

builder.Services.AddSaayCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("SaayCorsPolicy");

app.UseHttpsRedirection();

app.UseRateLimiter();

// Safe Message for rate limiter
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests)
    {
        await context.Response.WriteAsync("Too many attempts. Please try again later.");
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

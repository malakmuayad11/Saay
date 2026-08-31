using Saay.Extensions;
using Saay.Repository.Classes;
using Saay.Repository.Interfaces;
using Saay.Services.Classes;
using Saay.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureKeyVaultIfConfigured();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSaayPersistence(builder.Configuration);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, ArgonPasswordHasher>();

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

app.Run();

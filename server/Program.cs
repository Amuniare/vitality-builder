using Microsoft.EntityFrameworkCore;
using VitalityBuilder.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Database Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Add repository
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();

// Add authorization services (even if not using authentication yet)
builder.Services.AddAuthorization();

// Configure HTTPS properly
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7034; // Match your launchSettings.json
});




var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Disable HTTPS redirection in development if needed
    // app.UseHttpsRedirection();
}
else
{
    app.UseHttpsRedirection();
}

app.UseRouting();

// Add authentication middleware (if needed later)
// app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
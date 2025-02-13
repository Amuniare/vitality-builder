using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using VitalityBuilder.Api.Infrastructure.Data;
using VitalityBuilder.Api.Infrastructure.Security;
using VitalityBuilder.Api.Infrastructure.Validation;
using VitalityBuilder.Api.Interfaces.Repositories;
using VitalityBuilder.Api.Interfaces.Services;
using VitalityBuilder.Api.Services.Calculations;
using VitalityBuilder.Api.Services.Character;
using VitalityBuilder.Api.Services.Validation;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Vitality Builder API", 
        Version = "v1",
        Description = "API for the Vitality System Character Builder"
    });
    
    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

// Configure Database
builder.Services.AddDbContext<VitalityBuilderContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("VitalityBuilder.Api.Infrastructure")));

// Register Services
builder.Services.AddScoped<ICharacterRepository, CharacterRepository>();
builder.Services.AddScoped<ICharacterCreationService, CharacterCreationService>();
builder.Services.AddScoped<ICharacterUpdateService, CharacterUpdateService>();
builder.Services.AddScoped<ICharacterStatCalculator, CharacterStatCalculator>();
builder.Services.AddScoped<IPointPoolCalculator, PointPoolCalculator>();
builder.Services.AddScoped<IValidationService, ValidationService>();

// Register Validators
builder.Services.AddValidatorsFromAssemblyContaining<CharacterValidator>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<VitalityBuilderContext>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Vitality Builder API V1");
        c.RoutePrefix = string.Empty;
    });
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors();

// Add security headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    context.Response.Headers.Add("Content-Security-Policy", 
        "default-src 'self'; img-src 'self' data:; style-src 'self' 'unsafe-inline';");
    
    await next();
});

// Global error handling
app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();
app.MapHealthChecks("/health");

// Ensure database is created and migrated
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<VitalityBuilderContext>();
    db.Database.Migrate();
}

app.Run();

using Translator.Domain.Interfaces.Repositories;
using Translator.Domain.Interfaces.Services;
using Translator.Infrastructure.AiService;
using Translator.Persistence.Repositories;
using Translator.Application.Services;

namespace Translator.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services.AddOpenApi();

        builder.Services.AddScoped<ITranslationOrchestrator, TranslationOrchestrator>();
        builder.Services.AddScoped<ITranslationJobProcessor, TranslationJobProcessor>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ISessionCleanupService, SessionCleanupService>();

        builder.Services.AddScoped<INotificationService, NotificationService>();

        builder.Services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
        });

        builder.Services.AddHttpClient(nameof(IAiTranslationService));

        builder.Services.AddScoped<ISessionRepository, SessionRepository>();
        builder.Services.AddScoped<IJobRepository, JobRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAiTranslationService, AiTranslationService>();


        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapHub<TranslationHub>("/translationHub");

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapGet("/weatherforecast", (HttpContext httpContext) =>
        {
            var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                })
                .ToArray();
            return forecast;
        })
        .WithName("GetWeatherForecast");

        app.Run();
    }
}

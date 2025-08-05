namespace RabbitMQ.Core.Extensions;

public static class RabbitMqExtensions
{
    public static IServiceCollection AddRabbitMqExtensions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection(nameof(RabbitMqSettings)));

        services.AddSingleton<RabbitMqConnectionService>();

        services.AddScoped<IRabbitMqService, RabbitMqService>();

        return services;
    }
}

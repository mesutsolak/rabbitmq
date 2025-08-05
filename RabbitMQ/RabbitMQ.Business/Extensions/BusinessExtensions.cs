namespace RabbitMQ.Business.Extensions;

public static class BusinessExtensions
{
    public static IServiceCollection AddBusinessExtensions(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}

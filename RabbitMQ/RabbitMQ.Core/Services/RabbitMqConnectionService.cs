namespace RabbitMQ.Core.Services;

public class RabbitMqConnectionService(IOptions<RabbitMqSettings> rabbitMqSettings)
{
    public IModel CreateChannel()
    {
        var factory = new ConnectionFactory()
        {
            HostName = rabbitMqSettings.Value.HostName,
            UserName = rabbitMqSettings.Value.Username,
            Password = rabbitMqSettings.Value.Password
        };

        var connection = factory.CreateConnection();
        return connection.CreateModel();
    }
}

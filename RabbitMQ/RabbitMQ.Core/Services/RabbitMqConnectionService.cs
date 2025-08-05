namespace RabbitMQ.Core.Services;

public sealed class RabbitMqConnectionService(IOptions<RabbitMqSettings> rabbitMqSettings)
{
    public IModel CreateChannel()
    {
        var factory = new ConnectionFactory()
        {
            HostName = rabbitMqSettings.Value.HostName,
            UserName = rabbitMqSettings.Value.Username,
            Password = rabbitMqSettings.Value.Password,
            Port = rabbitMqSettings.Value.Port,
            VirtualHost = rabbitMqSettings.Value.Username,
            Ssl = new SslOption
            {
                Enabled = rabbitMqSettings.Value.Ssl,
                ServerName = rabbitMqSettings.Value.HostName
            }
        };

        var connection = factory.CreateConnection();
        return connection.CreateModel();
    }
}

namespace RabbitMQ.Core.Services;

public sealed class RabbitMqService(RabbitMqConnectionService rabbitMqConnectionService) : IRabbitMqService
{
    public void Send<T>(PublishRequest<T> request) where T : IPayload
    {
        using var channel = rabbitMqConnectionService.CreateChannel();

        channel.ExchangeDeclare(request.Exchange.Name, request.Exchange.Type);

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = request.Exchange.Persistence;
        properties.Headers = request.Exchange.Headers;

        channel.BasicPublish(request.Exchange.Name, request.Exchange.RoutingKey, basicProperties: properties, body: ConvertToByteArray(request.Payload));
    }

    public void SendBasic<T>(PublishRequest<T> request) where T : IPayload
    {
        using var channel = rabbitMqConnectionService.CreateChannel();

        channel.QueueDeclare(request.Queue.Name, request.Queue.Durable, request.Queue.Exclusive, request.Queue.AutoDelete);

        IBasicProperties properties = channel.CreateBasicProperties();
        properties.Persistent = request.Exchange.Persistence;

        channel.BasicPublish(request.Exchange.Name, request.Exchange.RoutingKey, basicProperties: properties, body: ConvertToByteArray(request.Payload));
    }

    private static byte[] ConvertToByteArray<T>(T message) where T : IPayload => JsonSerializer.SerializeToUtf8Bytes(message);
}

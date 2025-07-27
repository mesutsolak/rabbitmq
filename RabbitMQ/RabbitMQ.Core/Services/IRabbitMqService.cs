namespace RabbitMQ.Core.Services;

public interface IRabbitMqService
{
    void Send<T>(PublishRequest<T> request) where T : IPayload;
    void SendBasic<T>(PublishRequest<T> request) where T : IPayload;
}

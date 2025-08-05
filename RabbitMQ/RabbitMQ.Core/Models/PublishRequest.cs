namespace RabbitMQ.Core.Models;

public sealed record PublishRequest<T>(T value) where T : IPayload
{
    public T Payload { get; init; } = value;
    public QueueModel Queue { get; init; } = default!;
    public ExchangeModel Exchange { get; init; } = default!;
}

public sealed record QueueModel
{
    public string Name { get; init; } = string.Empty;
    public bool Durable { get; init; }
    public bool Exclusive { get; init; }
    public bool AutoDelete { get; init; }
}

public sealed record ExchangeModel
{
    public string Name { get; init; } = string.Empty;
    public string? Type { get; init; }
    public string RoutingKey { get; init; } = string.Empty;
    public bool Persistence { get; init; }
    public Dictionary<string, object>? Headers { get; set; }
}
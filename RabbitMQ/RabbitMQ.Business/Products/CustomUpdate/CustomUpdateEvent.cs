namespace RabbitMQ.Business.Products.CustomUpdate;

public sealed record CustomUpdateEvent : IPayload
{
    public int Id { get; init; }
}

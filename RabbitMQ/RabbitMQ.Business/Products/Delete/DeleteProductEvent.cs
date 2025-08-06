namespace RabbitMQ.Business.Products.Delete;

public sealed record DeleteProductEvent : IPayload
{
    public int Id { get; init; }
}

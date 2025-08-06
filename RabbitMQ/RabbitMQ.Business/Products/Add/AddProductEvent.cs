namespace RabbitMQ.Business.Products.Add;

public sealed record AddProductEvent : IPayload
{
    public required string Name { get; init; }
    public required int Stock { get; init; }
}

namespace RabbitMQ.Business.Products.Add;

public sealed record AddProductCommand : IRequest<Unit>
{
    public required string Name { get; init; }
    public required int Stock { get; init; }
}

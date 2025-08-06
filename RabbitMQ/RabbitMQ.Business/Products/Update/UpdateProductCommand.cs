namespace RabbitMQ.Business.Products.Update;

public sealed record UpdateProductCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required int Stock { get; init; }
}

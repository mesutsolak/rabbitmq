namespace RabbitMQ.Business.Products.Delete;

public sealed record DeleteProductCommand : IRequest<Unit>
{
    public int Id { get; init; }
}

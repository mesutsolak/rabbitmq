namespace RabbitMQ.Business.Products.UpdateStock;

public sealed record UpdateProductStockCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public required int Stock { get; init; }
}

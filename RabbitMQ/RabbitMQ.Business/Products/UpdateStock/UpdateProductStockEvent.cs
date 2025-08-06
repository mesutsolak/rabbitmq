namespace RabbitMQ.Business.Products.UpdateStock;

public sealed record UpdateProductStockEvent : IPayload
{
    public int Id { get; init; }
    public required int Stock { get; init; }
}

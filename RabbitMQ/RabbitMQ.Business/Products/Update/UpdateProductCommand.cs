namespace RabbitMQ.Business.Products.Update;

public sealed class UpdateProductCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

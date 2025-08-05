namespace RabbitMQ.Business.Products.Add;

public sealed class AddProductCommand : IRequest<Unit>
{
    public required string Name { get; set; }
}

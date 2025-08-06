namespace RabbitMQ.Business.Products.Add;

public class AddProductEvent : IPayload
{
    public required string Name { get; set; }
}

namespace RabbitMQ.Business.Products.Update;

public class UpdateProductEvent : IPayload
{
    public int Id { get; set; }

    public required string Name { get; set; }
}
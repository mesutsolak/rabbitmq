using RabbitMQ.Core.Models;

namespace RabbitMQ.Business.Products.Models;

public class AddProductEvent : IPayload
{
    public required string Name { get; set; }
}

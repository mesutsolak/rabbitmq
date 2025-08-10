namespace RabbitMQ.Business.Products.CustomDelete;

public sealed record CustomDeleteEvent : IPayload
{
    public int Id { get; init; }
}

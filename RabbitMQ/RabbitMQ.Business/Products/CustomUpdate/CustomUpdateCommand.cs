namespace RabbitMQ.Business.Products.CustomUpdate;

public sealed record CustomUpdateCommand : IRequest<Unit>
{
    public int Id { get; init; }
}

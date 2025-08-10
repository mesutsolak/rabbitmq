namespace RabbitMQ.Business.Products.CustomDelete;

public sealed record CustomDeleteCommand : IRequest<Unit>
{
    public int Id { get; init; }
}

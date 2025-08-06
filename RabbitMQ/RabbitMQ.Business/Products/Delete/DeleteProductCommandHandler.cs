namespace RabbitMQ.Business.Products.Delete;

public sealed class DeleteProductCommandHandler(IRabbitMqService service) : IRequestHandler<DeleteProductCommand, Unit>
{
    private readonly IRabbitMqService _service = service;

    public async Task<Unit> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var request = PrepareRequest(command);

        _service.Send(request);

        return await Task.FromResult(Unit.Value);
    }

    // Direct Exchange
    private static PublishRequest<DeleteProductEvent> PrepareRequest(DeleteProductCommand command)
    {
        var @event = new DeleteProductEvent
        {
            Id = command.Id
        };

        return new PublishRequest<DeleteProductEvent>(@event)
        {
            Exchange = new()
            {
                Name = "delete-product-stock",
                Type = ExchangeType.Direct,
                Persistence = true
            }
        };
    }
}

namespace RabbitMQ.Business.Products.UpdateStock;

public sealed class UpdateProductStockCommandHandler(IRabbitMqService service) : IRequestHandler<UpdateProductStockCommand, Unit>
{
    private readonly IRabbitMqService _service = service;

    public async Task<Unit> Handle(UpdateProductStockCommand command, CancellationToken cancellationToken)
    {
        var request = PrepareRequest(command);

        _service.Send(request);

        return await Task.FromResult(Unit.Value);
    }

    // Fanout Exchange
    private static PublishRequest<UpdateProductStockEvent> PrepareRequest(UpdateProductStockCommand command)
    {
        var @event = new UpdateProductStockEvent
        {
            Id = command.Id,
            Stock = command.Stock,
        };

        return new PublishRequest<UpdateProductStockEvent>(@event)
        {
            Exchange = new()
            {
                Name = "update-product-stock",
                Type = ExchangeType.Fanout,
                Persistence = true
            }
        };
    }
}

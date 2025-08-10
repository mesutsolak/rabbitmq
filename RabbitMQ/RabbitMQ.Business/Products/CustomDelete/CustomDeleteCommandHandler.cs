namespace RabbitMQ.Business.Products.CustomDelete;

public sealed class CustomDeleteCommandHandler(IRabbitMqService service) : IRequestHandler<CustomDeleteCommand, Unit>
{
    private readonly IRabbitMqService _service = service;

    public async Task<Unit> Handle(CustomDeleteCommand command, CancellationToken cancellationToken)
    {
        var request = PrepareRequest(command);

        _service.Send(request);

        return await Task.FromResult(Unit.Value);
    }

    // Topic Exchange
    private static PublishRequest<CustomDeleteEvent> PrepareRequest(CustomDeleteCommand command)
    {
        var @event = new CustomDeleteEvent
        {
            Id = command.Id
        };

        return new PublishRequest<CustomDeleteEvent>(@event)
        {
            Exchange = new()
            {
                Name = "custom-delete-product-exchange",
                RoutingKey = $"product.item.{command.Id}",
                Type = ExchangeType.Topic,
                Persistence = true
            }
        };
    }
}

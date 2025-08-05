namespace RabbitMQ.Business.Products.Add;

public sealed class AddProductCommandHandler(IRabbitMqService service) : IRequestHandler<AddProductCommand, Unit>
{
    private readonly IRabbitMqService _service = service;

    public async Task<Unit> Handle(AddProductCommand command, CancellationToken cancellationToken)
    {
        var request = PrepareRequest(command);

        _service.SendBasic(request);

        return await Task.FromResult(Unit.Value);
    }

    private static PublishRequest<AddProductEvent> PrepareRequest(AddProductCommand command)
    {
        var @event = new AddProductEvent
        {
            Name = command.Name,
        };

        return new PublishRequest<AddProductEvent>(@event)
        {
            Queue = new()
            {
                Name = "add-product",
                Durable = false,
                Exclusive = false,
                AutoDelete = false
            },
            Exchange = new()
            {
                RoutingKey = "add-product"
            }
        };
    }
}
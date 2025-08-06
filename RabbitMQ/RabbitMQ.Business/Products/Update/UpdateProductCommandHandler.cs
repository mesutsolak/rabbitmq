namespace RabbitMQ.Business.Products.Update;

public sealed class UpdateProductCommandHandler(IRabbitMqService service) : IRequestHandler<UpdateProductCommand, Unit>
{
    private readonly IRabbitMqService _service = service;

    public async Task<Unit> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var request = PrepareRequest(command);

        _service.SendBasic(request);

        return await Task.FromResult(Unit.Value);
    }

    // Secure Queue
    private static PublishRequest<UpdateProductEvent> PrepareRequest(UpdateProductCommand command)
    {
        var @event = new UpdateProductEvent
        {
            Name = command.Name,
            Id = command.Id,
        };

        return new PublishRequest<UpdateProductEvent>(@event)
        {
            Queue = new()
            {
                Name = "update-product",
                Durable = true,
                Exclusive = false,
                AutoDelete = false
            },
            Exchange = new()
            {
                RoutingKey = "update-product",
                Persistence = true
            }
        };
    }
}
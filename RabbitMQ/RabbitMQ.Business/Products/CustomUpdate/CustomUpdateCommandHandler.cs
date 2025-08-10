namespace RabbitMQ.Business.Products.CustomUpdate;

public class CustomUpdateCommandHandler(IRabbitMqService service) : IRequestHandler<CustomUpdateCommand, Unit>
{
    private readonly IRabbitMqService _service = service;

    public async Task<Unit> Handle(CustomUpdateCommand command, CancellationToken cancellationToken)
    {
        var request = PrepareRequest(command);

        _service.Send(request);

        return await Task.FromResult(Unit.Value);
    }

    // Headers Exchange
    private static PublishRequest<CustomUpdateEvent> PrepareRequest(CustomUpdateCommand command)
    {
        var @event = new CustomUpdateEvent
        {
            Id = command.Id
        };

        return new PublishRequest<CustomUpdateEvent>(@event)
        {
            Exchange = new()
            {
                Name = "custom-update-product-exchange",
                Type = ExchangeType.Headers,
                Persistence = true,
                Headers = new()
                {
                    ["id"] = command.Id
                }
            }
        };
    }
}

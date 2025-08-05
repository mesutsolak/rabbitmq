namespace RabbitMQ.Api.Controllers;

public sealed class ProductsController(IMediator mediator) : CustomBaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Add(AddProductCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }
}

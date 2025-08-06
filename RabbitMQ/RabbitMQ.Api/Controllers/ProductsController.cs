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

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProductCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }

    [HttpPatch("UpdateStock")]
    public async Task<IActionResult> UpdateStock(UpdateProductStockCommand command)
    {
        await _mediator.Send(command);

        return Ok();
    }
}

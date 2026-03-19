using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace HCG.FondoRevolvente.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolicitudesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SolicitudesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        return Ok();
    }
}

using System;
using System.Threading.Tasks;
using Application.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IMediator _Mediator;

        public JobController(IMediator mediator)
        {
            _Mediator = mediator;
        }

        [HttpPost]
        [Route("sync")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SyncDatabase([FromBody] DateTime lastExecution)
        {
            var command = new SyncDatabaseCommand(lastExecution);
            await _Mediator.Send(command);

            return Ok();
        }
    }
}
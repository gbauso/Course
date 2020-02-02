using System.Threading.Tasks;
using Domain.Service;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _Mediator;

        public CourseController(IMediator mediator)
        {
            _Mediator = mediator;
        }

        [HttpPost]
        [Route("enroll")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnrollCourse([FromBody] CourseEnrollmentCommand command)
        {
            await command.Validate();
            await _Mediator.Send(command);

            return StatusCode(201);
        }
    }
}
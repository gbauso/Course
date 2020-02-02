using System;
using System.Threading.Tasks;
using Application.Command;
using Application.Query;
using Infrastructure.Database.Query.Model;
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
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EnrollCourse([FromBody] EnrollmentRequestCommand command)
        {
            await command.Validate();
            await _Mediator.Send(command);

            return Accepted();
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SimpleCourse[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourses()
        {
            var list = await _Mediator.Send(new CourseListQuery());

            return Ok(list);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Course))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCourse([FromRoute] Guid id)
        {
            var course = await _Mediator.Send(new CourseQuery(id));

            return Ok(course);
        }
    }
}
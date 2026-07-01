using Microsoft.AspNetCore.Mvc;
using School.Exams.Api.Routes;
using School.Exams.Application.Abstractions;
using School.Exams.Domain.Models.Requests;
using School.Exams.Domain.Models.Responses;

namespace School.Exams.Api.Controllers;

[ApiController]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    [HttpPost(SchoolRoutes.Student.Main)]
    [ProducesResponseType(typeof(RegisterStudentResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterStudentResponse>> Register(
        [FromBody] RegisterStudentRequest request,
        CancellationToken cancellationToken)
    {
        var response = await studentService.RegisterAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet(SchoolRoutes.Student.Index)]
    [ProducesResponseType(typeof(IReadOnlyList<RegisterStudentResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RegisterStudentResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var response = await studentService.GetAllAsync(cancellationToken);
        return Ok(response);
    }
}

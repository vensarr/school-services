using Microsoft.AspNetCore.Mvc;
using School.Exams.Application.Abstractions;
using School.Exams.Api.Routes;
using School.Exams.Domain.Models.Requests;
using School.Exams.Domain.Models.Responses;

namespace School.Exams.Api.Controllers;

[ApiController]
public class SubjectsController(ISubjectService subjectService) : ControllerBase
{
    [HttpPost(SchoolRoutes.Subject.Main)]
    [ProducesResponseType(typeof(RegisterSubjectResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterSubjectResponse>> Register(
        [FromBody] RegisterSubjectRequest request,
        CancellationToken cancellationToken)
    {
        var response = await subjectService.RegisterAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet(SchoolRoutes.Subject.Index)]
    [ProducesResponseType(typeof(IReadOnlyList<RegisterSubjectResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RegisterSubjectResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var response = await subjectService.GetAllAsync(cancellationToken);
        return Ok(response);
    }
}
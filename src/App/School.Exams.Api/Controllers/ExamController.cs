using Microsoft.AspNetCore.Mvc;
using School.Exams.Api.Routes;
using School.Exams.Application.Abstractions;
using School.Exams.Domain.Models.Requests;
using School.Exams.Domain.Models.Responses;

namespace School.Exams.Api.Controllers;

[ApiController]
public class ExamController(IExamService examService) : ControllerBase
{
    [HttpPost(SchoolRoutes.Exam.Main)]
    [ProducesResponseType(typeof(RegisterExamResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterExamResponse>> Register(
        [FromBody] RegisterExamRequest request,
        CancellationToken cancellationToken)
    {
        var response = await examService.RegisterAsync(request, cancellationToken);
        return Ok(response);
    }

    [HttpGet(SchoolRoutes.Exam.Index)]
    [ProducesResponseType(typeof(IReadOnlyList<RegisterExamResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RegisterExamResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var response = await examService.GetAllAsync(cancellationToken);
        return Ok(response);
    }
}

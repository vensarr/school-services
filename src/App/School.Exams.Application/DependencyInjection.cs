using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using School.Exams.Application.Abstractions;
using School.Exams.Application.Services;
using School.Exams.Application.Validators;

namespace School.Exams.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterSubjectValidator>(ServiceLifetime.Scoped);
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IExamService, ExamService>();
        return services;
    }
}

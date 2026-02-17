using FluentValidation;
using TaskManagement.Application.DTOs.Project;

namespace TaskManagement.Application.Validators.Project
{
    public class AssignMemberDtoValidator : AbstractValidator<AssignMemberDto>
    {
        public AssignMemberDtoValidator()
        {
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project id is required");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User id is required");
        }
    }
}
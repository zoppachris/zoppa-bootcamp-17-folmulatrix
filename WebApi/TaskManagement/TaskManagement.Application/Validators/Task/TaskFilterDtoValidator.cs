using FluentValidation;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Validators.Task
{
    public class TaskFilterDtoValidator : AbstractValidator<TaskFilterDto>
    {
        public TaskFilterDtoValidator()
        {
            RuleFor(x => x.SearchTerm)
                .MaximumLength(100).WithMessage("Search term cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.SearchTerm));

            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100");

            When(x => x.Status.HasValue, () =>
            {
                RuleFor(x => x.Status!.Value)
                    .IsInEnum().WithMessage("Invalid status value");
            });

            When(x => x.Priority.HasValue, () =>
            {
                RuleFor(x => x.Priority!.Value)
                    .IsInEnum().WithMessage("Invalid priority value");
            });

            When(x => x.DueDateFrom.HasValue && x.DueDateTo.HasValue, () =>
            {
                RuleFor(x => x.DueDateFrom)
                    .LessThanOrEqualTo(x => x.DueDateTo)
                    .WithMessage("Due date from must be less than or equal to due date to");
            });
        }
    }
}
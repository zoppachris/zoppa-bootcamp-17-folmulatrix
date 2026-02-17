using FluentValidation;
using TaskManagement.Application.DTOs.Account;

namespace TaskManagement.Application.Validators.Account
{
    public class RefreshTokenDtoValidator : AbstractValidator<RefreshTokenDto>
    {
        public RefreshTokenDtoValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required");
        }
    }
}
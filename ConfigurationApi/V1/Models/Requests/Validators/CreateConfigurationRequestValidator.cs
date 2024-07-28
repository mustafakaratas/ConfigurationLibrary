using FluentValidation;

namespace ConfigurationApi.V1.Models.Requests.Validators;

public class CreateConfigurationRequestValidator : AbstractValidator<CreateConfigurationRequest>
{
    public CreateConfigurationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name can not be empty");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type can not be empty");

        RuleFor(x => x.Value)
            .NotEmpty()
            .WithMessage("Value can not be empty");

        RuleFor(x => x.ApplicationName)
            .NotEmpty()
            .WithMessage("ApplicationName can not be empty");
    }
}
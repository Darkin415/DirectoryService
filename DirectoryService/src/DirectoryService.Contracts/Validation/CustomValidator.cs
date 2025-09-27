using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Errors;
using FluentValidation;
using FluentValidation.Results;

namespace DirectoryService.Contracts.Validation;

public static class CustomValidator
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            Result<TValueObject, Error> result = factoryMethod(value);
            if (result.IsSuccess)
                return;

            context.AddFailure(result.Error.Serialize());
        });
    }

    public static ErrorList ToErrors(this IEnumerable<ValidationFailure> failures)
    {
        return new ErrorList(failures.Select(failure => Error.Deserialize(failure.ErrorMessage)));
    }
    
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> rule, Error error)
    {
        return rule.WithMessage(error.Serialize());
    }
}
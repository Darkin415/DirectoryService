using CSharpFunctionalExtensions;
using DirectoryService.Contacts.Errors;
using FluentValidation;
using FluentValidation.Results;

namespace DirectoryService.Contacts.Validation;

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
}
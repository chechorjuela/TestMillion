using FluentValidation.Results;
using TestMillion.Application.Common.Response;

namespace TestMillion.Application.Common.Exceptions.Validation;

public class ValidationException : Exception
{
    public List<ValidationError> Errors { get; }

    public ValidationException() : base("One or more validation errors occurred.")
    {
        Errors = new List<ValidationError>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .Select(f => new ValidationError 
            { 
                Field = f.PropertyName, 
                Message = f.ErrorMessage 
            })
            .ToList();
    }
}
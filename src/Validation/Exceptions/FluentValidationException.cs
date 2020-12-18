using FluentValidation.Results;

namespace BlitzFramework.Validation.Exceptions
{
    public class FluentValidationException : System.Exception
    {
        public ValidationResult ValidationResult { get; set; }
    }
}
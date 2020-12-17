using System;
using FluentValidation.Results;

namespace BlitzFramework.Validators
{
    public class FluentValidationException : Exception
    {
        public ValidationResult ValidationResult { get; set; }
    }
}
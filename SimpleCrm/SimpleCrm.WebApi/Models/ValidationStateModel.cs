using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace SimpleCrm.WebApi.Models
{
    public class ValidationStateModel
    {
        public string Messages { get; set; }
        public List<ValidationError> Errors { get; set; }

        public ValidationStateModel() { }
        public ValidationStateModel(string genericError)
        {
            Messages = genericError;
        }
        public ValidationStateModel(IEnumerable<string> genericErrors)
        {
            Messages = string.Join('.', genericErrors.Distinct());
        }
        public ValidationStateModel(IEnumerable<ValidationError> errors)
        {
            Errors = errors.ToList();
        }
        public ValidationStateModel(ModelStateDictionary modelState)
        {
            var genericErrors = modelState.Keys
                .Where(key => string.IsNullOrWhiteSpace(key))
                .Select(key => modelState[key].Errors.Select(x => x.ErrorMessage))
                .ToList();

            Messages = genericErrors.Count == 0 ? "Validation failed"
                : string.Join(".", genericErrors.Distinct());
            Errors = modelState.Keys
                .Where(key => !string.IsNullOrWhiteSpace(key))
                .SelectMany(key => modelState[key].Errors
                    .Select(x => new ValidationError { Field = key, Message = x.ErrorMessage }))
                .ToList();
        }
    }
}

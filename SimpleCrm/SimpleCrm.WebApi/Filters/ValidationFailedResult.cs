using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SimpleCrm.WebApi.Models;
using System.Collections.Generic;

namespace SimpleCrm.WebApi.Filters
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ValidationStateModel(modelState))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
        public ValidationFailedResult(IEnumerable<ValidationError> errors)
            : base(new ValidationStateModel(errors))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
        public ValidationFailedResult(string error)
            : base(new ValidationStateModel(error))
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}

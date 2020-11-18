using System.Collections.Generic;

namespace SimpleCrm
{
    public class ValidationState
    {
        public List<ValidationError> Errors { get; set; }
        public ValidationState()
        {
            Errors = new List<ValidationError>();
        }
    }
}

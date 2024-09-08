using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StoreHub.Validations
{
    public static class ModelStateExtentions
    {
        public static List<string> GetModelStateErrors(this ModelStateDictionary modelState)
        {
            var Errors = new List<string>();
            foreach(var state in modelState)
            {
                var stateErrors = state.Value.Errors;
                if(stateErrors.Count > 0)
                {
                    foreach(var error in stateErrors)
                        Errors.Add(error.ErrorMessage);
                }
            }
            return Errors;
        }
    }
}

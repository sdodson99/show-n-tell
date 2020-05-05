using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShowNTell.API.Models.Results
{
    public class ForbiddenObjectResult : IActionResult
    {
        private readonly ObjectResult _objectResult;

        public ForbiddenObjectResult(object data)
        {
            _objectResult = new ObjectResult(data)
            {
                StatusCode = 403
            };
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            return _objectResult.ExecuteResultAsync(context);
        }
    }
}
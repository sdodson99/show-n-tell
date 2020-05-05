using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShowNTell.API.Models;
using ShowNTell.API.Models.Responses;
using ShowNTell.API.Models.Results;

namespace ShowNTell.API.Filters.AccessModes
{
    public class RequireWriteAccessModeFilter : IResourceFilter
    {
        private readonly bool _writeAccessMode;

        public RequireWriteAccessModeFilter(bool writeAccessMode)
        {
            _writeAccessMode = writeAccessMode;
        }

        public void OnResourceExecuted(ResourceExecutedContext context){}

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if(!_writeAccessMode)
            {
                context.Result = new ForbiddenObjectResult(new ErrorResponse()
                {
                    Code = ErrorCode.WRITE_MODE_DISABLED,
                    Message = "Show 'N Tell is not in write mode."
                });
            }
        }
    }
}
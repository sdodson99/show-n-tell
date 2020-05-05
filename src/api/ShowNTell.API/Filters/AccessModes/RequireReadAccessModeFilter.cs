using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShowNTell.API.Models;
using ShowNTell.API.Models.Responses;

namespace ShowNTell.API.Filters.AccessModes
{
    public class RequireReadAccessModeFilter : IResourceFilter
    {
        private readonly bool _readAccessMode;

        public RequireReadAccessModeFilter(bool readAccessMode)
        {
            _readAccessMode = readAccessMode;
        }

        public void OnResourceExecuted(ResourceExecutedContext context){}

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if(!_readAccessMode)
            {
                context.Result = new BadRequestObjectResult(new ErrorResponse()
                {
                    Code = ErrorCode.READ_MODE_DISABLED,
                    Message = "Show 'N Tell is not in read mode."
                });
            }
        }
    }
}
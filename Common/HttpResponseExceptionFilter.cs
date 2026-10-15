using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace PetsApi.Common
{
    public class ErrorResponse
    {
        public string error { get; set; }
        public ErrorResponse(Exception ex)
        {
            string message = ex.Message;
            if (typeof(DbUpdateException).IsInstanceOfType(ex))
            {
                var exception = (DbUpdateException)ex;
                message = exception.InnerException.Message;
            }
            this.error = message;
        }
    }

    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                context.Result = new ObjectResult(new ErrorResponse(context.Exception)) {
                    StatusCode = 500
                };
                context.ExceptionHandled = true;
            }
        }
    }
}

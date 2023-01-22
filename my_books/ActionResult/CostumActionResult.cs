using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.ActionResult
{
    public class CostumActionResult : IActionResult
    {
        private readonly CustomActionResultVM _result;

        public CostumActionResult(CustomActionResultVM result)
        {
            _result = result;
        }
        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_result.Exception ?? _result.Publisher as object)
            {
                StatusCode = _result.Exception != null ? StatusCodes.Status500InternalServerError :
                StatusCodes.Status200OK
            };

            await objectResult.ExecuteResultAsync(context);

        }
    }
}

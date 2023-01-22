using my_books.Data.ViewModels;

namespace my_books.Controllers
{
    internal class CustomActionResult
    {
        private CustomActionResultVM responseObj;

        public CustomActionResult(CustomActionResultVM responseObj)
        {
            this.responseObj = responseObj;
        }
    }
}
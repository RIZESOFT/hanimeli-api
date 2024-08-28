using HanimeliApp.Application.Models;

namespace HanimeliApp.Application.Exceptions
{
    public class InformationException : ExceptionBase
    {
        public InformationException(int code, string? exceptionMessage = null) : base(ExceptionCategories.Information, code, exceptionMessage)
        {
        }
    }
}

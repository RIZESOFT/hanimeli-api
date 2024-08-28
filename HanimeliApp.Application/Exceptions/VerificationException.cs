using HanimeliApp.Application.Models;

namespace HanimeliApp.Application.Exceptions
{
    public class VerificationException : ExceptionBase
    {
        public VerificationException(int code, string? exceptionMessage = null) : base(ExceptionCategories.Verification, code, exceptionMessage)
        {
        }
    }

    public class VerificationExceptions
    {
        public static VerificationException BadRequest => new VerificationException(0001, "Hatalı istek");
	}
}

using HanimeliApp.Application.Models;

namespace HanimeliApp.Application.Exceptions
{
    public class AuthenticationException : ExceptionBase
    {
        public AuthenticationException(int code, string? exceptionMessage = null) : base(ExceptionCategories.Authentication, code, exceptionMessage)
        {
        }
    }

    public class AuthenticationExceptions
    {
        public static AuthenticationException UserInvalidException => new AuthenticationException(0001, "Kullanıcı adı veya şifre yanlış.");
    }
}

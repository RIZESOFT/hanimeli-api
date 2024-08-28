using HanimeliApp.Application.Models;

namespace HanimeliApp.Application.Exceptions
{
    public class InternalException : ExceptionBase
    {
        public InternalException(int code, string? exceptionMessage = null, Exception? innerException = null) : base(ExceptionCategories.InternalError, code, exceptionMessage, innerException)
        {
        }
    }

    public class InternalExceptions
    {
		public static InternalException Unknown(Exception? innerException = null) => new InternalException(0001, "Kategorize edilmemiş hata", innerException: innerException);
        public static InternalException OutOfMemory(Exception? innerException = null) => new InternalException(0002, "Yetersiz bellek hatası", innerException: innerException);
        public static InternalException AuditCanNotUpdateOnCtaComplete(Exception? innerException = null) => new InternalException(1001, "Denetim cta complete edildiğinde güncellenemedi", innerException: innerException);
    }
}

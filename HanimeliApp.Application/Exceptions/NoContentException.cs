using HanimeliApp.Application.Models;

namespace HanimeliApp.Application.Exceptions
{
    public class NoContentException : ExceptionBase
    {
        public NoContentException(int code, string? exceptionMessage = null) : base(ExceptionCategories.NoContent, code, exceptionMessage)
        {
        }
	}

	public class NoContentExceptions
	{
		public static NoContentException RecordNotFound => new NoContentException(0001, "Kayıt bulunamadı");
	}
}

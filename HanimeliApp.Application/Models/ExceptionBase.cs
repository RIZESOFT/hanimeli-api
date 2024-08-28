
using HanimeliApp.Application.Exceptions;
using HanimeliApp.Application.Utilities;

namespace HanimeliApp.Application.Models
{
    public enum ExceptionCategories
    {
        Authentication = 10, //401
        Authorization = 11, //403
        Validation = 12, //400
        Verification = 13, //400

        NoContent = 20, //204
        Information = 21, //200

        InternalError = 30, //500
    }

    public abstract class ExceptionBase : Exception
    {
        public ExceptionCategories Category { get; init; }
        public int Code { get; init; }
        public string? DisplayMessage { get; init; }

		public ExceptionBase(ExceptionCategories category, int code, string? exceptionMessage = null, Exception? innerException = null) : base(exceptionMessage, innerException)
        {
            Category = category;
            Code = code;
            DisplayMessage = Localizer.Instance.Translate($"EXC_{this.GetType().Name}_{code.ToString().PadLeft(4, '0')}") ?? Localizer.Instance.Translate($"EXC_{this.GetType().Name}");
        }


        public string ErrorCode
        {
            get
            {
                return ((int)Category).ToString().PadRight(2, '0') + Code.ToString().PadLeft(4, '0');
            }
        }

        public static ExceptionBase FromException(Exception ex)
        {
            return ex switch
            {
                ExceptionBase eb => eb,
                OutOfMemoryException => InternalExceptions.OutOfMemory(ex),
                _ => InternalExceptions.Unknown(ex)
            };
        }
    }
}

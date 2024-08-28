using HanimeliApp.Application.Models;

namespace HanimeliApp.Application.Exceptions
{
    public class ValidationException : ExceptionBase
    {
        public ValidationException(int code, string? exceptionMessage = null) : base(ExceptionCategories.Validation, code, exceptionMessage)
        {
        }
	}

	public class ValidationExceptions
	{
		// Generic hatalar 0 ile başlar
		public static ValidationException InvalidEmail => new ValidationException(0001, "Geçersiz email");
		public static ValidationException InvalidUser => new ValidationException(0002, "Geçersiz kullanıcı");
		public static ValidationException RecordNotFound => new ValidationException(0003, "Kayıt bulunamadı");
		public static ValidationException InvalidCompany => new ValidationException(0004, "Geçersiz firma");
		public static ValidationException InvalidDateTime => new ValidationException(0005, "Geçersiz tarih-saat");
        public static ValidationException InvalidType => new ValidationException(0006, "Geçersiz tip");
        public static ValidationException InvalidId => new ValidationException(0007, "Geçersiz Id");
        public static ValidationException InvalidSalesPoint => new ValidationException(0008, "Geçersiz Sales point");
		public static ValidationException RequestRequired => new ValidationException(0009, "İstek boş olamaz");
		public static ValidationException InvalidLanguage => new ValidationException(0010, "Geçersiz dil");
		public static ValidationException InvalidVersion => new ValidationException(0011, "Geçersiz versiyon");
		public static ValidationException UserAlreadyExists => new ValidationException(0012, "Böyle bir kullanıcı mevcut");




		// Specific hatalar 1 ile başlar
		public static ValidationException RemarkIsMandatory => new ValidationException(1001, "Remark alanı zorunludur");
		public static ValidationException StartDateCanNotBeGreaterThanEndDate => new ValidationException(1002, "StartDate, EndDate'ten büyük olamaz");
		public static ValidationException DateCanNotBeEarlierThanToday => new ValidationException(1003, "Tarih bugünden eski olamaz");
		public static ValidationException InvalidStatus => new ValidationException(1004, "Geçersiz status");
        public static ValidationException NoteRequired => new ValidationException(1005, "Not alanı zorunludur");
        public static ValidationException VersionNotFound => new ValidationException(1006, "Versiyon bulunamadı");
        public static ValidationException VersionNotesNotFound => new ValidationException(1007, "Versiyon notları bulunamadı");
        public static ValidationException VersionNotesDefaultLangugeNotFound => new ValidationException(1008, "Versiyon notları varsayılan dil bulunamadı");
	}
}

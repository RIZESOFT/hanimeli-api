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
		public static ValidationException CartNotFound => new ValidationException(0004, "Sepet bulunamadı");
		public static ValidationException InvalidDateTime => new ValidationException(0005, "Geçersiz tarih-saat");
        public static ValidationException InvalidType => new ValidationException(0006, "Geçersiz tip");
        public static ValidationException InvalidId => new ValidationException(0007, "Geçersiz Id");
        public static ValidationException InvalidSalesPoint => new ValidationException(0008, "Geçersiz Sales point");
		public static ValidationException RequestRequired => new ValidationException(0009, "İstek boş olamaz");
		public static ValidationException InvalidLanguage => new ValidationException(0010, "Geçersiz dil");
		public static ValidationException InvalidVersion => new ValidationException(0011, "Geçersiz versiyon");
		public static ValidationException UserAlreadyExists => new ValidationException(0012, "Böyle bir kullanıcı mevcut");




		// Order Validations
		public static ValidationException OrderUserRequired => new ValidationException(1001, "Kullanıcı alanı zorunludur");
		public static ValidationException OrderItemsRequired => new ValidationException(1002, "Siparişteki ürünler boş olamaz!");
		public static ValidationException OrderItemsCookRequired => new ValidationException(1003, "Aşçı alanı zorunludur");
		public static ValidationException OrderItemsMenuRequired => new ValidationException(1004, "Siparişteki ürünlerde menü alanı zorunludur");
		public static ValidationException StartDateCanNotBeGreaterThanEndDate => new ValidationException(1002, "StartDate, EndDate'ten büyük olamaz");
		public static ValidationException DateCanNotBeEarlierThanToday => new ValidationException(1003, "Tarih bugünden eski olamaz");
		
	}
}

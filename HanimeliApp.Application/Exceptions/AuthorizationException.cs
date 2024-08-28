using HanimeliApp.Application.Models;

namespace HanimeliApp.Application.Exceptions
{
    public class AuthorizationException : ExceptionBase
    {
        public AuthorizationException(int code, string? exceptionMessage = null) : base(ExceptionCategories.Authorization, code, exceptionMessage)
        {
        }
    }

    public class AuthorizationExceptions 
    {
        public static AuthorizationException UserNotAuthorizedException => new AuthorizationException(0001, "Kullanıcı SoUser veya SuperUser değil ya da salespoint hiyerarşide değil");
		public static AuthorizationException NotPermittedCompany => new AuthorizationException(0002, "İlgili firma için kullanıcının yetkisi yok");
		public static AuthorizationException NotInSalesHierarchy => new AuthorizationException(0003, "Kullanıcı sales hiyerarşiye dahil değil");
		public static AuthorizationException YouAreNotNearToThePoint => new AuthorizationException(0004, "Kullanıcı satış noktasına yakın değil");
		public static AuthorizationException UserNotPermittedToAssing => new AuthorizationException(0005, "Kullanıcı görev atamaya yetkili değil");
		public static AuthorizationException CompanyHasNoLicenceOrPassive => new AuthorizationException(0006, "Company pasif veya lisansı yok");
        public static AuthorizationException UncompletedPriorityTasks => new AuthorizationException(0007, "Tamamlanmamış öncelikli denetimler mevcut");
    }
}

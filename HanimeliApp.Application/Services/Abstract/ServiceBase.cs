using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Application.Services.Abstract
{
	public abstract class ServiceBase
	{
		protected readonly IUnitOfWork UnitOfWork;

		public ServiceBase(IUnitOfWork unitOfWork)
		{
			UnitOfWork = unitOfWork;
		}
	}
}

using HanimeliApp.Application.Services.Abstract;
using HanimeliApp.Domain.UnitOfWorks;

namespace HanimeliApp.Application.Services
{
    public class UserService : ServiceBase
    {
        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

    }
}

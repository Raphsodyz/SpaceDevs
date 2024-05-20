using Application.Interface;
using Business.Interface;
using Domain.Interface;
using Domain.Materializated.Views;

namespace Application.Business
{
    public class LaunchViewBusiness : BusinessViewBase<LaunchView, ILaunchViewRepository>, ILaunchViewBusiness, IBusiness
    {
        public LaunchViewBusiness(IUnitOfWork uow):base(uow)
        {
            
        }       
        public async Task<bool> ViewExists()
        {
            return await _repository.ViewExists();
        }

        public async Task RefreshView()
        {
            await _repository.RefreshView();
            return;
        }
    }
}
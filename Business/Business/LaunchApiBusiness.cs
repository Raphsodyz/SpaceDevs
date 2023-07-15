using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Domain.Enum;
using Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class LaunchApiBusiness : BusinessBase<Launch, ILaunchRepository>, ILaunchApiBusiness, IBusiness
    {
        public LaunchApiBusiness(IUnitOfWork uow):base(uow)
        {
            
        }

        public Launch GetOneLaunch(int? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId && l.status == EStatus.PUBLISHED.GetDisplayName();
            bool launchExist = _launchBusiness.GetSelected(filter: launchQuery, selectColumns: l => l.Id) > 0;
            if (!launchExist)
                throw new KeyNotFoundException(ErrorMessages.KeyNotFound);

            Launch launch = _launchBusiness.Get(
                filter: launchQuery,
                includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location");

            return launch;
        }

        public Pagination<Launch> GetAllLaunchPaged(int? page)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            int totalEntities = _launchBusiness.EntityCount();
            int totalPages = totalEntities % 10 == 0 ? totalEntities / 10 : (totalEntities / 10) + 1;
            if (page > totalPages)
                throw new InvalidOperationException($"{ErrorMessages.InvalidPageSelected} Total pages = {totalPages}");

            var selectedPageLaunchList = _launchBusiness.GetAllPaged(
                page ?? 1, 10,
                includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location",
                orderBy: l => l.OrderBy(la => la.Id));

            if (selectedPageLaunchList.Entities?.Count == 0)
                throw new KeyNotFoundException(ErrorMessages.NoData);

            return selectedPageLaunchList;
        }

        public void DeleteLaunch(int? launchId)
        {
            ILaunchBusiness _launchBusiness = GetBusiness(typeof(ILaunchBusiness)) as ILaunchBusiness;

            if (launchId == null)
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            Expression<Func<Launch, bool>> launchQuery = l => l.Id == launchId;
            var launch = _launchBusiness.Get(filter: launchQuery) ?? throw new KeyNotFoundException(ErrorMessages.KeyNotFound);
            
            using var trans = _repository.GetTransaction();
            try
            {
                _launchBusiness.DeleteTransaction(launch);
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                throw ex;
            }
        }
    }
}

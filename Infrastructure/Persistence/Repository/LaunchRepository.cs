using Domain.Entities;
using Domain.Interface;
using Infrastructure.DTO;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repository
{
    public class LaunchRepository : GenericRepository<Launch>, ILaunchRepository
    {
        public LaunchRepository(FutureSpaceContext context):base(context)
        {
            
        }

        public async Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Launch, TResult>> selectColumns, string includedProperties = null)
        {
            IQueryable<Launch> query = _dbSet;

            if(!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(s => EF.Functions.ILike(s.Search, $"%{searchTerm}%"));

            if (!string.IsNullOrWhiteSpace(includedProperties))
                foreach (var includeProperty in includedProperties.Split (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty.TrimStart());

            var selectedQuery = query.Select(selectColumns);
            var result = await selectedQuery.ToListAsync();

            return result;
        }

        public async Task SetUpBaseEntityData(Launch launch)
        {
            var launchBaseEntityCompoundData = await GetAllSelectedColumns(
                filters: new List<Expression<Func<Launch, bool>>>() { l => l.ApiGuid == launch.ApiGuid },
                includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location",
                selectColumns: l => new 
                { 
                    LaunchId = l.Id, LaunchApiGuid = l.ApiGuid, LaunchAtualizationDate = l.AtualizationDate, LaunchImportedT = l.ImportedT, LaunchStatus = l.EntityStatus,
                    IdStatus = l.IdStatus, StatusId = l.Status.Id, StatusIdFromApi = l.Status.IdFromApi, StatusAtualizationDate = l.Status.AtualizationDate, StatusImportedT = l.Status.ImportedT, StatusStatus = l.Status.EntityStatus,
                    IdLaunchServiceProvider = l.IdLaunchServiceProvider, LaunchServiceProviderId = l.IdLaunchServiceProvider, LaunchServiceProviderIdFromApi = l.LaunchServiceProvider.IdFromApi, LaunchServiceProviderAtualizationDate = l.LaunchServiceProvider.AtualizationDate,
                    LaunchServiceProviderImportedT = l.LaunchServiceProvider.ImportedT, LaunchServiceProviderStatus = l.LaunchServiceProvider.EntityStatus,
                    IdRocket = l.IdRocket, RocketId = l.Rocket.Id, RocketIdFromApi = l.Rocket.IdFromApi, RocketAtualizationDate = l.Rocket.AtualizationDate, RocketImportedT = l.Rocket.ImportedT, RocketStatus = l.Rocket.EntityStatus
                    IdConfiguration = l.Rocket.IdConfiguration, ConfigurationId = l.Rocket.Configuration.Id, ConfigurationIdFromApi = l.Rocket.Configuration.IdFromApi, ConfigurationAtualizationDate = l.Rocket.Configuration.AtualizationDate, ConfigurationImportedT = l.Rocket.Configuration.ImportedT, ConfigurationStatus = l.Rocket.Configuration.EntityStatus,
                },
                buildObject: l => new LaunchBaseEntityCompoundData()
                {
                    LaunchBaseEntity = new BaseEntityLaunchData()
                    {
                        Id = l.LaunchId,
                        ApiGuid = l.LaunchApiGuid,
                        AtualizationDate = l.LaunchAtualizationDate,
                        ImportedT = l.LaunchImportedT,
                        Status = l.LaunchStatus
                    },
                    StatusBaseEntity = l.IdStatus != null ? new BaseEntityData()
                    {
                        Id = l.StatusId,
                        IdFromApi = l.StatusIdFromApi,
                        AtualizationDate = l.StatusAtualizationDate,
                        ImportedT = l.StatusImportedT,
                        Status = l.StatusStatus
                    } : null,
                    LaunchServiceProviderBaseEntity = l.IdLaunchServiceProvider != null ? new BaseEntityData()
                    {
                        Id = l.LaunchServiceProviderId,
                        IdFromApi = l.LaunchServiceProviderIdFromApi,
                        AtualizationDate = l.LaunchServiceProviderAtualizationDate,
                        ImportedT = l.LaunchServiceProviderImportedT,
                        Status = l.LaunchServiceProviderStatus
                    } : null,
                    RocketBaseEntity = l.IdRocket != null ? new BaseEntityData()
                    {
                        Id = l.RocketId,
                        IdFromApi = l.RocketIdFromApi,
                        AtualizationDate = l.RocketAtualizationDate,
                        ImportedT = l.RocketImportedT,
                        Status = l.RocketStatus
                    } : null,
                    ConfigurationBaseEntity = l.IdConfiguration != null ? new BaseEntityData()
                    {
                        Id = l.ConfigurationId,
                        IdFromApi = l.ConfigurationIdFromApi,
                        AtualizationDate = l.ConfigurationAtualizationDate,
                        ImportedT = l.ConfigurationImportedT,
                        Status = l.ConfigurationStatus
                    } : null
                }
            );
        }
    }
}

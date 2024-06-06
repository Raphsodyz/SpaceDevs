using AutoMapper;
using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.Interface;
using Infrastructure.DTO;
using Infrastructure.Persistence.Context.Factory;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repository
{
    public class LaunchRepository : GenericRepository<Launch>, ILaunchRepository
    {
        private readonly IMapper _mapper;
        public LaunchRepository(IDbContextFactory contexts, IMapper mapper):base(contexts)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Expression<Func<Launch, TResult>> selectColumns, string includedProperties = null)
        {
            var _context = _contexts.GetContext(ContextNames.FutureSpaceQuery);
            DbSet<Launch> _dbSet = _context.Set<Launch>();

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

        public async Task<Launch> SetUpBaseEntityDTO(Launch launch)
        {
            var launchBaseEntityCompoundData = await GetSelected(
                filter: l => l.ApiGuid == launch.ApiGuid,
                includedProperties: "Status, LaunchServiceProvider, Rocket.Configuration, Mission.Orbit, Pad.Location",
                selectColumns: l => new 
                { 
                    LaunchId = l.Id,
                    LaunchApiGuid = (Guid?)l.ApiGuid,
                    LaunchAtualizationDate = (DateTime?)l.AtualizationDate,
                    LaunchImportedT = (DateTime?)l.ImportedT,
                    LaunchStatus = l.EntityStatus,
                    IdStatus = l.IdStatus,
                    StatusId = (Guid?)l.Status.Id,
                    StatusIdFromApi = l.Status.IdFromApi,
                    StatusAtualizationDate = (DateTime?)l.Status.AtualizationDate,
                    StatusImportedT = (DateTime?)l.Status.ImportedT,
                    StatusStatus = l.Status.EntityStatus,
                    IdLaunchServiceProvider = l.IdLaunchServiceProvider,
                    LaunchServiceProviderId = (Guid?)l.LaunchServiceProvider.Id,
                    LaunchServiceProviderIdFromApi = l.LaunchServiceProvider.IdFromApi,
                    LaunchServiceProviderAtualizationDate = (DateTime?)l.LaunchServiceProvider.AtualizationDate,
                    LaunchServiceProviderImportedT = (DateTime?)l.LaunchServiceProvider.ImportedT,
                    LaunchServiceProviderStatus = l.LaunchServiceProvider.EntityStatus,
                    IdRocket = l.IdRocket,
                    RocketId = (Guid?)l.Rocket.Id,
                    RocketIdFromApi = l.Rocket.IdFromApi,
                    RocketAtualizationDate = (DateTime?)l.Rocket.AtualizationDate,
                    RocketImportedT = (DateTime?)l.Rocket.ImportedT,
                    RocketStatus = l.Rocket.EntityStatus,
                    IdConfiguration = l.Rocket.IdConfiguration,
                    ConfigurationId = (Guid?)l.Rocket.Configuration.Id,
                    ConfigurationIdFromApi = l.Rocket.Configuration.IdFromApi,
                    ConfigurationAtualizationDate = (DateTime?)l.Rocket.Configuration.AtualizationDate,
                    ConfigurationImportedT = (DateTime?)l.Rocket.Configuration.ImportedT,
                    ConfigurationStatus = l.Rocket.Configuration.EntityStatus,
                    IdMission = l.IdMission,
                    MissionId = (Guid?)l.Mission.Id,
                    MissionIdFromApi = l.Mission.IdFromApi,
                    MissionAtualizationDate = (DateTime?)l.Mission.AtualizationDate,
                    MissionImportedT = (DateTime?)l.Mission.ImportedT,
                    MissionStatus = l.Mission.EntityStatus,
                    IdOrbit = l.Mission.IdOrbit,
                    OrbitId = (Guid?)l.Mission.Orbit.Id,
                    OrbitIdFromApi = l.Mission.Orbit.IdFromApi,
                    OrbitAtualizationDate = (DateTime?)l.Mission.Orbit.AtualizationDate,
                    OrbitImportedT = (DateTime?)l.Mission.Orbit.ImportedT,
                    OrbitStatus = l.Mission.Orbit.EntityStatus,
                    IdPad = l.IdPad,
                    PadId = (Guid?)l.Pad.Id,
                    PadIdFromApi = l.Pad.IdFromApi,
                    PadAtualizationDate = (DateTime?)l.Pad.AtualizationDate,
                    PadImportedT = (DateTime?)l.Pad.ImportedT,
                    PadStatus = l.Pad.EntityStatus,
                    IdLocation = l.Pad.IdLocation,
                    LocationId = (Guid?)l.Pad.Location.Id,
                    LocationIdFromApi = l.Pad.Location.IdFromApi,
                    LocationAtualizationDate = (DateTime?)l.Pad.Location.AtualizationDate,
                    LocationImportedT = (DateTime?)l.Pad.Location.ImportedT,
                    LocationStatus = l.Pad.Location.EntityStatus
                },
                buildObject: l => new LaunchBaseEntityCompoundDTO()
                {
                    LaunchBaseEntity = new BaseEntityLaunchDTO()
                    {
                        Id = l.LaunchId,
                        ApiGuid = l.LaunchApiGuid,
                        AtualizationDate = l.LaunchAtualizationDate,
                        ImportedT = l.LaunchImportedT,
                        Status = l.LaunchStatus
                    },
                    StatusBaseEntity = l.IdStatus != null ? new BaseEntityDTO()
                    {
                        Id = l.StatusId,
                        IdFromApi = l.StatusIdFromApi,
                        AtualizationDate = l.StatusAtualizationDate,
                        ImportedT = l.StatusImportedT,
                        Status = l.StatusStatus
                    } : null,
                    LaunchServiceProviderBaseEntity = l.IdLaunchServiceProvider != null ? new BaseEntityDTO()
                    {
                        Id = l.LaunchServiceProviderId,
                        IdFromApi = l.LaunchServiceProviderIdFromApi,
                        AtualizationDate = l.LaunchServiceProviderAtualizationDate,
                        ImportedT = l.LaunchServiceProviderImportedT,
                        Status = l.LaunchServiceProviderStatus
                    } : null,
                    RocketBaseEntity = l.IdRocket != null ? new BaseEntityDTO()
                    {
                        Id = l.RocketId,
                        IdFromApi = l.RocketIdFromApi,
                        AtualizationDate = l.RocketAtualizationDate,
                        ImportedT = l.RocketImportedT,
                        Status = l.RocketStatus
                    } : null,
                    ConfigurationBaseEntity = l.IdConfiguration != null ? new BaseEntityDTO()
                    {
                        Id = l.ConfigurationId,
                        IdFromApi = l.ConfigurationIdFromApi,
                        AtualizationDate = l.ConfigurationAtualizationDate,
                        ImportedT = l.ConfigurationImportedT,
                        Status = l.ConfigurationStatus
                    } : null,
                    MissionBaseEntity = l.IdMission != null ? new BaseEntityDTO()
                    {
                        Id = l.MissionId,
                        IdFromApi = l.MissionIdFromApi,
                        AtualizationDate = l.MissionAtualizationDate,
                        ImportedT = l.MissionImportedT,
                        Status = l.MissionStatus
                    } : null,
                    OrbitBaseEntity = l.IdOrbit != null ? new BaseEntityDTO()
                    {
                        Id = l.OrbitId,
                        IdFromApi = l.OrbitIdFromApi,
                        AtualizationDate = l.OrbitAtualizationDate,
                        ImportedT = l.OrbitImportedT,
                        Status = l.OrbitStatus
                    } : null,
                    PadBaseEntity = l.IdPad != null ? new BaseEntityDTO()
                    {
                        Id = l.PadId,
                        IdFromApi = l.PadIdFromApi,
                        AtualizationDate = l.PadAtualizationDate,
                        ImportedT = l.PadImportedT,
                        Status = l.PadStatus
                    } : null,
                    LocationBaseEntity = l.IdLocation != null ? new BaseEntityDTO()
                    {
                        Id = l.LocationId,
                        IdFromApi = l.LocationIdFromApi,
                        AtualizationDate = l.LocationAtualizationDate,
                        ImportedT = l.LocationImportedT,
                        Status = l.LocationStatus
                    } : null
                }
            );
            return _mapper.Map(launchBaseEntityCompoundData, launch);
        }

        public async Task SaveOnUpdateLaunch(Launch launch, Guid? idStatus, Guid? idLaunchServiceProvider, Guid? idRocket, Guid? idMission, Guid? idPad)
        {
            launch.IdStatus = idStatus;
            launch.IdLaunchServiceProvider = idLaunchServiceProvider;
            launch.IdRocket = idRocket;
            launch.IdMission = idMission;
            launch.IdPad = idPad;

            //Setup null navigation properties for ef change tracker..
            launch.Status = null;
            launch.LaunchServiceProvider = null;
            launch.Rocket = null;
            launch.Mission = null;
            launch.Pad = null;

            await Save(launch);
        }
    }
}

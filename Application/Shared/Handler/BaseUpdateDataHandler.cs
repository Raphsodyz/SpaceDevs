using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Reflection;
using Application.DTO;
using Cross.Cutting.Enum;
using Cross.Cutting.Helper;
using Domain.Entities;
using Domain.Interface;

namespace Application.Shared.Handler
{
    public abstract class BaseUpdateDataHandler
    {
        protected readonly ILaunchRepository _launchRepository;
        protected readonly IGenericDapperRepository _genericDapperRepository;
        protected readonly IUpdateLogRepository _updateLogRepository;
        protected BaseUpdateDataHandler(
            ILaunchRepository launchRepository,
            IGenericDapperRepository genericDapperRepository,
            IUpdateLogRepository updateLogRepository)
        {
            _launchRepository = launchRepository;
            _genericDapperRepository = genericDapperRepository;
            _updateLogRepository = updateLogRepository;
        }

        protected async Task SaveLaunch(Launch launch, bool replaceData)
        {
            if (ObjectHelper.IsObjectEmpty(launch))
                throw new ArgumentNullException(ErrorMessages.NullArgument);

            if(replaceData == false)
            {
                if(await _launchRepository.EntityExist(l => l.ApiGuid == launch.ApiGuid))
                    return;
            }
            else
                await _launchRepository.SetUpBaseEntityDTO(launch);

            await _launchRepository.BeginTransaction();
            try
            {
                var currentlyTransaction = _launchRepository.GetCurrentlyTransaction();
                var efConnection = _launchRepository.GetEfConnection();

                Guid? idStatus = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Status>(launch.Status, null, null), efConnection, currentlyTransaction, replaceData);
                Guid? idLaunchServiceProvider = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<LaunchServiceProvider>(launch.LaunchServiceProvider, null, null), efConnection, currentlyTransaction,replaceData);
                Guid? idConfiguration = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Configuration>(launch.Rocket?.Configuration, null, null), efConnection, currentlyTransaction, replaceData);
                Guid? idRocket = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Rocket>(launch.Rocket, idConfiguration, LaunchNestedObjectsForeignKeys.ROCKET), efConnection, currentlyTransaction, replaceData);
                Guid? idOrbit = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Orbit>(launch.Mission?.Orbit, null, null), efConnection, currentlyTransaction, replaceData);
                Guid? idMission = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Mission>(launch.Mission, idOrbit, LaunchNestedObjectsForeignKeys.MISSION), efConnection, currentlyTransaction, replaceData);
                Guid? idLocation = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Location>(launch.Pad?.Location, null, null), efConnection, currentlyTransaction, replaceData);
                Guid? idPad = await SaveLaunchEntitiesProcesses(new ForeignKeyManagerDTO<Pad>(launch.Pad, idLocation, LaunchNestedObjectsForeignKeys.PAD), efConnection, currentlyTransaction, replaceData);

                await _launchRepository.SaveOnUpdateLaunch(launch, idStatus, idLaunchServiceProvider, idRocket, idMission, idPad);
                await _launchRepository.CommitTransaction();
            }
            catch
            {
                await _launchRepository.RollbackTransaction();
                throw;
            }
        }

        private async Task<Guid?> SaveLaunchEntitiesProcesses<T>(
            ForeignKeyManagerDTO<T> fkManager,
            DbConnection sharedConnection,
            DbTransaction transaction,
            bool replaceData) where T : BaseEntity
        {
            if(ObjectHelper.IsObjectEmpty(fkManager.Entity)) return null;
            if(!string.IsNullOrWhiteSpace(fkManager.DesiredFk)) _launchRepository.SetupForeignKey(fkManager.Entity, fkManager.DesiredFk, (Guid)fkManager.FkValue);
            if(replaceData == false) fkManager.Entity.Id = await DatabaseGuid(fkManager.Entity, sharedConnection, transaction);
            
            if(fkManager.Entity.Id == Guid.Empty)
                return await SaveNewLaunchEntity(fkManager.Entity, sharedConnection, transaction);

            if(replaceData == true)
                await UpdateExistingLaunch(fkManager.Entity, sharedConnection, transaction);

            return fkManager.Entity.Id;
        }

        private async Task<Guid> DatabaseGuid<T>(
            T entity,
            DbConnection sharedConnection,
            DbTransaction transaction) where T : BaseEntity
        {
            return await _genericDapperRepository.GetSelected<Guid>(
                query: $"SELECT Id FROM {typeof(T)?.GetCustomAttribute<TableAttribute>()?.Name} WHERE id_from_api = @IdFromApi",
                parameters: new { IdFromApi = entity.IdFromApi },
                sharedConnection: sharedConnection,
                transaction: transaction);
        }

        private async Task<Guid> SaveNewLaunchEntity<T>(
            T entity,
            DbConnection sharedConnection,
            DbTransaction transaction) where T : BaseEntity
        {
            entity.Id = Guid.NewGuid();
            entity.ImportedT = DateTime.Now;
            entity.AtualizationDate = DateTime.Now;
            entity.EntityStatus = EStatus.PUBLISHED.GetDisplayName();

            await _genericDapperRepository.Save(entity, sharedConnection, transaction);
            return entity.Id;
        }

        private async Task UpdateExistingLaunch<T>(
            T entity, DbConnection sharedConnection,
            DbTransaction transaction) where T : BaseEntity
        {
            await _genericDapperRepository.FullUpdate(entity, "id_from_api = @IdFromApi", sharedConnection, transaction);
        }

        protected async Task GenerateLog(int offset, string message, int entityCount, bool success)
        {
            await _updateLogRepository.Save(new UpdateLog(offset, message, entityCount, success));
        }
    }
}
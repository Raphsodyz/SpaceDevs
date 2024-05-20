using Domain.Entities;

namespace Domain.Interface
{
    public interface IUnitOfWork
    {
        IRepository Repository(Type type);
        Task Save();
        void Dispose();
        IGenericDapperRepository Dapper();
        void SetupForeignKey<T>(T entity, string foreignKeyName, Guid desiredFkValue) where T : BaseEntity;
    }
}

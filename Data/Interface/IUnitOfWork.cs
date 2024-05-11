using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Data.Interface
{
    public interface IUnitOfWork
    {
        IRepository Repository(Type type);
        Task Save();
        void Dispose();
        IRepository Dapper<T>() where T : BaseEntity;
        void SetupForeignKey<T>(T entity, string foreignKeyName, Guid desiredFkValue) where T : BaseEntity;
    }
}

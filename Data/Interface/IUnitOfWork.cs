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
        void Save();
        void Dispose();
        IRepository Dapper<T>() where T : BaseEntity;
    }
}

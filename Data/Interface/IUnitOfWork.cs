using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interface
{
    public interface IUnitOfWork
    {
        IRepository Repository(Type type);
        void Save();
        void Dispose();
    }
}

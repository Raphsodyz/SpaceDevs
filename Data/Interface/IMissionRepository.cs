using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interface
{
    public interface IMissionRepository : IGenericRepository<Mission>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Func<Mission, TResult> selectColumns, string includedProperties = null);    }
    }

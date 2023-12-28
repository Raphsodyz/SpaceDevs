using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interface
{
    public interface IPadRepository : IGenericRepository<Pad>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Func<Pad, TResult> selectColumns, string includedProperties = null);    }
    }

using Data.Interface;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IConfigurationBusiness : IBusinessBase<Configuration, IConfigurationRepository>
    {
        Task<IEnumerable<TResult>> ILikeSearch<TResult>(string searchTerm, Func<Configuration, TResult> selectColumns, string includedProperties = null);
    }
}

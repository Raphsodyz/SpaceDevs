using Data.Context;
using Data.Interface;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class ConfigurationRepository : GenericRepository<Configuration>, IConfigurationRepository
    {
        public ConfigurationRepository(FutureSpaceContext context):base(context)
        {
            
        }
    }
}

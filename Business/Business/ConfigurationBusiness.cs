using Business.Interface;
using Data.Interface;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Business
{
    public class ConfigurationBusiness : BusinessBase<Configuration, IConfigurationRepository>, IConfigurationBusiness, IBusiness
    {
        public ConfigurationBusiness(IUnitOfWork uow):base(uow)
        {
            
        }
    }
}

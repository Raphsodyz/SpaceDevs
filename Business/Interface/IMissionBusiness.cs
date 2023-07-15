using Data.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IMissionBusiness : IBusinessBase<Mission, IMissionRepository>
    {
    }
}

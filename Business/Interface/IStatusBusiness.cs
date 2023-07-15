using Data.Interface;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IStatusBusiness : IBusinessBase<Status, IStatusRepository>
    {
    }
}

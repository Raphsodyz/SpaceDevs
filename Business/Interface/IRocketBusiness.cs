using Data.Interface;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IRocketBusiness : IBusinessBase<Rocket, IRocketRepository>
    {
    }
}

using Data.Context;
using Data.Interface;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class RocketRepository : GenericRepository<Rocket>, IRocketRepository
    {
        public RocketRepository(FutureSpaceContext context):base(context)
        {
            
        }
    }
}

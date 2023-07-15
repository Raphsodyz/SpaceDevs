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
    public class OrbitRepository : GenericRepository<Orbit>, IOrbitRepository
    {
        public OrbitRepository(FutureSpaceContext context):base(context)
        {
            
        }
    }
}

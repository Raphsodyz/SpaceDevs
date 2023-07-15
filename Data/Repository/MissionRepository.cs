using Data.Context;
using Data.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class MissionRepository : GenericRepository<Mission>, IMissionRepository
    {
        public MissionRepository(FutureSpaceContext context):base(context)
        {
            
        }
    }
}

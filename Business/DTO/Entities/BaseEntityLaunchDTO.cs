using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.DTO.Entities
{
    public class BaseEntityLaunchDTO
    {
        public Guid Id { get; set; }
        public Guid ApiGuid { get; set; }
        public DateTime AtualizationDate { get; set; }
        public DateTime ImportedT { get; set; }
        public string Status { get; set; }
    }
}
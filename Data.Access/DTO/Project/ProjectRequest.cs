using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Enum;

namespace Domain.Model.DTO.Project
{
    public class ProjectRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string UserId { get; set; }
    }
}

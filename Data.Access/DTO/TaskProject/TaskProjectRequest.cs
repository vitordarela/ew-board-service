using Domain.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.DTO.TaskBoard
{
    public class TaskProjectRequest
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public TaskProjectPriority Priority { get; set; }

        public DateTime DueDate { get; set; }

        public TaskStatus Status { get; set; }
    }
}

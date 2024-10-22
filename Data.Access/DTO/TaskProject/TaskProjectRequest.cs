﻿using Domain.Model.Enum;

namespace Domain.Model.DTO.TaskBoard
{
    public class TaskProjectRequest
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public TaskProjectPriority Priority { get; set; }

        public DateTime DueDate { get; set; }

        public TaskStatus Status { get; set; }
    }
}

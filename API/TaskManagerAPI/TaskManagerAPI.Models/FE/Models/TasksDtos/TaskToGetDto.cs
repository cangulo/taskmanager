using System;

namespace TaskManagerAPI.Models.FE.TasksDtos
{
    public class TaskToGetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateToBeFinished { get; set; }
        public DateTime DateToBeNotified { get; set; }
    }
}
using System;

namespace TaskManagerAPI.Models.BE.Tasks
{
    public class TaskForUpdated
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateToBeFinished { get; set; }
        public DateTime DateToBeNotified { get; set; }
    }
}
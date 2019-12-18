using System;

namespace TaskManagerAPI.Models.BE.Tasks
{
    public class TaskDomain
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateToBeFinished { get; set; }
        public DateTime DateToBeNotified { get; set; }
        public Account Account { get; set; }
        public int AccountId { get; set; }
    }
}
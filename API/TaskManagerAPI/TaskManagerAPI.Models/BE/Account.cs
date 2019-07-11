using System;

namespace TaskManagerAPI.Models.BE
{
    public class Account
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int FailedLoginAttempts { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime LastLogintime { get; set; }
        public string Token { get; set; }
        public UserStatus Status { get; set; }
    }

    public enum UserStatus
    {
        Active,
        Disable,
        Locked
    }
}

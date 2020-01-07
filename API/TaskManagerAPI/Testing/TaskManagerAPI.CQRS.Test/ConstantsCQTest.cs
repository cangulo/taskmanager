using System;
using System.Collections.Generic;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.BE.Tasks;

namespace TaskManagerAPI.CQRS.Test
{
    public class ConstantsCQTest
    {
        public const int Id = 1;
        public const string Email = "test@test.com";
        public const string Username = "test username";
        public const string Password = "pass@word";
        public const int FailedloginAttempts = 0;
        public const string PhoneNumber = "+34123456789";
        public static DateTime LastLoginTime = DateTime.MinValue;
        public const string Token = "Token";
        public const string Token2 = "Token2";
        public const UserStatus Status = UserStatus.Active;

        public static Account AccountTest = new Account
        {
            Email = Email,
            Password = Password,
            Id = Id,
            Username = Username,
            FailedLoginAttempts = FailedloginAttempts,
            PhoneNumber = PhoneNumber,
            LastLogintime = LastLoginTime,
            Token = Token,
            Status = Status,
            Tasks = new List<TaskDomain>()
        };
    }
}

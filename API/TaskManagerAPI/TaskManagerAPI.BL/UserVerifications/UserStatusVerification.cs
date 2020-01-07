using FluentResults;
using System.Linq;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.Models.BE;
using TaskManagerAPI.Models.Errors;
using TaskManagerAPI.Resources.Errors;

namespace TaskManagerAPI.BL.UserStatusVerification
{
    public class UserStatusVerification : IUserStatusVerification
    {
        private readonly ITaskManagerDbContext _taskManagerDbContext;

        public UserStatusVerification(ITaskManagerDbContext taskManagerDbContext)
        {
            _taskManagerDbContext = taskManagerDbContext;
        }

        public Result UserIsActive(int userId)
        {
            if (this._taskManagerDbContext.Accounts.Any(x => x.Id == userId))
            {
                UserStatus status = this._taskManagerDbContext.Accounts.First(x => x.Id == userId).Status;
                if (status == UserStatus.Active)
                {
                    return Results.Ok();
                }
                else if (status == UserStatus.Disable)
                {
                    return Results.Fail(
                        new CustomError(ErrorsCodesContants.USER_DISABLED, ErrorsMessagesConstants.USER_DISABLED, 401));
                }
                else if (status == UserStatus.Locked)
                {
                    return Results.Fail(
                        new CustomError(ErrorsCodesContants.USER_LOCKED, ErrorsMessagesConstants.USER_LOCKED, 401));
                }
                else
                {
                    return Results.Fail(
                        new CustomError(ErrorsCodesContants.UNKNOWN_ERROR_API, ErrorsMessagesConstants.UNKNOWN_ERROR_API, 500));
                }
            }
            else
            {
                return Results.Fail(
                    new CustomError(ErrorsCodesContants.USER_ID_NOT_FOUND, ErrorsMessagesConstants.USER_ID_NOT_FOUND, 401));
            }
        }
    }
}
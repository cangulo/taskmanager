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

        public Result<bool> UserIsActive(int userId)
        {
            if (this._taskManagerDbContext.Accounts.Any(x => x.Id == userId))
            {
                UserStatus status = this._taskManagerDbContext.Accounts.First(x => x.Id == userId).Status;
                if (status == UserStatus.Active)
                {
                    return Results.Ok<bool>(true);
                }
                else if (status == UserStatus.Disable)
                {
                    return Results.Fail<bool>(
                        new ErrorCodeAndMessage(
                                ErrorsCodesContants.USER_DISABLED,
                                ErrorsMessagesConstants.USER_DISABLED));
                }
                else if (status == UserStatus.Locked)
                {
                    return Results.Fail<bool>(
                        new ErrorCodeAndMessage(
                                ErrorsCodesContants.USER_LOCKED,
                                ErrorsMessagesConstants.USER_LOCKED));
                }
                else
                {
                    return Results.Fail<bool>(
                        new ErrorCodeAndMessage(
                                ErrorsCodesContants.UNKNOWN_ERROR_API,
                                ErrorsMessagesConstants.UNKNOWN_ERROR_API));
                }
            }
            else
            {
                return Results.Fail<bool>(
                    new ErrorCodeAndMessage(
                            ErrorsCodesContants.USER_ID_NOT_FOUND,
                            ErrorsMessagesConstants.USER_ID_NOT_FOUND));
            }
        }
    }
}

using FluentResults;
using System;
using TaskManagerAPI.BL.CurrentUserService;

namespace TaskManagerAPI.CQRS.TasksCQ.BaseClasses
{
    public interface IBaseCommandQuery
    {
        int GetCurrentUserId();
    }

    public abstract class BaseCommandQuery : IBaseCommandQuery
    {
        private readonly int _currentUserId;

        protected BaseCommandQuery(ICurrentUserService currentUserService)
        {
            if (currentUserService == null)
            {
                throw new ArgumentNullException(nameof(currentUserService));
            }
            else
            {
                Result<int> opGetId = currentUserService.GetIdCurrentUser();
                if (opGetId.IsSuccess)
                {
                    _currentUserId = opGetId.Value;
                }
                else
                {
                    throw new ArgumentNullException(nameof(opGetId));
                }
            }
        }

        public int GetCurrentUserId()
        {
            return this._currentUserId;
        }
    }
}
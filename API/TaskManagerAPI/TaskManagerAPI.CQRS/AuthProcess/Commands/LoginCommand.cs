﻿using FluentResults;
using MediatR;
using TaskManagerAPI.Models.FE;

namespace TaskManagerAPI.CQRS.AuthProcess.Commands
{
    public class LoginCommand : IRequest<Result<PortalAccount>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
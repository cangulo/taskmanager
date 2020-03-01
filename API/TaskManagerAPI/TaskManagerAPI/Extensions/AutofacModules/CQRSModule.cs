using Autofac;
using MediatR;
using System;
using System.Linq;
using TaskManagerAPI.CQRS.DomainValidatorModel;
using TaskManagerAPI.CQRS.HandlerDecorator;
using TaskManagerAPI.CQRS.TasksCQ.CommandHandlers;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.CQRS.TasksCQ.CommandValidators;

namespace TaskManagerAPI.Extensions.AutofacModules
{
    public class CQRSModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var dataAccess = typeof(DeleteTaskCommandValidator).Assembly;

            var validators = dataAccess
                .GetTypes()
                .Where(x =>
                    {
                        return
                        x.IsClass &&
                        x.BaseType.IsAbstract &&
                        x.BaseType.IsGenericType &&
                        x.BaseType.GetGenericTypeDefinition() == typeof(BaseCustomDomainValidator<>);
                    })
                .ToArray();

            containerBuilder
                .RegisterTypes(validators)
                .As((validator) =>
                {
                    var requestType = validator.BaseType.GenericTypeArguments[0];
                    return typeof(ICustomDomainValidator<>).MakeGenericType(new Type[] { requestType });
                });

            containerBuilder
                .RegisterGenericDecorator(typeof(RequestHandlerLogDecorator<,>), typeof(IRequestHandler<,>));
        }
    }
}
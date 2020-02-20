using Autofac;
using FluentResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagerAPI.CQRS.DomainValidatorModel;
using TaskManagerAPI.CQRS.HandlerDecorator;
using TaskManagerAPI.CQRS.TasksCQ.Commands;
using TaskManagerAPI.CQRS.TasksCQ.CommandValidators;

namespace TaskManagerAPI.Extensions.AutofacModules
{
    public class CQRSModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            var validators = typeof(DeleteTaskCommandValidator)
                .Assembly
                .GetTypes()
                .Where(x => x.Name.EndsWith("Validator"))
                .ToArray();

            containerBuilder
                .RegisterTypes(validators)
                .As((validator) =>
                    {
                        Type requestType = validator.BaseType.GenericTypeArguments[0];
                        return typeof(ICustomDomainValidator<>).MakeGenericType(new Type[] { requestType });
                    }
                );

            containerBuilder
                .RegisterGenericDecorator(
                    typeof(RequestHandlerValidatorDecorator<,>),
                    typeof(IRequestHandler<,>),
                    (context) =>
                    {
                        string requestTypeName = context.ServiceType.GetGenericArguments()[0].Name;
                        return validators.Any(x => x.Name == requestTypeName + "Validator");
                    });

            containerBuilder
                .RegisterGenericDecorator(typeof(RequestHandlerLogDecorator<,>), typeof(IRequestHandler<,>));
        }
    }
}
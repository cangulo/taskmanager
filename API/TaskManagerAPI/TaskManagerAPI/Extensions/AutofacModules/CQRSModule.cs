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
            List<Type> classes = typeof(DeleteTaskCommandValidator).Assembly.GetTypes().ToList();
            List<Type> validators = classes.Where(x => x.Name.EndsWith("Validator")).ToList();
            foreach (var validator in validators)
            {
                Console.WriteLine(validator.Namespace);

                Type requestType = validator.BaseType.GenericTypeArguments[0];
                Type responseType = requestType.GetInterfaces()[0].GenericTypeArguments[0];


                Type interfaceCustomValidatorType = typeof(ICustomDomainValidator<>);
                Type validatorInterfaceTyped = interfaceCustomValidatorType.MakeGenericType(new Type[] { requestType });

                containerBuilder.RegisterType(validator).As(validatorInterfaceTyped);


                Type validatorDecoratorGeneric = typeof(RequestHandlerValidatorDecorator<,>);
                Type[] decoratorArgs = new Type[] { requestType, responseType };
                Type validatorDecoratorSpecific = validatorDecoratorGeneric.MakeGenericType(decoratorArgs);


                Type requestHandlerGeneric = typeof(IRequestHandler<,>);
                Type requestHandlerSpecific = requestHandlerGeneric.MakeGenericType(decoratorArgs);

                containerBuilder
                    .RegisterDecorator(validatorDecoratorSpecific, requestHandlerSpecific);

            }

            containerBuilder
                .RegisterGenericDecorator(typeof(RequestHandlerLogDecorator<,>), typeof(IRequestHandler<,>));
        }
    }
}
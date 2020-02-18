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
            // TODO: Make Generic
            containerBuilder
                .RegisterType<DeleteTaskCommandValidator>()
                .As<ICustomDomainValidator<DeleteTaskCommand>>();

            // TODO: Make Generic
            //containerBuilder
            //    .RegisterDecorator(typeof(RequestHandlerValidatorDecorator<DeleteTaskCommand, Result>), typeof(IRequestHandler<DeleteTaskCommand, Result>));

            List<Type> classes = typeof(DeleteTaskCommandValidator).Assembly.GetTypes().ToList();
            List<Type> validators = classes.Where(x => x.Name.EndsWith("Validator")).ToList();
            foreach (var validator in validators)
            {
                Console.WriteLine(validator.Namespace);


                Type requestType = validator.BaseType.GenericTypeArguments[0];
                Type responseType = requestType.GetInterfaces()[0].GenericTypeArguments[0];

                Type validatorDecoratorGeneric = typeof(RequestHandlerValidatorDecorator<,>);
                Type[] decoratorArgs = new Type[] { requestType, responseType };
                Type validatorDecoratorSpecific = validatorDecoratorGeneric.MakeGenericType(decoratorArgs);


                Type requestHandlerGeneric = typeof(IRequestHandler<,>);
                Type requestHandlerSpecific = requestHandlerGeneric.MakeGenericType(decoratorArgs);

                containerBuilder
                    .RegisterDecorator(validatorDecoratorSpecific, requestHandlerSpecific);

            }

            //containerBuilder
            //    .RegisterGenericDecorator(typeof(RequestHandlerValidatorDecorator<,>), typeof(IRequestHandler<,>), (context) =>
            //    {
            //        return registerDecorator;
            //    });

            containerBuilder
                .RegisterGenericDecorator(typeof(RequestHandlerLogDecorator<,>), typeof(IRequestHandler<,>));
        }
    }
}
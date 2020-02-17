using Autofac;
using FluentResults;
using MediatR;
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
            containerBuilder
                .RegisterType<DeleteTaskCommandValidator>()
                .As<ICustomDomainValidator<DeleteTaskCommand>>();

            //bool registerDecorator = false;


            containerBuilder
                .RegisterDecorator(typeof(RequestHandlerValidatorDecorator<DeleteTaskCommand, Result>), typeof(IRequestHandler<DeleteTaskCommand, Result>));
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
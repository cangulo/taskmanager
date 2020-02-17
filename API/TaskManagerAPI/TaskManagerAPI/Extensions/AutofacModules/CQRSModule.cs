using Autofac;
using MediatR;
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
            //var dataAccess = typeof(CQRSModule).Assembly;

            //containerBuilder.RegisterAssemblyTypes(dataAccess)
            //    .AsClosedTypesOf(typeof(ICustomDomainValidator<>))
            //    .AsImplementedInterfaces();

            // TODO: Make Generic
            containerBuilder
                .RegisterType<DeleteTaskCommandValidator>()
                .As<ICustomDomainValidator<DeleteTaskCommand>>();

            //containerBuilder
            //    .RegisterGeneric(typeof(CustomDomainValidator<>))
            //    .As(typeof(ICustomDomainValidator<>)).InstancePerLifetimeScope();
            containerBuilder
                .RegisterGenericDecorator(typeof(RequestHandlerLogDecorator<,>), typeof(IRequestHandler<,>));
        }
    }
}
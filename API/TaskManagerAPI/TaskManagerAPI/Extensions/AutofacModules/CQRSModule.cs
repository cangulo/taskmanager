using Autofac;
using MediatR;
using TaskManagerAPI.CQRS.HandlerDecorator;

namespace TaskManagerAPI.Extensions.AutofacModules
{
    public class CQRSModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterGenericDecorator(typeof(RequestHandlerLogDecorator<,>), typeof(IRequestHandler<,>));
        }
    }
}

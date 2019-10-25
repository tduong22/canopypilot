using Autofac;
using CanopyManage.Common.EventBus.Abstractions;
using System.Reflection;

namespace CanopyManage.Application.Compositions
{
    public class EventHandlingModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(
               typeof(EventHandlingModule).GetTypeInfo().Assembly
               ).AsClosedTypesOf(typeof(IIntegrationEventHandler<>));
        }
    }
}

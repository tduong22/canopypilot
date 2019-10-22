using Autofac;
using CanopyManage.Common.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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

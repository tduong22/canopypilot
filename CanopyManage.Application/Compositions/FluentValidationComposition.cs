using Autofac;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CanopyManage.Application.Compositions
{
    public class FluentValidationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register the Command's Validators (Validators based on FluentValidation library)
            builder
                .RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();
        }
    }
}

using System;
using Microsoft.Extensions.DependencyInjection;

namespace CashTrak.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        public Type ServiceType { get; set; }
        public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Transient;
    }
}
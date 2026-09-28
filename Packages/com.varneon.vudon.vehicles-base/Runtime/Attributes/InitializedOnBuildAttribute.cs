using System;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Marks a field to be initialized on build instead of during runtime
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InitializedOnBuildAttribute : Attribute { }
}

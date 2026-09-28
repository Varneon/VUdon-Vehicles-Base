using System;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Marks the value set on a field to be considered per vehicle, must be overridden with data from other vehicle template during runtime
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PerVehicleDataAttribute : Attribute { }
}

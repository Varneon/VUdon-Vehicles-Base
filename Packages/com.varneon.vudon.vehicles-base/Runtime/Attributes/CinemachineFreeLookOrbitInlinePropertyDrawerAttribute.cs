using Cinemachine;
using System;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Add this attribute to a field of type <see cref="CinemachineFreeLook.Orbit"/> to override it with an inline property drawer
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class CinemachineFreeLookOrbitInlinePropertyDrawerAttribute : Attribute
    {
        public readonly string Name;

        /// <summary>
        /// Override's the field's default inspector with an inline property attribute
        /// </summary>
        /// <param name="name">Name of the property's header instead of</param>
        public CinemachineFreeLookOrbitInlinePropertyDrawerAttribute(string name)
        {
            Name = name;
        }
    }
}

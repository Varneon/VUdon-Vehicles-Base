using UnityEngine;
using Varneon.VSDK.Interfaces;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Defines an <see cref="AudioLowPassFilter"/> that should be overridden when a player is sitting in a vehicle
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Vehicle Exterior LowPass Descriptor")]
    [DisallowMultipleComponent]
    [ExcludeFromPreset]
    [RequireComponent(typeof(AudioLowPassFilter))]
    public class VehicleExteriorLowPassDescriptor : LandVehicleObjectDescriptor { }
}

using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining a root GameObject for realtime lights on a vehicle that should be activated during night time
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Vehicle Realtime Lights Descriptor")]
    [DisallowMultipleComponent]
    public class VehicleRealtimeLightsDescriptor : LandVehicleObjectDescriptor { } // TODO: Migrate to LandVehicleConditionalActiveDescriptor as an enum item "NightTime"
}

using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;
using Varneon.VUdon.VisibilitySensors;

namespace Varneon.VUdon.VehiclesBase
{
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [ExcludeFromPreset]
    [RequireComponent(typeof(VisibilitySensorDescriptor))]
    public class VehicleVisibilitySensorDescriptor : LandVehicleObjectDescriptor { }
}

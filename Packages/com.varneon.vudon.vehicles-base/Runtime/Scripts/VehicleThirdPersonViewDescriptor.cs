using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining the root of a third person view camera rig attached to a vehicle
    /// </summary>
    /// <remarks>
    /// Automatically handled during prefab generation, user shouldn't have to deal with this manually
    /// </remarks>
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [ExcludeFromPreset]
    public class VehicleThirdPersonViewDescriptor : LandVehicleObjectDescriptor
    {
        [Tooltip("Follow target used by CarController to turn the camera rig 180 degrees when gear is in reverse")]
        public Transform FollowTarget;
    }
}

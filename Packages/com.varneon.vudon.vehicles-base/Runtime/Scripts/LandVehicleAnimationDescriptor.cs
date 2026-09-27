using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for non-destructively animating a <see cref="Transform"/> with vehicle's own built-in animator for performance
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Animation Descriptor")]
    [DisallowMultipleComponent]
    public class LandVehicleAnimationDescriptor : LandVehicleObjectDescriptor
    {
        [Tooltip("Vehicle's animation layer to which this Transform's orientation will be bound to.")]
        public LandVehicleAnimationType AnimationType;

        [Tooltip("Rotation at normalized time 0.0 of the AnimationClip")]
        public Vector3 StartRotation;

        [Tooltip("Rotation at normalized time 1.0 of the AnimationClip")]
        public Vector3 EndRotation;
    }
}

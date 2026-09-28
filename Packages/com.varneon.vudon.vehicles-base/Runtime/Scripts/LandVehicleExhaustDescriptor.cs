using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;
using Varneon.VUdon.VehiclesLite;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining a <see cref="ParticleSystem"/> that should be used to emit exhaust smoke
    /// </summary>
    /// <remarks>
    /// NOTE: <see cref="CarController"/> only supports one exhaust particle system for performance reasons, if you have multiple exhaust pipes, use a mesh as the shape of the ParticleSystem with triangles pointing outwards at each tip
    /// </remarks>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Exhaust Descriptor")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ParticleSystem))]
    public class LandVehicleExhaustDescriptor : LandVehicleObjectDescriptor
    {
        [Tooltip("Initial speed of the particles based on normalized engine RPM")]
        public AnimationCurve SpeedCurve = AnimationCurve.Linear(0f, 1f, 1f, 4f);

        [Tooltip("Amount of particles to emit based on normalized engine RPM")]
        public AnimationCurve EmissionCurve = new AnimationCurve(
            new Keyframe(0f, 100f, -2f, -2f),
            new Keyframe(0.2f, 0f, 0f, 0f),
            new Keyframe(1f, 0f, 0f, 0f)
            );

        [Tooltip("Multiplier for emission based on normalized throttle input")]
        public AnimationCurve ThrottleMultiplier = new AnimationCurve(new Keyframe(0f, 0.2f, 0f, 0f), new Keyframe(0.25f, 1f, 0f, 0f), new Keyframe(1f, 1f, 0f, 0f));
    }
}

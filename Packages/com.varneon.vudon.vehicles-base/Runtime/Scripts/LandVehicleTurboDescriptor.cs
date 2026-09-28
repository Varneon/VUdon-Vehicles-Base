using UnityEngine;
using UnityEngine.Serialization;
using Varneon.VUdon.Editors;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining a simple turbocharger on a vehicle
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Turbo Descriptor")]
    [DisallowMultipleComponent]
    public class LandVehicleTurboDescriptor : LandVehicleObjectDescriptor
    {
        [Header("Physics")]
        [Tooltip("Torque in Nm that will be added to the engine's torque at given normalized turbocharger speed")]
        public AnimationCurve TorqueCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 150f, 285.7502f, 285.7502f));

        [Tooltip("Arbitrary number for how fast the turbocharger can accelerate")]
        public float Acceleration = 0.45f;

        [Tooltip("Arbitrary number for how fast the turbocharger can decelerate")]
        public float Deceleration = 0.25f;

        [Header("Audio")]
        public AnimationCurve Volume = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(1f, 0.5f, 1.282088f, 1.282088f));

        public AnimationCurve Pitch = new AnimationCurve(new Keyframe(0f, 1f, 0f, 3f), new Keyframe(1f, 4f, 3f, 1f));

        [FormerlySerializedAs("turboSFX")]
        [FieldNullWarning]
        public AudioClip TurboSFX;

        [FormerlySerializedAs("blowoffSFX")]
        [Tooltip("Optional")]
        public AudioClip BlowoffSFX;
    }
}

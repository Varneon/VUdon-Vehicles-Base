using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Varneon.VUdon.Editors;
using Varneon.VUdon.VehiclesBase.Abstract;
using Object = UnityEngine.Object;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// A descriptor for defining a wheel to be automatically set up for a vehicle
    /// </summary>
    /// <remarks>
    /// If the source vehicle model was created according to established standards, user will never have to add these manually, as the hierarchy scan will add these automatically when <see cref="LandVehicleDescriptor"/> is added
    /// </remarks>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Wheel Descriptor")]
    [DisallowMultipleComponent]
    public class LandVehicleWheelDescriptor : LandVehicleObjectDescriptor
    {
        [FoldoutHeader("Tire Profile")]
        [Range(0.01f, 2f)]
        public float Radius = 0.35f;

        [Range(0.01f, 3f)]
        public float Width = 0.2f;

        [Range(0.01f, 1f)]
        [Tooltip("Normalized fraction of the wheel's width that is in contact with the ground.\n\nThis will determine the width of the skidmark renderer trail.")]
        public float ContactFraction = 0.9f;

        [Range(0f, 1f)]
        [Tooltip("0 = Slick\n1 = Off-Road")]
        public float Profile = 0.25f;

        [FoldoutHeader("Configuration")]
        [Min(1f)]
        public float BrakeTorque = 2700f;

        public bool Driven = true;

        public bool Steered = true;

        [Min(0.01f)]
        public float SuspensionDistance = 0.15f;

        [FormerlySerializedAs("Offset")]
        [Tooltip("Offset for where the generated WheelCollider should be located")]
        public Vector3 ColliderOffset = new Vector3(0f, 0.075f, 0f);

        [FoldoutHeader("Spring Values", "Modify the joint spring values of the wheel")]
        [Tooltip("If you already know the exact force that the spring should be set to, you may provide it manually instead of a multiplier")]
        public bool UseAbsoluteSpringOverride = false;

        [FieldDisable(LogicType.NOR, nameof(UseAbsoluteSpringOverride))]
        [Tooltip("Final multiplier for the automatically calculated spring force")]
        [Min(0.01f)]
        public float SpringMultiplier = 1f;

        [FieldDisable(nameof(UseAbsoluteSpringOverride))]
        [Min(1f)]
        public float SpringOverride = 75000;

        [Tooltip("If you already know the exact force that the damper should be set to, you may provide it manually instead of a multiplier")]
        public bool UseAbsoluteDamperOverride = false;

        [FieldDisable(LogicType.NOR, nameof(UseAbsoluteDamperOverride))]
        [Tooltip("Final multiplier for the automatically calculated damper force")]
        [Min(0.01f)]
        public float DamperMultiplier = 1f;

        [FieldDisable(nameof(UseAbsoluteDamperOverride))]
        [Min(1f)]
        public float DamperOverride = 3250;

        [FoldoutHeader("Friction Values", "Modify the extremum and asymptote values of the wheel friction curves")]
        [Range(1f, 2f)]
        public float ForwardExtremum = 1.75f;

        [Range(0.75f, 1.5f)]
        public float ForwardAsymptote = 1.25f;

        [Range(1f, 2f)]
        public float SidewaysExtremum = 1.75f;

        [Range(0.75f, 1.5f)]
        public float SidewaysAsymptote = 1.25f;

        [FoldoutHeader("Extra")]
        [Tooltip("Horizontal compensation for the center of the wheel mesh bounds")]
        public float HorizontalOffset = 0f;

        [Tooltip("Optional root for e.g. brake calibers, these will move with suspension and rotate based on steering but not roll with the wheel")]
        public Transform WheelHub;

        /// <summary>
        /// The generated WheelCollider linked to this descriptor
        /// </summary>
        [HideInInspector]
        public WheelCollider Collider;

        internal bool IsRotationValid => VehicleDescriptor && Quaternion.Angle(VehicleDescriptor.transform.rotation, transform.rotation) == 0f && transform.localRotation == Quaternion.identity;

        private LandVehicleDescriptor VehicleDescriptor
        {
            get
            {
                if(vehicleDescriptor == null)
                {
                    vehicleDescriptor = GetComponentInParent<LandVehicleDescriptor>();
                }

                return vehicleDescriptor;
            }
        }

        private LandVehicleDescriptor vehicleDescriptor;

        internal Transform Root
        {
            get
            {
                if(root == null)
                {
                    root = VehicleDescriptor?.transform;
                }

                return root;
            }
        }

        [NonSerialized]
        private Transform root;

        public void ApplyHorizontalOffset(float offset)
        {
#if UNITY_EDITOR
            Undo.RecordObjects(new Object[] { transform, this }, "Apply Wheel Offset");
#endif

            HorizontalOffset -= offset;

            Vector3 offsetVector = Vector3.right * offset;

            transform.localPosition += offsetVector;

            for(int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

#if UNITY_EDITOR
                Undo.RecordObject(child, "Apply Wheel Offset");
#endif

                child.localPosition += Vector3.left * offset;
            }
        }
    }
}

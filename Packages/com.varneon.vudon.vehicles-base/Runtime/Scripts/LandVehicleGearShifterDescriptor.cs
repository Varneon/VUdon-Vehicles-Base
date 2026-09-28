#if !COMPILER_UDONSHARP
using UnityEngine;
using Varneon.VSDK;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining the orientation of the gear shifter for each gear
    /// </summary>
    /// <remarks>
    /// <para>All Euler angles are in local space, ensure that this descriptor is attached to a Transform with neutral orientation</para>
    /// </remarks>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Gear Shifter Descriptor")]
    [DisallowMultipleComponent]
    public class LandVehicleGearShifterDescriptor : LandVehicleObjectDescriptor
    {
        [Header("Shifter")]
        [Tooltip("Center position of the knob in neutral")]
        public Vector3 KnobOffset = new Vector3(0f, 0.2f, 0f);

        [Min(0.01f)]
        public float KnobRadius = 0.025f;

        [Header("Gears")]
        [Tooltip("Euler angle of the gear shifter in neutral")]
        public Vector3 Neutral;

        [Tooltip("Euler angles of the gear shifter in reverse gear")]
        public Vector3 Reverse = new Vector3(-10f, 0f, -10f);

        [Tooltip("Euler angles of the gear shifter in all forward gears")]
        public Vector3[] ForwardGears = new Vector3[]
        {
            new Vector3(10f, 0f, 10f),
            new Vector3(-10f, 0f, 10f),
            new Vector3(10f, 0f, 0f),
            new Vector3(-10f, 0f, 0f),
            new Vector3(10f, 0f, -10f),
            new Vector3(-10f, 0f, -10f)
        };

        private void OnDrawGizmosSelected()
        {
            DrawKnobGizmos(Color.cyan, ref Neutral);
            DrawKnobGizmos(Color.red, ref Reverse);

            for(int i = 0; i < ForwardGears.Length; i++)
            {
                DrawKnobGizmos(Color.green, ref ForwardGears[i]);
            }
        }

        private void DrawKnobGizmos(Color color, ref Vector3 eulerAngles)
        {
            using (new GizmosDrawingScope(color, transform.localToWorldMatrix * Matrix4x4.Rotate(Quaternion.Euler(eulerAngles))))
            {
                Gizmos.DrawLine(Vector3.zero, KnobOffset);
                Gizmos.DrawWireSphere(KnobOffset, KnobRadius);
            }
        }
    }
}
#endif

using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor.Utilities
{
    public static class WheelColliderUtility
    {
        public static WheelFrictionCurve DefaultFrictionCurve(float extremumValue = 1.75f, float asymptoteValue = 1.25f) => new WheelFrictionCurve()
        {
            extremumSlip = 0.15f,
            extremumValue = extremumValue,
            asymptoteSlip = 0.3f,
            asymptoteValue = asymptoteValue,
            stiffness = 1f
        };

        public static JointSpring DefaultSpring(Rigidbody rigidbody, float suspensionDistance, int wheelCount)
        {
            float spring = rigidbody.mass * 9.81f * 2f / wheelCount / suspensionDistance;

            return new JointSpring()
            {
                spring = spring,
                damper = spring / 20f,
                targetPosition = 0.5f
            };
        }

        public static JointSpring DefaultSpring(Rigidbody rigidbody, LandVehicleWheelDescriptor descriptor, int wheelCount)
        {
            float spring = rigidbody.mass * 9.81f * 2f / wheelCount / descriptor.SuspensionDistance;

            return new JointSpring()
            {
                spring = descriptor.UseAbsoluteSpringOverride ? descriptor.SpringOverride : (spring * descriptor.SpringMultiplier),
                damper = descriptor.UseAbsoluteDamperOverride ? descriptor.DamperOverride : (spring / 20f * descriptor.DamperMultiplier),
                targetPosition = 0.5f
            };
        }

        public static WheelCollider AddWheelCollider(GameObject gameObject, float radius, float suspensionDistance, Vector3 center, JointSpring spring)
        {
            WheelCollider w = gameObject.AddComponent<WheelCollider>();

            w.mass = 100f;
            w.radius = radius;
            w.center = center;
            w.suspensionSpring = spring;
            w.suspensionDistance = suspensionDistance;
            w.forwardFriction = DefaultFrictionCurve();
            w.sidewaysFriction = DefaultFrictionCurve();
            w.forceAppPointDistance = 0.05f;

            return w;
        }
    }
}

using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;

namespace Varneon.VUdon.VehiclesBase.Editor.Utilities
{
    public static class LandVehicleAnimationUtility
    {
        public static void BindAnimationDescriptor(ref AnimationClip clip, LandVehicleAnimationDescriptor descriptor)
        {
            Transform root = descriptor.GetComponentInParent<LandVehicleDescriptor>().transform;

            string path = AnimationUtility.CalculateTransformPath(descriptor.transform, root);

            Vector3 startRotation = descriptor.StartRotation;
            Vector3 endRotation = descriptor.EndRotation;

            BindAnimationDescriptor(ref clip, path, startRotation.x, endRotation.x, Axis.X);
            BindAnimationDescriptor(ref clip, path, startRotation.y, endRotation.y, Axis.Y);
            BindAnimationDescriptor(ref clip, path, startRotation.z, endRotation.z, Axis.Z);
        }

        private static void BindAnimationDescriptor(ref AnimationClip clip, string path, float start, float end, Axis axis)
        {
            clip.SetCurve(path, typeof(Transform), "localEulerAnglesRaw." + axis.ToString().ToLower(), AnimationCurve.Linear(0f, start, 1f, end));
        }
    }
}

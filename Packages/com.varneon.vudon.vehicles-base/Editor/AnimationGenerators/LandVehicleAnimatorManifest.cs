using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    public class LandVehicleAnimatorManifest
    {
        public readonly LandVehicleDescriptor Descriptor;

        public readonly string Directory;

        public readonly string Name;

        public readonly AnimatorController Controller;

        public readonly Dictionary<LandVehicleAnimationType, AnimationClip> Animations;

        public LandVehicleAnimatorManifest(LandVehicleDescriptor descriptor, string directory, string name, AnimatorController controller)
        {
            Descriptor = descriptor;
            Directory = directory;
            Name = name;
            Controller = controller;
            Animations = new Dictionary<LandVehicleAnimationType, AnimationClip>();
        }
    } 
}

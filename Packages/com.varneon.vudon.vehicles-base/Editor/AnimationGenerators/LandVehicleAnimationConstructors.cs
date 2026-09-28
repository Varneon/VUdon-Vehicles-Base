using System;
using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor.AssetGeneration
{
    /// <summary>
    /// Class for the constructors of all land vehicle animations
    /// </summary>
    public static class LandVehicleAnimationConstructors
    {
        public static AnimationClip Animation(LandVehicleAnimationType type)
        {
            switch (type)
            {
                case LandVehicleAnimationType.AbsoluteSpeed: return AbsoluteSpeedAnimation();
                case LandVehicleAnimationType.Brake: return BrakeAnimation();
                case LandVehicleAnimationType.InReverse: return InReverseAnimation();
                case LandVehicleAnimationType.RPM: return RPMAnimation();
                case LandVehicleAnimationType.Steering: return SteeringAnimation();
                case LandVehicleAnimationType.Throttle: return ThrottleAnimation();
                case LandVehicleAnimationType.RemoteSignedSpeed: return WheelRollAnimation();
                default: throw new NotImplementedException();
            }
        }

        public static AnimationClip AbsoluteSpeedAnimation() => new AnimationClip() { name = "AbsoluteSpeed", wrapMode = WrapMode.ClampForever };

        public static AnimationClip BrakeAnimation() => new AnimationClip() { name = "Brake", wrapMode = WrapMode.ClampForever };

        public static AnimationClip InReverseAnimation()
        {
            AnimationClip animation = new AnimationClip() { name = "InReverse", wrapMode = WrapMode.ClampForever };

            AnimationUtility.SetAnimationClipSettings(animation, new AnimationClipSettings() { loopTime = true });

            return animation;
        }

        public static AnimationClip RPMAnimation() => new AnimationClip() { name = "RPM", wrapMode = WrapMode.ClampForever };

        public static AnimationClip SteeringAnimation() => new AnimationClip() { name = "Steering", wrapMode = WrapMode.ClampForever };

        public static AnimationClip ThrottleAnimation() => new AnimationClip() { name = "Throttle", wrapMode = WrapMode.ClampForever };

        public static AnimationClip WheelRollAnimation()
        {
            AnimationClip animation = new AnimationClip() { name = "WheelRoll" };

            AnimationUtility.SetAnimationClipSettings(animation, new AnimationClipSettings() { loopTime = true });

            return animation;
        }
    }
}

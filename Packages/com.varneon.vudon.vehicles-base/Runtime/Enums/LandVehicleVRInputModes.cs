using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Enums
{
    public enum LandVehicleVRInputPreset
    {
        [InspectorName("Realistic (Grab Wheel, Triggers, Jump)")]
        Realistic,

        Simple,

        SimpleThumbsticks,

        [InspectorName("Right Controller Only (Realistic)")]
        NoLeftController,

        [InspectorName("Left Controller Only (Realistic)")]
        NoRightController,

        [InspectorName("Right Controller Only (Simple)")]
        NoLeftControllerSimple,

        [InspectorName("Left Controller Only (Simple)")]
        NoRightControllerSimple,
    }

    public enum LandVehicleVRSteeringMode
    {
        Realistic,
        MovementHorizontal,
        LookHorizontal
    }

    public enum LandVehicleVRThrottleMode
    {
        RightTrigger,
        LeftTrigger,
        LookUp,
        MoveForward
    }

    public enum LandVehicleVRBrakeMode
    {
        LeftTrigger,
        RightTrigger,
        LookDown,
        MoveBack
    }

    public enum LandVehicleVRHandbrakeMode
    {
        Jump,
        LeftUse,
        RightUse,
        LookDown,
        MoveBack
    }

    public enum LandVehicleVRGearShiftMode
    {
        Realistic,
        RightThumbstickVertical,
        RightThumbstickHorizontal,
        LeftThumbstickVertical,
        LeftThumbstickHorizontal
    }

    public enum LandVehicleVRGrabMode
    {
        Grip,
        Use
    }
}
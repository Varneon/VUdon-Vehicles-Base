using UnityEngine;

namespace Varneon.VUdon.VehiclesBase
{
    public enum LandVehicleAnimationType
    {
        [Tooltip("From standstill to fastest speed set by the vehicle's SpecSheet")]
        AbsoluteSpeed,
        [Tooltip("Normalized brake input")]
        Brake,
        [Tooltip("0: Forward gear, 1: Reverse gear")]
        InReverse,
        [Tooltip("Continuous rolling animation for wheels on remote clients")]
        RemoteSignedSpeed,
        [Tooltip("Normalized engine RPM determined by the vehicle's SpecSheet")]
        RPM,
        [Tooltip("0: Left, 0.5: Center, 1: Right")]
        Steering,
        [Tooltip("Normalized throttle input")]
        Throttle
    }
}

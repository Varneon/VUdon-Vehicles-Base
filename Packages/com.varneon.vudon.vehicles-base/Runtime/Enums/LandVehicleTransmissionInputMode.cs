using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Enums
{
    public enum LandVehicleTransmissionInputMode
    {
        [Tooltip("Automatic shifting to reverse from stationary when braking, emulated manual transmission shifting")]
        Arcade,
        [Tooltip("Keyboard inputs using Left Shift and Left Ctrl or VR thumbsticks")]
        Sequential,
        [Tooltip("Physical VR shifter or shifter game controller input")]
        Realistic
    }
}

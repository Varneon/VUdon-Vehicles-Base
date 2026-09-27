using UnityEngine;

namespace Varneon.VUdon.VehiclesBase
{
    public enum LandVehicleEmissiveRendererType
    {
        None,
        [Tooltip("The _EMISSION keyword will be enabled when the engine is running, disabled when not")]
        Default,
        [Tooltip("Same as Default, but the specified Color property will be adjusted to the desired color when brake is engaged")]
        BrakeLight,
        [Tooltip("The _EMISSION keyword will be enabled when the transmission is in reverse, disabled when not")]
        ReverseLight,
        [Tooltip("Same as Default, but uses the proprietary vehicle light shader to control all lights via single Vector4 property, multiplied by vertex colors at each given channel.\n\nR: Brake, G: Reverse, B: Headlights")]
        CombinedLightShader
    }
}

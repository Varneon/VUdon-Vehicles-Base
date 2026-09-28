using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining an emissive renderer that should be manipulated based on the provided criteria
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Emissive Renderer")]
    public class LandVehicleEmissiveRenderer : LandVehicleObjectDescriptor
    {
        public LandVehicleEmissiveRendererType RendererType = LandVehicleEmissiveRendererType.Default;

        public int MaterialIndex;

        [ColorUsage(false, true)]
        public Color ActiveColor = Color.white;

        public string ColorProperty = "_EmissionColor";

        public bool OverrideDefaultColor;

        [ColorUsage(false, true)]
        public Color DefaultColor = Color.black;
    }
}

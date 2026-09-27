using TMPro;
using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining a <see cref="TextMeshPro"/>-based gear indicator text that should be updated when the gear on the vehicle changes
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Gear Indicator")]
    [DisallowMultipleComponent]
    [ExcludeFromPreset]
    [RequireComponent(typeof(TextMeshPro))] // TODO: Add support for TMP_Text
    public class LandVehicleGearIndicatorDescriptor : LandVehicleObjectDescriptor { }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    [UnityEditor.CustomEditor(typeof(LandVehicleGearIndicatorDescriptor))]
    public class LandVehicleGearIndicatorDescriptorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("This descriptor is used to define a gear indicator text for a land vehicle.", UnityEditor.MessageType.Info);
        }
    }
#endif
}

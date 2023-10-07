using TMPro;
using UnityEngine;
using Varneon.VUdon.VehiclesBase.Interfaces;

namespace Varneon.VUdon.VehiclesBase
{
    [RequireComponent(typeof(TextMeshPro))]
    [DisallowMultipleComponent]
    [AddComponentMenu("VUdon/Vehicles/Descriptors/Land Vehicle Gear Indicator")]
    public class LandVehicleGearIndicatorDescriptor : MonoBehaviour, IDestroyOnBuild { }

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

using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    [DisallowMultipleComponent]
    [AddComponentMenu("VUdon/Vehicles/Descriptors/Center Of Mass Descriptor")]
    public class LandVehicleCenterOfMassDescriptor : LandVehicleObjectDescriptor { }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    [UnityEditor.CustomEditor(typeof(LandVehicleCenterOfMassDescriptor))]
    public class LandVehicleCenterOfMassDescriptorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("This descriptor is used to define the center of mass for a land vehicle.", UnityEditor.MessageType.Info);
        }
    }
#endif
}

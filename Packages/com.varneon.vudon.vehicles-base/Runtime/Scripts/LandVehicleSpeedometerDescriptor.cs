using System;
using TMPro;
using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining a <see cref="TextMeshPro"/>-based speedometer text that should be updated when the speed of the vehicle changes
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Speedometer")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(TextMeshPro))]
    public class LandVehicleSpeedometerDescriptor : LandVehicleObjectDescriptor
    {
        private TextMeshPro SpeedometerText
        {
            get
            {
                if(speedometerText == null)
                {
                    speedometerText = GetComponent<TextMeshPro>();
                }

                return speedometerText;
            }
        }

        [NonSerialized]
        private TextMeshPro speedometerText;

        public string SpeedometerStringTemplate = "<mspace=64>{0}</mspace>";

        [Header("Debug")]
        [Range(0f, 1f)]
        public float TestValue;

        private void OnValidate()
        {
            if (SpeedometerText)
            {
                SpeedometerText.text = string.Format(SpeedometerStringTemplate, Mathf.RoundToInt(Mathf.Pow(TestValue, 2f) * 999f));
            }
        }
    }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    [UnityEditor.CustomEditor(typeof(LandVehicleSpeedometerDescriptor))]
    public class LandVehicleSpeedometerDescriptorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("This descriptor is used to define a digital speedometer text for a land vehicle.", UnityEditor.MessageType.Info);

            base.OnInspectorGUI();
        }
    }
#endif
}

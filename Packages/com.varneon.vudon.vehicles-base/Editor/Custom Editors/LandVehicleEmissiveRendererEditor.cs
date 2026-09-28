using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LandVehicleEmissiveRenderer))]
    public class LandVehicleEmissiveRendererEditor : UnityEditor.Editor
    {
        private string[] materialNames;

        SerializedProperty
            rendererTypeProperty,
            materialIndexProperty,
            colorPropertyProperty,
            activeColorProperty;

        private void OnEnable()
        {
            LandVehicleEmissiveRenderer renderer = (LandVehicleEmissiveRenderer)target;

            rendererTypeProperty = serializedObject.FindProperty(nameof(LandVehicleEmissiveRenderer.RendererType));
            materialIndexProperty = serializedObject.FindProperty(nameof(LandVehicleEmissiveRenderer.MaterialIndex));
            colorPropertyProperty = serializedObject.FindProperty(nameof(LandVehicleEmissiveRenderer.ColorProperty));
            activeColorProperty = serializedObject.FindProperty(nameof(LandVehicleEmissiveRenderer.ActiveColor));

            if(renderer.TryGetComponent(out Renderer r))
            {
                materialNames = r.GetComponent<Renderer>().sharedMaterials.Select((m, index) => string.Concat("[", index, "] ", m.name)).ToArray();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            LandVehicleEmissiveRenderer renderer = (LandVehicleEmissiveRenderer)target;

            if(materialNames == null)
            {
                EditorGUILayout.HelpBox("A Renderer must be attached to this object in order!", MessageType.Error);

                return;
            }

            EditorGUILayout.PropertyField(rendererTypeProperty);

            Rect rect = EditorGUILayout.GetControlRect();
            EditorGUI.BeginProperty(rect, new GUIContent(materialIndexProperty.displayName), materialIndexProperty);
            materialIndexProperty.intValue = EditorGUI.Popup(rect, "Material", renderer.MaterialIndex, materialNames);
            EditorGUI.EndProperty();

            switch ((LandVehicleEmissiveRendererType)rendererTypeProperty.enumValueIndex)
            {
                case LandVehicleEmissiveRendererType.BrakeLight:
                    EditorGUILayout.DelayedTextField(colorPropertyProperty);

                    EditorGUILayout.PropertyField(activeColorProperty);

                    EditorGUILayout.HelpBox("This descriptor will tell VUdon Vehicles SDK to set the material's provided color property value to the color above when the brake is pressed.", MessageType.Info);
                    break;
                case LandVehicleEmissiveRendererType.Default:
                    EditorGUILayout.HelpBox("This descriptor will tell VUdon Vehicles SDK to set the emissive active on the material instance based on engine's running state.", MessageType.Info);
                    break;
                case LandVehicleEmissiveRendererType.ReverseLight:
                    EditorGUILayout.HelpBox("This descriptor will tell VUdon Vehicles SDK to set the emissive active on the material instance when the transmission in on reverse gear.", MessageType.Info);
                    break;
                case LandVehicleEmissiveRendererType.CombinedLightShader:
                    EditorGUILayout.HelpBox("This descriptor will tell VUdon Vehicles SDK to feed combined light data to the material using the provided vehicle light shader.", MessageType.Info);
                    break;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

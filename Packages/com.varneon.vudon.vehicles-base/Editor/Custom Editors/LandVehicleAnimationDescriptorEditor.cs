using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LandVehicleAnimationDescriptor))]
    public class LandVehicleAnimationDescriptorEditor : UnityEditor.Editor
    {
        private SerializedProperty
            animationTypeProperty,
            startRotationProperty,
            endRotationProperty;

        private void OnEnable()
        {
            animationTypeProperty = serializedObject.FindProperty(nameof(LandVehicleAnimationDescriptor.AnimationType));
            startRotationProperty = serializedObject.FindProperty(nameof(LandVehicleAnimationDescriptor.StartRotation));
            endRotationProperty = serializedObject.FindProperty(nameof(LandVehicleAnimationDescriptor.EndRotation));
        }

        public override void OnInspectorGUI()
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;

            serializedObject.Update();

            EditorGUILayout.PropertyField(animationTypeProperty);

            Rect rect = EditorGUILayout.GetControlRect(GUILayout.Height(lineHeight));
            rect.width -= 22f;
            EditorGUI.PropertyField(rect, startRotationProperty);
            rect.x += rect.width + 2f;
            rect.width = 20f;
            if(GUI.Button(rect, "A"))
            {
                startRotationProperty.vector3Value = ((LandVehicleAnimationDescriptor)serializedObject.targetObject).transform.localEulerAngles;
            }

            rect = EditorGUILayout.GetControlRect(GUILayout.Height(lineHeight));
            rect.width -= 22f;
            EditorGUI.PropertyField(rect, endRotationProperty);
            rect.x += rect.width + 2f;
            rect.width = 20f;
            if (GUI.Button(rect, "A"))
            {
                endRotationProperty.vector3Value = ((LandVehicleAnimationDescriptor)serializedObject.targetObject).transform.localEulerAngles;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

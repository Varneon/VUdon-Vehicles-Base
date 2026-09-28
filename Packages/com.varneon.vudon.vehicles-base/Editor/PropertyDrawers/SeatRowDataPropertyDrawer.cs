using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    [CustomPropertyDrawer(typeof(LandVehicleDescriptor.SeatRowData))]
    public class SeatRowDataPropertyDrawer : PropertyDrawer
    {
        private static GUIContent iconContent = EditorGUIUtility.IconContent("d_Transform Icon");

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 4f + 10f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;

            Rect controlRect = new Rect(position.x, position.y, position.width, lineHeight);

            EditorGUI.PropertyField(controlRect, property.FindPropertyRelative("SeatCount"));

            controlRect.y += lineHeight + 2f;
            Rect toggleRect = new Rect(controlRect.x + controlRect.width - 20f, controlRect.y, 20f, controlRect.height);
            controlRect.width -= 22f;
            EditorGUI.PropertyField(controlRect, property.FindPropertyRelative("Position"));
            DrawToggleButton(toggleRect, property.FindPropertyRelative("EditingPosition"));

            controlRect.y += lineHeight + 2f;
            toggleRect.y += lineHeight + 2f;
            EditorGUI.PropertyField(controlRect, property.FindPropertyRelative("HandlePosition"));
            DrawToggleButton(toggleRect, property.FindPropertyRelative("EditingHandlePosition"));

            controlRect.y += lineHeight + 2f;
            controlRect.width += 22f;
            EditorGUI.PropertyField(controlRect, property.FindPropertyRelative("Rotation"));
        }

        private void DrawToggleButton(Rect position, SerializedProperty property)
        {
            GUI.color = property.boolValue ? Color.red : Color.green;
            if (GUI.Button(position, iconContent, EditorStyles.helpBox))
            {
                property.boolValue ^= true;
            }

            GUI.color = Color.white;
        }
    }
}

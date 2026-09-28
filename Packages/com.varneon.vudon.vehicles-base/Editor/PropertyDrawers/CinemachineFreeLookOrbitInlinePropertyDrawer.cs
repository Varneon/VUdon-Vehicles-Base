using Cinemachine;
using System;
using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    [CustomPropertyDrawer(typeof(CinemachineFreeLookOrbitInlinePropertyDrawerAttribute))]
    public class CinemachineFreeLookOrbitInlinePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;

            Rect controlRect = new Rect(position.x, position.y, position.width, lineHeight);

            controlRect = EditorGUI.PrefixLabel(controlRect, new GUIContent(property.displayName));

            controlRect.width /= 2f;

            EditorGUI.PropertyField(controlRect, property.FindPropertyRelative("m_Height"));

            controlRect.x += controlRect.width;

            EditorGUI.PropertyField(controlRect, property.FindPropertyRelative("m_Radius"));
        }
    }
}

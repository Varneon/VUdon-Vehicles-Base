using UnityEditor;
using UnityEngine;
using Varneon.VUdon.VehiclesBase.Editor.Utilities;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    [CustomPropertyDrawer(typeof(DirectoryPickerContextButtonAttribute))]
    public class DirectoryPickerContextButtonPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if(property.propertyType == SerializedPropertyType.String)
            {
                Rect buttonRect = new Rect(position.x + position.width - 20f, position.y, 20f, position.height);
                position.width -= 22f;
                EditorGUI.PropertyField(position, property, label);
                if (GUI.Button(buttonRect, GUIContent.none))
                {
                    EditorApplication.delayCall += () =>
                    {
                        string path = EditorUtility.SaveFolderPanel("Choose Directory", Application.dataPath, "");

                        if (!string.IsNullOrWhiteSpace(path))
                        {
                            property.stringValue = PathUtility.ConvertToRelativePath(path);

                            property.serializedObject.ApplyModifiedProperties();
                        }
                    };
                }
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}

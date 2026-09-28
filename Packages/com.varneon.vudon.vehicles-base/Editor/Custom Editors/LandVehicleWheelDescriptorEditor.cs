using UnityEditor;
using UnityEngine;
using Varneon.VUdon.Editors.Editor;
using Varneon.VUdon.VehiclesBase.Editor.Utilities;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LandVehicleWheelDescriptor))]
    public class LandVehicleWheelDescriptorEditor : InspectorBase
    {
        private Transform descriptorTransform;

        protected override string PersistenceKey => "Varneon/VUdon/VehiclesBase/WheelDescriptor/Editor/Foldouts";

        protected override InspectorHeader Header => null;

        private void DrawLabel(Vector3 point, string text, string tooltip)
        {
            GUIContent content = new GUIContent(text, tooltip);

            Rect rect = HandleUtility.WorldPointToSizedRect(point, content, EditorStyles.textArea);

            rect.position += rect.size * new Vector2(-0.5f, -1f) + new Vector2(4f, 0f);

            GUI.Label(rect, content, EditorStyles.textArea);
        }

        protected override void OnPreDrawFields()
        {
            LandVehicleWheelDescriptor descriptor = (LandVehicleWheelDescriptor)target;

            Transform root = descriptor.Root;

            if (!root)
            {
                EditorGUILayout.HelpBox("This vehicle's root doesn't have LandVehicleDescriptor! Please add one to access the descriptor properties.", MessageType.Error);

                return;
            }

            EditorGUILayout.HelpBox("This descriptor is used to define a wheel for a land vehicle.", MessageType.Info);
        }

        protected override void OnPostDrawFields()
        {
            LandVehicleWheelDescriptor descriptor = (LandVehicleWheelDescriptor)target;

            if (!descriptor.IsRotationValid)
            {
                EditorGUILayout.HelpBox("Ensure that wheel rotation is zero by default!", MessageType.Error);
            }
        }

        private void OnSceneGUI()
        {
            LandVehicleWheelDescriptor descriptor = (LandVehicleWheelDescriptor)target;

            descriptorTransform = descriptor.transform;

            Transform root = descriptor.Root;

            if (!root) { return; }

            Quaternion rotation = descriptorTransform.rotation;

            Vector3 position = descriptorTransform.position;

            float radius = descriptor.Radius;

            float width = descriptor.Width;

            float halfWidth = width / 2f;

            float horizontalOffset = descriptor.HorizontalOffset;

            // Draw the preview of effective contact patch
            using (new Handles.DrawingScope(Color.green, descriptor.transform.localToWorldMatrix))
            {
                float fillet = halfWidth - (halfWidth * descriptor.ContactFraction);

                float left = halfWidth - fillet + horizontalOffset;
                float right = -halfWidth + fillet + horizontalOffset;

                Handles.DrawWireDisc(new Vector3(left, 0f, 0f), Vector3.right, radius);
                Handles.DrawWireDisc(new Vector3(right, 0f, 0f), Vector3.right, radius);

                for(int i = 0; i < 16; i++)
                {
                    float angle = i / 8f * Mathf.PI;

                    float sin = Mathf.Sin(angle) * radius;
                    float cos = Mathf.Cos(angle) * radius;

                    Handles.DrawLine(new Vector3(left, sin, cos), new Vector3(right, sin, cos));
                }

                Handles.color = Color.HSVToRGB(1f / 3f * descriptor.ContactFraction, 1f, 1f);
                Handles.DrawWireDisc(new Vector3(halfWidth + horizontalOffset, 0f, 0f), Vector3.right, radius - fillet);
                Handles.DrawWireDisc(new Vector3(-halfWidth + horizontalOffset, 0f, 0f), Vector3.right, radius - fillet);

                using (EditorGUI.ChangeCheckScope changedScope = new EditorGUI.ChangeCheckScope())
                {
                    Vector3 rightPos = Vector3.right * (halfWidth + horizontalOffset);

                    Vector3 rightSlider = Handles.Slider(rightPos, Vector3.right, HandleUtility.GetHandleSize(rightPos) * 0.05f, Handles.DotHandleCap, 0.01f);

                    if (changedScope.changed)
                    {
                        float delta = (rightSlider - rightPos).x;

                        Undo.RecordObject(descriptor, "Adjust Wheel Size");
                        descriptor.Width += delta;
                        descriptor.HorizontalOffset += delta / 2f;
                    }
                }

                using (EditorGUI.ChangeCheckScope changedScope = new EditorGUI.ChangeCheckScope())
                {
                    Vector3 leftPos = Vector3.right * (-halfWidth + horizontalOffset);

                    Vector3 leftSlider = Handles.Slider(leftPos, Vector3.left, HandleUtility.GetHandleSize(leftPos) * 0.05f, Handles.DotHandleCap, 0.01f);

                    if (changedScope.changed)
                    {
                        float delta = (leftSlider - leftPos).x;

                        Undo.RecordObject(descriptor, "Adjust Wheel Size");
                        descriptor.Width -= delta;
                        descriptor.HorizontalOffset += delta / 2f;
                    }
                }
            }

            Handles.BeginGUI();
            DrawLabel(position + rotation * new Vector3(horizontalOffset, radius + 0.05f, 0f), string.Concat("Radius: ", Mathf.Round(radius * 1000f), " mm\nWidth: ", Mathf.Round(width * 1000f), " mm\nBrake Torque: ", descriptor.BrakeTorque, " Nm\nDriven: ", descriptor.Driven, "\nSteered: ", descriptor.Steered), string.Empty);
            Handles.EndGUI();

            WheelGUIUtility.DrawWheelPreviewHandles(descriptorTransform, descriptor.ColliderOffset, descriptor.SuspensionDistance, descriptor.Radius);
        }
    }
}

using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining a steering wheel transform on a vehicle
    /// </summary>
    /// <remarks>
    /// If the source vehicle model was created according to established standards, user will never have to add this manually, as the hierarchy scan will add this automatically when <see cref="LandVehicleDescriptor"/> is added
    /// </remarks>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Steering Wheel Descriptor")]
    [DisallowMultipleComponent]
    public class LandVehicleSteeringWheelDescriptor : LandVehicleObjectDescriptor
    {
        [Range(0.01f, 1f)]
        public float Radius = 0.175f;

        [Range(0.01f, 0.05f)]
        public float Thickness = 0.015f;

        [Tooltip("Range in degrees which the steering will rotate from one end to another.")]
        [Range(10f, 2000f)]
        public float Range = 1080f;

        [Tooltip("Maximum distance for grabbing steering wheel.")]
        [Range(0.05f, 0.15f)]
        public float GrabThresholdDistance = 0.075f;

        [Tooltip("Distance at which steering wheel will automatically be let go.")]
        [Range(0.1f, 0.3f)]
        public float DetachThresholdDistance = 0.15f;

        [Tooltip("Steering wheel mesh renderer to be used as a reference for showing a grabbing proximity highlight in VR.\n\nIt is recommended to use the highest LOD mesh if multiple are available.")]
        public MeshRenderer HighlightRendererReference;

        [Tooltip("If the model already has a root for ensuring zeroed default rotation of the steering wheel proxy, manually assigning it here will prevent creation of new root automatically.")]
        public Transform CustomRoot;
    }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    [UnityEditor.CustomEditor(typeof(LandVehicleSteeringWheelDescriptor))]
    public class LandVehicleSteeringWheelDescriptorEditor : UnityEditor.Editor
    {
        private LandVehicleSteeringWheelDescriptor descriptor;

        private Transform descriptorTransform;

        private Quaternion lastRot;

        private bool isValidRotationAngle;

        private bool isValidRotationAxis;

        private Vector3 eulerAngles;

        private void OnEnable()
        {
            descriptor = (LandVehicleSteeringWheelDescriptor)target;

            descriptorTransform = descriptor.transform;

            lastRot = descriptorTransform.rotation;
        }

        private void DrawLabel(Vector3 point, string text, string tooltip)
        {
            GUIContent content = new GUIContent(text, tooltip);

            Rect rect = UnityEditor.HandleUtility.WorldPointToSizedRect(point, content, UnityEditor.EditorStyles.textArea);

            rect.position += rect.size * new Vector2(-0.5f, -1f) + new Vector2(4f, 0f);

            GUI.Label(rect, content, UnityEditor.EditorStyles.textArea);
        }

        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("This descriptor is used to define the steering wheel for a land vehicle.", UnityEditor.MessageType.Info);

            base.OnInspectorGUI();

            if (!isValidRotationAxis)
            {
                UnityEditor.EditorGUILayout.HelpBox("Do not rotate the steering wheel on Y or Z axis, only X axis rotation is allowed!", UnityEditor.MessageType.Error);
            }

            if (!isValidRotationAngle)
            {
                UnityEditor.EditorGUILayout.HelpBox("Keep X axis rotation between 0 and 90!", UnityEditor.MessageType.Error);
            }
        }

        private void OnSceneGUI()
        {
            Quaternion rotation = descriptorTransform.rotation;

            eulerAngles = (Quaternion.Inverse(descriptorTransform.root.rotation) * rotation).eulerAngles;

            isValidRotationAxis = Mathf.Round(eulerAngles.y) == 0f && Mathf.Round(eulerAngles.z) == 0f;

            isValidRotationAngle = eulerAngles.x >= 0f && eulerAngles.x < 90f;

            using (UnityEditor.Handles.DrawingScope scope = new UnityEditor.Handles.DrawingScope(isValidRotationAxis && isValidRotationAngle ? Color.cyan : Color.red))
            {
                Vector3 position = descriptorTransform.position;

                Vector3 direction = rotation * Vector3.forward;

                float radius = UnityEditor.Handles.RadiusHandle(rotation, position, descriptor.Radius, true);

                if(radius != descriptor.Radius)
                {
                    UnityEditor.Undo.RecordObject(descriptor, "Adjust Steering Wheel Radius");

                    descriptor.Radius = Mathf.Clamp(radius, 0.01f, 1f);
                }

                float thickness = UnityEditor.Handles.ScaleValueHandle(descriptor.Thickness, position + rotation * new Vector3(0f, descriptor.Radius, 0f), rotation * Quaternion.Euler(0f, 90f, 0f), descriptor.Thickness * 6.666f, UnityEditor.Handles.CircleHandleCap, 0.005f);

                if (thickness != descriptor.Thickness)
                {
                    UnityEditor.Undo.RecordObject(descriptor, "Adjust Steering Wheel Thickness");

                    descriptor.Thickness = Mathf.Clamp(thickness, 0.01f, 0.05f);
                }

                UnityEditor.Handles.DrawWireDisc(position, direction, radius - thickness);
                UnityEditor.Handles.DrawWireDisc(position, direction, radius + thickness);
                UnityEditor.Handles.DrawWireDisc(position + rotation * new Vector3(0f, 0f, thickness), direction, radius);
                UnityEditor.Handles.DrawWireDisc(position - rotation * new Vector3(0f, 0f, thickness), direction, radius);

                UnityEditor.Handles.BeginGUI();
                DrawLabel(position + rotation * new Vector3(0f, radius + thickness + 0.05f, 0f), string.Concat("Radius: ", Mathf.Round(radius * 1000f), "mm\nThickness: ", Mathf.Round(thickness * 1000f), " mm\nRange: ", descriptor.Range, "°"), "Tooltip");
                UnityEditor.Handles.EndGUI();

            }
        }
    }
#endif
}

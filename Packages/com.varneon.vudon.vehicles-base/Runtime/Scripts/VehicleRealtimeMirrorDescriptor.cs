using System;
using System.Linq;
using UnityEngine;
using Varneon.VSDK;
using Object = UnityEngine.Object;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining the root GameObject of realtime mirrors that should be activated when the user is in first person and has mirrors enabled in preferences
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Realtime Mirror Descriptor")]
    [ExcludeFromPreset]
    [DisallowMultipleComponent]
    public class VehicleRealtimeMirrorDescriptor : MonoBehaviour, VSDK.Interfaces.IDestroyOnBuild
    {
        public enum AlignmentAxis
        {
            Horizontal,
            Vertical
        }

        [Serializable]
        public class Mirror
        {
            public Camera Camera;

            public MeshRenderer Renderer;

            [Tooltip("Angle relative to this Transform at which the camera should be oriented")]
            public Vector3 Angle = new Vector3(0f, 180f, 0f);

            [Tooltip("Should the gate of the camera be adjusted based on the mirror mesh bounding box's horizontal or vertical axis")]
            public AlignmentAxis AlignmentAxis = AlignmentAxis.Horizontal;

            public Mirror(Camera camera, MeshRenderer renderer)
            {
                Camera = camera;
                Renderer = renderer;
            }
        }

        public Mirror[] Mirrors = new Mirror[0];

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        [ContextMenu("Auto-Detect Mirrors")]
        internal void AutoDetectMirrors()
        {
            UnityEditor.Undo.RecordObject(this, "Auto-Detect Mirrors");

            Camera[] cameras = GetComponentsInChildren<Camera>(true);

            Mirrors = cameras.Select(c => new Mirror(c, c.GetComponentInParent<MeshRenderer>(true))).ToArray();
        }

        [ContextMenu("Align Cameras")]
        private void AlignCameras()
        {
            for (int i = 0; i < Mirrors.Length; i++)
            {
                Mirror mirror = Mirrors[i];

                Camera c = mirror.Camera;

                Transform t = c.transform;

                UnityEditor.Undo.RecordObjects(new Object[] { t, c }, "Align Cameras");

                Renderer r = t.GetComponentInParent<Renderer>();

                Bounds b = r.localBounds;

                Quaternion angle = Quaternion.Euler(mirror.Angle);

                bool alignHorizontally = mirror.AlignmentAxis == AlignmentAxis.Horizontal;

                float size = alignHorizontally ? b.size.x : b.size.y;

                float fov = alignHorizontally ? c.fieldOfView : Camera.VerticalToHorizontalFieldOfView(c.fieldOfView, c.aspect);

                float near = size / Mathf.Tan(Mathf.Deg2Rad * fov / 2f) / 2f;
                if (alignHorizontally)
                {
                    near /= c.aspect;
                }
                else
                {
                    near *= c.aspect;
                }

                c.nearClipPlane = near;

                t.SetLocalPositionAndRotation(b.center + Vector3.back * b.size.z / 2f + angle * Vector3.back * near, angle);
            }
        }

        private void OnDrawGizmosSelected()
        {
            using (GizmosDrawingScope scope = new GizmosDrawingScope(Color.green, transform.localToWorldMatrix))
            {
                foreach (Mirror mirror in Mirrors)
                {
                    Camera c = mirror.Camera;

                    if(c == null) { continue; }

                    Gizmos.matrix = c.transform.localToWorldMatrix;

                    Gizmos.DrawFrustum(Vector3.zero, c.fieldOfView, c.farClipPlane, c.nearClipPlane, c.aspect);
                }
            }
        }
#endif
    }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    [UnityEditor.CustomEditor(typeof(VehicleRealtimeMirrorDescriptor))]
    public class VehicleRealtimeMirrorDescriptorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            UnityEditor.EditorGUILayout.HelpBox("This descriptor is used to define realtime mirror(s) for a vehicle.", UnityEditor.MessageType.Info);

            base.OnInspectorGUI();
        }
    }
#endif
}

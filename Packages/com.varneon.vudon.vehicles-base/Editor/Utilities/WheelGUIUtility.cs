using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor.Utilities
{
    public static class WheelGUIUtility
    {
        public static void DrawWheelPreviewHandles(Transform wheel, Vector3 offset, float suspensionDistance, float radius)
        {
            if (wheel == null) { return; }

            Vector3 wheelPos = wheel.TransformPoint(offset.x, 0f, 0f);

            // Draw the wheel at the original position
            Handles.color = Color.green;
            Handles.DrawWireDisc(wheelPos, wheel.right, radius);
            Handles.color = Color.white;

            float xOffset = offset.x;

            float yOffset = offset.y;

            float zOffset = offset.z;

            float yOffsetWithDistance = yOffset - suspensionDistance;

            Handles.color = Color.yellow;
            Handles.DrawLine(wheel.TransformPoint(xOffset, yOffset, zOffset - 0.1f), wheel.TransformPoint(xOffset, yOffset, zOffset + 0.1f));
            Handles.DrawLine(wheel.TransformPoint(xOffset, yOffset, zOffset), wheel.TransformPoint(xOffset, yOffsetWithDistance, zOffset));
            Handles.DrawLine(wheel.TransformPoint(xOffset, yOffsetWithDistance, zOffset - 0.1f), wheel.TransformPoint(xOffset, yOffsetWithDistance, zOffset + 0.1f));
            Handles.color = Color.white;

            // Draw the extended and compressed reach of the wheel
            Handles.DrawWireArc(wheel.TransformPoint(xOffset, yOffsetWithDistance, zOffset), wheel.right, wheel.TransformDirection(0f, -1f, 1f), 90f, radius);
            Handles.DrawWireArc(wheel.TransformPoint(xOffset, yOffset, zOffset), wheel.right, wheel.TransformDirection(0f, 1f, -1f), 90f, radius);
        }

        public static void DrawWheelMeshRollingPreviewHandles(Transform wheel, float radius, Transform root)
        {
            if(wheel == null) { return; }

            using (Handles.DrawingScope scope = new Handles.DrawingScope(Matrix4x4.TRS(wheel.position, root.rotation, root.lossyScale)))
            {
                Vector3 wheelPos = wheel.position;

                float halfRadius = radius / 2f;

                float doubleRadius = radius * 2f;

                // Draw the wheel at the original position
                Handles.color = Color.green;
                Handles.DrawWireDisc(Vector3.zero, Vector3.right, radius);

                Handles.color = Color.cyan;
                Handles.DrawLine(new Vector3(0f, -radius, halfRadius), new Vector3(0f, -radius, doubleRadius));
                Handles.DrawLine(new Vector3(0f, -radius, doubleRadius), new Vector3(0.1f, -radius, doubleRadius - 0.1f));
                Handles.DrawLine(new Vector3(0f, -radius, doubleRadius), new Vector3(-0.1f, -radius, doubleRadius - 0.1f));

                Handles.DrawWireArc(Vector3.zero, Vector3.right, Vector3.down, 270f, halfRadius);

                Handles.DrawLine(new Vector3(0f, 0f, halfRadius), new Vector3(0f, 0.1f, halfRadius - 0.1f));
                Handles.DrawLine(new Vector3(0f, 0f, halfRadius), new Vector3(0f, 0.1f, halfRadius + 0.1f));

                Handles.color = Color.white;
            }
        }
    }
}

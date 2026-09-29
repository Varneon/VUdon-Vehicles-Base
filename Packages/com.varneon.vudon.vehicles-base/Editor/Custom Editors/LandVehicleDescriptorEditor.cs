using UnityEditor;
using UnityEngine;
using Varneon.VUdon.Editors.Editor;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    [CustomEditor(typeof(LandVehicleDescriptor))]
    public class LandVehicleDescriptorEditor : InspectorBase
    {
        private LandVehicleDescriptor descriptor;

        [SerializeField]
        private Texture2D headerIcon;

        private bool editingAudioPositions;

        protected override InspectorHeader Header => new InspectorHeaderBuilder()
            .WithTitle("VUdon - Vehicles - Base")
            .WithDescription("Land Vehicle Descriptor")
            .WithIcon(headerIcon)
            .Build();

        private const string
            avatarGUID = "653725390b448564b9092af425c55e28";

        protected override void OnEnable()
        {
            descriptor = (LandVehicleDescriptor)target;

            base.OnEnable();

            TryLoadPreviewAvatarAssets();
        }

        private bool TryLoadPreviewAvatarAssets()
        {
            string avatarPath = AssetDatabase.GUIDToAssetPath(avatarGUID);

            if (string.IsNullOrWhiteSpace(avatarPath)) { return false; }

            descriptor.avatarMesh = AssetDatabase.LoadAssetAtPath<Mesh>(avatarPath);

            return true;
        }

        protected override void OnPostDrawFields()
        {
            if (editingAudioPositions)
            {
                if (GUILayout.Button("Stop Editing Audio Positions")) { editingAudioPositions = false; }
            }
            else
            {
                if (GUILayout.Button("Start Editing Audio Positions")) { editingAudioPositions = true; }
            }
        }

        private void OnSceneGUI()
        {
            LandVehicleDescriptor descriptor = (LandVehicleDescriptor)target;

            using (new Handles.DrawingScope(descriptor.transform.localToWorldMatrix))
            {
                if (descriptor.OverrideThirdPersonView)
                {
                    Vector3 offset = new Vector3(0f, 0f, descriptor.LookPosition.z);

                    Handles.DrawWireDisc(Vector3.up * descriptor.OrbitTop.m_Height + offset, Vector3.up, descriptor.OrbitTop.m_Radius);
                    Handles.DrawWireDisc(Vector3.up * descriptor.OrbitMiddle.m_Height + offset, Vector3.up, descriptor.OrbitMiddle.m_Radius);
                    Handles.DrawWireDisc(Vector3.up * descriptor.OrbitBottom.m_Height + offset, Vector3.up, descriptor.OrbitBottom.m_Radius);
                }

                Undo.RecordObject(descriptor, "Adjust Seat Positions");

                for (int i = 0; i < descriptor.SeatRows.Length; i++)
                {
                    LandVehicleDescriptor.SeatRowData seatRow = descriptor.SeatRows[i];

                    if (seatRow.EditingPosition)
                    {
                        seatRow.Position = Handles.PositionHandle(seatRow.Position, Quaternion.identity);
                    }

                    if (seatRow.EditingHandlePosition)
                    {
                        seatRow.HandlePosition = Handles.PositionHandle(seatRow.HandlePosition, Quaternion.identity);
                    }

                    descriptor.SeatRows[i] = seatRow;
                }
            }

            if (editingAudioPositions)
            {
                using (new Handles.DrawingScope(descriptor.transform.localToWorldMatrix))
                {
                    Handles.BeginGUI();

                    Handles.Label(descriptor.EnginePosition, "Engine", EditorStyles.textArea);

                    Handles.Label(descriptor.ExhaustPosition, "Exhaust", EditorStyles.textArea);

                    Handles.Label(descriptor.GearShifterPosition, "Gear Shifter", EditorStyles.textArea);

                    Handles.Label(descriptor.CenterOfMass, "Center Of Mass", EditorStyles.textArea);

                    Handles.EndGUI();

                    Undo.RecordObject(descriptor, "Adjust Audio Position");

                    descriptor.EnginePosition = Handles.PositionHandle(descriptor.EnginePosition, Quaternion.identity);

                    descriptor.ExhaustPosition = Handles.PositionHandle(descriptor.ExhaustPosition, Quaternion.identity);

                    descriptor.GearShifterPosition = Handles.PositionHandle(descriptor.GearShifterPosition, Quaternion.identity);

                    descriptor.CenterOfMass = Handles.PositionHandle(descriptor.CenterOfMass, Quaternion.identity);
                }
            }
        }
    }
}

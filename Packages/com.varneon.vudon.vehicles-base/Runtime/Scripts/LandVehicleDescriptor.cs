using Cinemachine;
using System;
using UnityEngine;
using Varneon.VSDK;
using Varneon.VUdon.Editors;
using Varneon.VUdon.VehiclesBase.Abstract;
using Varneon.VUdon.VehiclesBase.DataPresets;
using Varneon.VUdon.VehiclesBase.Enums;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for non-destructively defining a drivable vehicle by setting up wheels, steering wheel, seats, engine, transmission, differential, audio, etc.
    /// </summary>
    /// <remarks>
    /// This descriptor is the entry point for each new vehicle at the root after the source vehicle model has standardized wheels and steering wheel, everything else can be adjusted later
    /// </remarks>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Descriptor")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class LandVehicleDescriptor : LandVehicleObjectDescriptor
    {
        [Serializable]
        public struct SeatRowData
        {
            [Min(1)]
            [Tooltip("How many seats should this row have")]
            public int SeatCount;

            [Tooltip("Position of the first seat on the row.\n\nSecond seat on rows with 2 seats will use this position as mirrored.\n\nRows with more seats will interpolate the remaining seats between the first two.")]
            public Vector3 Position;

            [Tooltip("Position where the player will have to click in order to get in the seat")]
            public Vector3 HandlePosition;

            [Tooltip("Rotation of the seat on the vehicle's local Y-axis (up).")]
            public float Rotation;

            /// <summary>
            /// The user is currently adjusting the position of the first seat
            /// </summary>
            public bool EditingPosition;

            /// <summary>
            /// The user is currently adjusting the position of the first seat's handle
            /// </summary>
            public bool EditingHandlePosition;

            public SeatRowData(int seatCount = 2, float position = -0.5f)
            {
                SeatCount = seatCount;

                Position = new Vector3(position, 0f, 0f);

                HandlePosition = new Vector3(position - 0.5f, 0.75f, 0f);

                Rotation = 0f;

                EditingPosition = false;

                EditingHandlePosition = false;
            }

            public SeatRowData(int seatCount = 2, Vector3 position = default)
            {
                SeatCount = seatCount;

                Position = position;

                HandlePosition = new Vector3(position.x - 0.5f, 0.75f, 0f);

                Rotation = 0f;

                EditingPosition = false;

                EditingHandlePosition = false;
            }

            public readonly Vector3 GetPosition(int index)
            {
                if(index >= SeatCount) { return Vector3.zero; }

                float position = Position.x;

                float fraction = (SeatCount == 1) ? 0f : position * 2f / ((float)SeatCount - 1f);

                return Position + fraction * index * Vector3.left;
            }

            public readonly Quaternion GetRotation(int index)
            {
                if(Rotation == 0f) { return Quaternion.identity; }

                return Quaternion.Euler(0f, index % 2 == 0 ? Rotation : -Rotation, 0f);
            }
        }

        [FoldoutHeader("Configuration")]
        [Tooltip("Spec sheet containing the necessary source data for generating the Rigidbody, engine and drivetrain.\n\nIt is recommended that the spec sheets are as true to the real life counterparts as possible, and using the additional properties to adjust the physics to be enjoyable in-game.")]
        public CarSpecSheet SpecSheet;

        [DirectoryPickerContextButton]
        [Tooltip("Directory in the project where the prefab assets will be stored")]
        public string ProjectPath = "Assets/-/Vehicles/Projects/VehicleProject";

        [Tooltip("Position for overriding the vehicle's Rigidbody's center of mass")]
        public Vector3 CenterOfMass;

        [Tooltip("Should the feet be pointing forward as in normal passenger cars, or downwards in taller vehicles such as trucks and buses")]
        public SeatOrientationMode SeatOrientation;

        [FoldoutHeader("Controller Properties")]
        [Tooltip("How much the vehicle's g-forces are reflected on the driver's seat.\n\nIncreasing this on large trucks helps the driving feel more authentic (~3.0), lowering on fast race cars helps with maintaining grip with the steering wheel when strapped down to a bucket seat.")]
        public Vector3 SeatGForceMultiplier = Vector3.one;

        [Tooltip("How fast the g-forces change on the driver's seat.\n\nLower this on heavier vehicles for more bounce (~2.5)")]
        public Vector3 SeatGForceSpeed = new Vector3(5f, 5f, 5f);

        [Range(0.1f, 5f)]
        [Tooltip("Arbitrary value for how strong the anti-rollbar should be")]
        public float AntiRollbarMultiplier = 0.75f;

        [Range(1f, 10f)]
        [Tooltip("Arbitrary value for how fast the anti-rollbar reacts to body roll")]
        public float AntiRollbarSpeed = 1f;

        [Tooltip("Use the Ackermann steering geometry. Helps eliminate tire slip at slow speeds.")]
        public bool UseAckermannSteering = true;

        [Range(-90f, 90f)]
        [Tooltip("Maximum steering angle of the furthest axles.\n\nNOTE: Ackermann steering geometry will cause the inner wheel to steer further than value set here.")]
        public float MaxSteeringAngle = 35f;

        [Tooltip("How fast the steering wheel will rotate freely by default in VR when not being held.\n\nDefault value good for passenger cars, SUVs (~0.2), lower for heavy vehicles (~0.05)")]
        public float SteeringVRBaseDeltaSpeed = 0.25f;

        [Tooltip("Arbitrary limit for how fast the steering wheel can rotate freely in VR when not being held.\n\nDefault value good for passenger cars, SUVs (~0.4), lower for heavy vehicles (~0.2)")]
        public float SteeringVRMaxDeltaSpeed = 0.5f;

        [Min(10f)]
        [Tooltip("What is the maximum speed in KPH that the model's speedometer is capable of showing")]
        public float SpeedometerRange = 200f;

        /// <summary>
        /// How much the engine will resist turning by default.
        /// <para>Example values:</para>
        /// <para>200kg go-kart: 10</para>
        /// <para>2000kg passenger car: 20</para>
        /// <para>20000kg truck: 800</para>
        /// </summary>
        [Tooltip("How much the engine will resist turning by default. Hint: 200kg go-kart: 10, passenger car: 20, 20-ton truck: 800")]
        public float EngineBaseFriction = 20f;

        public bool AllWheelDrive;

        /// <summary>
        /// How much more the engine will resist turning with increased RPM squared.
        /// <para>Example values:</para>
        /// <para>200kg go-kart: 0.01</para>
        /// <para>2000kg passenger car: 0.02</para>
        /// <para>20000kg truck: 0.1</para>
        /// </summary>
        [Tooltip("How much more the engine will resist turning with increased RPM squared.\n\nDefault value good for most passenger cars, increase to ~0.05 for heavy trucks")]
        public float DrivetrainRPMFrictionCoefficient = 0.02f;

        /// <summary>
        /// Moment of inertia of the drivetrain (I=½MR²)
        /// <para>Example values:</para>
        /// <para>200kg go-kart: 0.025</para>
        /// <para>2000kg passenger car: 0.1</para>
        /// <para>20000kg truck: 1.5</para>
        /// </summary>
        [Tooltip("Moment of inertia of the drivetrain (I=½MR²)\n\n0.1 is good for most passenger cars, must be increased to 1 - 1.5 for heavy trucks in order to drive up hills")]
        public float DrivetrainInertia = 0.1f;

        [Tooltip("Legacy override for VUdon Vehicles - Lite CarController, since it doesn't support true manual transmission yet.\n\nIf the vehicle has manual transmission, enable this.")]
        public bool EmulatedManualShifting;

        [FoldoutHeader("Audio")]
        [Tooltip("RES2-style engine audio preset to be used")]
        public EngineAudioPreset EngineAudioPreset;

        [Tooltip("Use the 'Engine Aggression' channel of the preset")]
        public bool EngineAggression;

        [Range(0.1f, 1f)]
        [Tooltip("Multiplier for how far the high end the RPM range will go.\n\nAllows for the engine RPM range to be cut for vehicles with lower speed engines")]
        public float EngineAudioRpmRange = 1f;

        [Tooltip("Position where the engine audio will be coming from.")]
        public Vector3 EnginePosition = new Vector3(0f, 0.75f, 1.5f);

        [Tooltip("Position where the exhaust audio will be coming from.\n\nNOTE: Not used by VUdon Vehicles - Lite's CarController due to limitations")]
        public Vector3 ExhaustPosition = new Vector3(0f, 0.25f, -2f);

        [Tooltip("Position where the gear shifter SFX will be played from.")]
        public Vector3 GearShifterPosition = new Vector3(0f, 0.5f, 0.5f);

        [Tooltip("Array of AudioClips to be randomly played when a gear is shifted")]
        public AudioClip[] GearShiftSFXClips;

        [Tooltip("AudioClip to be played when the handbrake is pulled")]
        public AudioClip HandbrakeSFX;

        [FoldoutHeader(null)] // Serialized arrays are already under foldout headers which cannot be nested
        public SeatRowData[] SeatRows = new SeatRowData[] { new SeatRowData() };

        [FoldoutHeader("Third Person View (Desktop Only)")]
        [Tooltip("Override the automatically generated third person view on vehicles where it fails to deliver an acceptable experience")]
        public bool OverrideThirdPersonView;

        [ContextMenuItem("Preview", nameof(PreviewOrbitTop))]
        [CinemachineFreeLookOrbitInlinePropertyDrawer("Top")]
        public CinemachineFreeLook.Orbit OrbitTop = new CinemachineFreeLook.Orbit(4f, 3f);

        [ContextMenuItem("Preview", nameof(PreviewOrbitMiddle))]
        [CinemachineFreeLookOrbitInlinePropertyDrawer("Middle")]
        public CinemachineFreeLook.Orbit OrbitMiddle = new CinemachineFreeLook.Orbit(2f, 4f);

        [ContextMenuItem("Preview", nameof(PreviewOrbitBottom))]
        [CinemachineFreeLookOrbitInlinePropertyDrawer("Bottom")]
        public CinemachineFreeLook.Orbit OrbitBottom = new CinemachineFreeLook.Orbit(0.1f, 3f);

        private void PreviewOrbitTop() { PreviewOrbit(OrbitTop); }
        private void PreviewOrbitMiddle() { PreviewOrbit(OrbitMiddle); }
        private void PreviewOrbitBottom() { PreviewOrbit(OrbitBottom); }

        private void PreviewOrbit(CinemachineFreeLook.Orbit orbit)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            UnityEditor.SceneView sv = UnityEditor.SceneView.lastActiveSceneView;

            Vector3 offset = sv.pivot - sv.camera.transform.position;
            float distance = offset.magnitude;
            Vector3 lookPositionWorld = transform.TransformPoint(LookPosition);
            Vector3 cameraPos = transform.TransformPoint(0f, orbit.m_Height, -orbit.m_Radius);
            Quaternion cameraRot = Quaternion.LookRotation(lookPositionWorld - cameraPos);
            float delta = sv.cameraDistance - Vector3.Distance(lookPositionWorld, cameraPos);
            sv.LookAt(lookPositionWorld + cameraRot * Vector3.forward * delta, cameraRot);
#endif
        }

        [Tooltip("Point which the third person camera will look at")]
        public Vector3 LookPosition = new Vector3(0f, 1.25f, 0f);

        [NonSerialized]
        internal Mesh avatarMesh;

#if UNITY_EDITOR && !COMPILER_UDONSHARP
        private void OnDrawGizmosSelected()
        {
            using (new GizmosDrawingScope(Color.cyan, transform.localToWorldMatrix))
            {
                Gizmos.DrawWireSphere(EnginePosition, 0.05f);

                Gizmos.DrawWireSphere(ExhaustPosition, 0.05f);

                Gizmos.DrawWireSphere(GearShifterPosition, 0.05f);

                Gizmos.DrawWireSphere(CenterOfMass, 0.05f);

                Gizmos.color = new Color(0.333f, 0.666f, 1f, 0.5f);

                foreach (SeatRowData rowData in SeatRows)
                {
                    Gizmos.DrawWireSphere(rowData.HandlePosition, 0.05f);
                    Gizmos.DrawWireSphere(Vector3.Scale(rowData.HandlePosition, new Vector3(-1f, 1f, 1f)), 0.05f);
                }

                if (avatarMesh)
                {
                    Gizmos.color = new Color(0.333f, 0.666f, 1f, 0.5f);

                    foreach (SeatRowData rowData in SeatRows)
                    {
                        Gizmos.matrix = Matrix4x4.TRS(transform.TransformPoint(rowData.Position), transform.rotation, Vector3.one);

                        float position = rowData.Position.x;

                        float fraction = (rowData.SeatCount == 1) ? 0f : position * 2f / ((float)rowData.SeatCount - 1f);

                        for (int i = 0; i < rowData.SeatCount; i++)
                        {
                            Gizmos.DrawMesh(avatarMesh, 0, fraction * i * Vector3.left, rowData.GetRotation(i));
                        }
                    }
                }

                if (OverrideThirdPersonView)
                {
                    Gizmos.matrix = transform.localToWorldMatrix;

                    Gizmos.color = Color.green;

                    Gizmos.DrawWireSphere(LookPosition, 0.05f);

                    Vector3 lookPositionWorld = transform.TransformPoint(LookPosition);

                    DrawThirdPersonViewFrustum(lookPositionWorld, OrbitTop);
                    DrawThirdPersonViewFrustum(lookPositionWorld, OrbitMiddle);
                    DrawThirdPersonViewFrustum(lookPositionWorld, OrbitBottom);
                }
            }
        }

        private void DrawThirdPersonViewFrustum(Vector3 lookPositionWorld, CinemachineFreeLook.Orbit orbit)
        {
            Vector3 camPos = transform.TransformPoint(new Vector3(0f, orbit.m_Height, -orbit.m_Radius));
            Gizmos.matrix = Matrix4x4.TRS(camPos, Quaternion.LookRotation(transform.TransformPoint(LookPosition) - camPos), Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, 60f, 1f, 0f, 16f / 9f);
        }
#endif
    }
}

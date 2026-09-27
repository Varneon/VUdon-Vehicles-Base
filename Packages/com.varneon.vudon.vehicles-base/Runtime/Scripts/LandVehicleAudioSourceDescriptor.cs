using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining an AudioSource on a vehicle for specific output
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Audio Source")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class LandVehicleAudioSourceDescriptor : LandVehicleObjectDescriptor
    {
        public enum AudioSourceType
        {
            [Tooltip("Looping engine audio based on RES2 format")]
            Engine,
            [Tooltip("Engine start and stop")]
            EngineSFX,
            [Tooltip("Misc. SFX that will be played inside the interior of the vehicle")]
            InteriorSFX,
            [Tooltip("Tire humming on asphalt from rolling, rocks banging on the body on dirt roads, water sloshing at shallow bodies of water, etc.")]
            RoadNoise,
            [Tooltip("Tire slipping noise")]
            SkidNoise,
            [Tooltip("Other miscellaneous sound effects")]
            Other
        }

        public AudioSourceType Type => type;

        [SerializeField]
        private AudioSourceType type;

        public bool DisableSpatialForInterior => disableSpatialForInterior;

        [SerializeField]
        [Tooltip("Audio spatialization doesn't like fast moving sources and listeners, recommended to disable spatialization when seated in the vehicle for comfort")]
        private bool disableSpatialForInterior;

        public bool LowPassEnabled => lowpassEnabled;

        [SerializeField]
        [Tooltip("Use low-pass filter to muffle audio originating from foreign sources of the vehicle when seated")]
        private bool lowpassEnabled;

        public float LowPassFrequency => lowpassFrequency;

        [SerializeField, Range(0f, 22000f)]
        [Tooltip("Frequency for muffling exterior audio sources from foreign sources")]
        private float lowpassFrequency = 22000f;

        public float InteriorDryLevel => interiorDryLevel;

        [SerializeField, Range(-10000f, 0f)]
        [Tooltip("Reverb dry level for muffling vehicle's own exterior sounds when seated.\n\nLower for better soundproofed luxury vehicles, increase for vehicles without enclosing interior")]
        private float interiorDryLevel = -1000f;

        public AudioSource[] Sources => GetComponents<AudioSource>();
    }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
    [UnityEditor.CanEditMultipleObjects]
    [UnityEditor.CustomEditor(typeof(LandVehicleAudioSourceDescriptor))]
    public class LandVehicleAudioSourceDescriptorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UnityEditor.EditorGUILayout.HelpBox("This descriptor is used to define an audio source for a land vehicle.", UnityEditor.MessageType.Info);
        }
    }
#endif
}

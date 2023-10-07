using UnityEngine;
using Varneon.VUdon.VehiclesBase.Interfaces;

namespace Varneon.VUdon.VehiclesBase
{
    [RequireComponent(typeof(AudioSource))]
    [DisallowMultipleComponent]
    [AddComponentMenu("VUdon/Vehicles/Descriptors/Land Vehicle Audio Source")]
    public class LandVehicleAudioSourceDescriptor : MonoBehaviour, IDestroyOnBuild
    {
        public enum AudioSourceType
        {
            Engine,
            EngineSFX,
            InteriorSFX,
            RoadNoise,
            SkidNoise,
            Other
        }

        public AudioSourceType Type => type;

        [SerializeField]
        private AudioSourceType type;

        [SerializeField]
        private bool lowpassEnabled;

        [SerializeField, Range(0f, 22000f)]
        private float lowpassFrequency = 22000f;
    }

#if UNITY_EDITOR && !COMPILER_UDONSHARP
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

using Newtonsoft.Json;
using System;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.DataPresets
{
    [CreateAssetMenu(menuName = "VUdon - Vehicles/Data Presets/Engine Audio Preset", fileName = "NewEngineAudioPreset.asset", order = 100)]
    public class EngineAudioPreset : ScriptableObject
    {
        public string Name;

        public string Description;

        public AudioClip IdleClip;

        public AnimationCurve IdleVolumeCurve;

        public AnimationCurve IdlePitchCurve;

        public AudioClip[] OnLoadClips;

        public AudioClip[] OffLoadClips;

        public AnimationCurve[] VolumeCurves;

        public AnimationCurve[] PitchCurves;

        public AudioClip AggrOnClip;

        public AudioClip AggrOffClip;

        public AnimationCurve AggrVolumeCurve;

        public AnimationCurve AggrPitchCurve;

        public AudioClip RedlineClip;

        public AnimationCurve RedlineVolumeCurve;

        public AnimationCurve OnLoadRPMVolume;

        public AnimationCurve OffLoadRPMVolume;

        public AudioClip StartClip;

        public AudioClip StopClip;
    }
}

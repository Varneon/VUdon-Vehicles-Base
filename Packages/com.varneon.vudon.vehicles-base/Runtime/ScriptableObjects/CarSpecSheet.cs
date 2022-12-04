using UnityEngine;
using Newtonsoft.Json;

namespace Varneon.VUdon.VehiclesBase.DataPresets
{
    [CreateAssetMenu(menuName = "VUdon - Vehicles/Data Presets/Car Spec Sheet", fileName = "NewCarSpecSheet.asset", order = 100)]
    public class CarSpecSheet : ScriptableObject
    {
        public CarSpecSheetData Data
        {
            get
            {
                return string.IsNullOrEmpty(rawJsonData) ? new CarSpecSheetData() : JsonConvert.DeserializeObject<CarSpecSheetData>(rawJsonData);
            }
            set
            {
                rawJsonData = JsonConvert.SerializeObject(value, Formatting.Indented);
            }
        }

        public string RawJsonData => rawJsonData;

        [SerializeField]
        internal string rawJsonData;

        public class CarSpecSheetData
        {
            /// <summary>
            /// Name of the car preset
            /// </summary>
            public string Name = string.Empty;

            /// <summary>
            /// Description of the car
            /// </summary>
            public string Description = string.Empty;

            /// <summary>
            /// Make of the car
            /// </summary>
            public string Make = string.Empty;

            /// <summary>
            /// Car model
            /// </summary>
            public string Model = string.Empty;

            /// <summary>
            /// Production year of the car
            /// </summary>
            public int Year = 2022;

            /// <summary>
            /// Default operational weight of the car
            /// </summary>
            public float Weight = 1750f;

            /// <summary>
            /// Top speed of the car
            /// </summary>
            public float TopSpeed = 200f;

            /// <summary>
            /// Maximum torque of the engine in Nm
            /// </summary>
            public float MaxEngineTorque = 400f;

            /// <summary>
            /// AnimationCurve keyframes for describing the engine's torque curve
            /// </summary>
            public Keyframe[] EngineTorqueCurveKeyframes;

            /// <summary>
            /// Number of forward gears
            /// </summary>
            public int GearCount = 6;

            /// <summary>
            /// Ratios of each forward gear
            /// </summary>
            public float[] GearRatios = new float[] { 5.5f, 3.5f, 2f, 1.35f, 1f, 0.8f };

            /// <summary>
            /// Reverse gear's ratio
            /// </summary>
            public float ReverseGearRatio = 5f;

            /// <summary>
            /// Final drive's ratio
            /// </summary>
            public float FinalDriveRatio = 4f;
        }
    }
}

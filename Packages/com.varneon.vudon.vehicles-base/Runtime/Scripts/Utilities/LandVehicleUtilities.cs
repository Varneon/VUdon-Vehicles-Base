using System.Linq;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Common land vehicle utility methods
    /// </summary>
    public static class LandVehicleUtilities
    {
        /// <summary>
        /// Gets the average radius across all wheels
        /// </summary>
        /// <remarks>
        /// Uses <see cref="System.Linq"/> for C#, manual iteration through the array on U#
        /// </remarks>
        /// <param name="wheelColliders"></param>
        /// <returns>Average wheel radius in metres</returns>
        public static float GetWheelColliderAverageRadius(ref WheelCollider[] wheelColliders)
        {
#if UNITY_EDITOR && !COMPILER_UDONSHARP
            return wheelColliders.Average(w => w.radius);
#else
            float radius = 0f;

            for (int i = 0; i < wheelColliders.Length; i++)
            {
                radius = wheelColliders[i].radius;
            }

            return radius / wheelColliders.Length;
#endif
        }

        /// <summary>
        /// Calculates the speeds at which (km/h) the vehicle should shift gear up or down
        /// </summary>
        /// <param name="wheelRadius">Radius of the driven wheel in metres</param>
        /// <param name="gearRatios">Ratio of each forward gear in the transmission</param>
        /// <param name="finalDrive">Final drive to multiply the gear ratios with (differential + transfer case?)</param>
        /// <param name="upShiftOnThrottleRPM">Desired RPM at which to shift gear up at full throttle</param>
        /// <param name="downShiftOffThrottleRPM">Desired RPM at which to shift gear down without throttle</param>
        /// <param name="upShiftSpeeds">Array to populate with speeds at which to shift gear up</param>
        /// <param name="downShiftSpeeds">Array to populate with speeds at which to shift gear down</param>
        public static void CalculateShiftSpeeds(float wheelRadius, float[] gearRatios, float finalDrive, float upShiftOnThrottleRPM, float downShiftOffThrottleRPM, ref float[] upShiftSpeeds, ref float[] downShiftSpeeds)
        {
            float wheelCircumference = wheelRadius * Mathf.PI * 2f;

            gearRatios = gearRatios.Select(r => r * finalDrive).ToArray();

            upShiftSpeeds = gearRatios.Select(r => upShiftOnThrottleRPM / r / 60f * 3.6f * wheelCircumference).ToArray();
            downShiftSpeeds = gearRatios.Select(r => downShiftOffThrottleRPM / r / 60f * 3.6f * wheelCircumference).ToArray();
        }
    }
}

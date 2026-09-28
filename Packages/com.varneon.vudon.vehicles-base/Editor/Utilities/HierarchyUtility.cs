using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor.Utilities
{
    public static class HierarchyUtility
    {
        private const string WHEEL_REGEX = @"wheels?_?((f(ront)?)|(m(iddle)?)|(r(ear)?))_?\d?((l(eft)?)|(r(ight)?))";

        private const string WHEEL_HUB_REGEX = "((wheel_?hub)|(brake_?(cali[pb]er)?))_?((f(ront)?)|(r(ear)?))_?d?((l(eft)?)|(r(ight)?))";

        private const string STEERING_WHEEL_REGEX = "steering_?wheel";

        public static bool TryGetWheelTransforms(Transform root, out Transform[] wheels)
        {
            Regex regex = new Regex(WHEEL_REGEX, RegexOptions.IgnoreCase);

            List<Transform> wheelsList = new List<Transform>();

            foreach(Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                if (!regex.IsMatch(t.name)) { continue; }

                if (t.GetComponentsInParent<Transform>().Any(p => wheelsList.Contains(p))) { continue; }

                wheelsList.Add(t);
            }

            wheels = wheelsList.ToArray();

            int wheelCount = wheels.Length;

            return wheelCount >= 4 && wheelCount % 2 == 0;
        }

        public static bool TryGetWheelHubTransforms(Transform root, out Transform[] wheelHubs)
        {
            Regex regex = new Regex(WHEEL_HUB_REGEX, RegexOptions.IgnoreCase);

            List<Transform> wheelsList = new List<Transform>();

            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                if (!regex.IsMatch(t.name)) { continue; }

                if (t.GetComponentsInParent<Transform>().Any(p => wheelsList.Contains(p))) { continue; }

                wheelsList.Add(t);
            }

            wheelHubs = wheelsList.ToArray();

            int wheelCount = wheelHubs.Length;

            return wheelCount >= 4 && wheelCount % 2 == 0;
        }

        public static bool TryGetSteeringWheelTransform(Transform root, out Transform steeringWheel)
        {
            Regex regex = new Regex(STEERING_WHEEL_REGEX, RegexOptions.IgnoreCase);

            steeringWheel = null;

            foreach (Transform t in root.GetComponentsInChildren<Transform>(true))
            {
                if (!regex.IsMatch(t.name)) { continue; }

                steeringWheel = t;

                break;
            }

            return steeringWheel != null;
        }
    }
}

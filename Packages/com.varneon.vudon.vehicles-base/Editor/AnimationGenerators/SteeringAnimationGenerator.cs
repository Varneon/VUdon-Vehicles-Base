using System;
using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    public class SteeringAnimationGenerator : EditorWindow
    {
        private Transform vehicleRoot;

        private Transform steeringWheelMesh;

        private Transform[] wheelHubs;

        private float steeringWheelRange = 1080f;

        [MenuItem("Varneon/VUdon - Vehicles/Asset Generators/Animations/SteeringWheel Animation Generator")]
        private static void OpenWindow()
        {
            GetWindow<SteeringAnimationGenerator>("SteeringAnimationGenerator");
        }

        private void OnGUI()
        {
            vehicleRoot = (Transform)EditorGUILayout.ObjectField("Vehicle Root", vehicleRoot, typeof(Transform), true);

            steeringWheelMesh = (Transform)EditorGUILayout.ObjectField("Steering Wheel Mesh", steeringWheelMesh, typeof(Transform), true);

            steeringWheelRange = EditorGUILayout.FloatField("Steering Wheel Range", steeringWheelRange);

            using (new EditorGUI.DisabledScope(vehicleRoot == null || steeringWheelMesh == null))
            {
                if (GUILayout.Button("Generate Animation")) { GenerateAnimation(vehicleRoot, steeringWheelMesh, steeringWheelRange); }
            }
        }

        public static void GenerateAnimation(Transform vehicleRoot, Transform steeringWheelMesh, float steeringWheelRange, string path = null)
        {
            string hierarchyPath = AnimationUtility.CalculateTransformPath(steeringWheelMesh, vehicleRoot);

            GenerateAnimation(hierarchyPath, steeringWheelRange, path);
        }

        public static void GenerateAnimation(string hierarchyPath, float steeringWheelRange, string path = null, AnimationClip originalClip = null, params string[] hubHierarchyPaths)
        {
            AnimationCurve curve = AnimationCurve.Linear(0f, steeringWheelRange / 2f, 1f, -steeringWheelRange / 2f);

            AnimationClip animation = originalClip ?? new AnimationClip() { name = "Steering" };

            animation.wrapMode = WrapMode.ClampForever;

            animation.SetCurve(hierarchyPath, typeof(Transform), "localEulerAnglesRaw.z", curve);

            AnimationCurve hubCurve = AnimationCurve.Linear(0f, -45f, 1f, 45f);

            foreach(string hubPath in hubHierarchyPaths)
            {
                animation.SetCurve(hubPath, typeof(Transform), "localEulerAnglesRaw.y", hubCurve);
            }

            try
            {
                if (path == null) { path = EditorUtility.SaveFilePanel("Save animation", "Assets", "Steering", "anim"); }

                if (string.IsNullOrEmpty(path))
                {
                    Debug.LogError($"[<color=purple>{nameof(SteeringAnimationGenerator)}</color>]: Invalid path!");

                    return;
                }

                string projectPath = Application.dataPath;

                projectPath = projectPath.Substring(0, projectPath.Length - 6);

                path = path.Replace(projectPath, string.Empty);

                VehicleAnimationAssetGenerator.GenerateAnimationAsset(animation, path);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}

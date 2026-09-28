using System;
using UnityEditor;
using UnityEngine;
using Varneon.VUdon.VehiclesBase.Editor.Utilities;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    public class WheelRollingAnimationGenerator : EditorWindow
    {
        private Transform vehicleRoot;

        [SerializeField]
        private Transform[] wheels = new Transform[0];

        private float wheelRadius = 0.35f;

        private SerializedObject serializedObject;

        private SerializedProperty wheelsProperty;

        [MenuItem("Varneon/VUdon - Vehicles/Asset Generators/Animations/Wheel Rolling Animation Generator")]
        private static void OpenWindow()
        {
            GetWindow<WheelRollingAnimationGenerator>("WheelRollingAnimationGenerator");
        }

        private void Awake()
        {
            wheels = new Transform[0];
        }

        private void OnEnable()
        {
            serializedObject = new SerializedObject(this);

            wheelsProperty = serializedObject.FindProperty("wheels");

            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            vehicleRoot = (Transform)EditorGUILayout.ObjectField("Vehicle Root", vehicleRoot, typeof(Transform), true);

            serializedObject.Update();

            EditorGUILayout.PropertyField(wheelsProperty, true);

            serializedObject.ApplyModifiedProperties();

            wheelRadius = EditorGUILayout.FloatField("Wheel Radius", wheelRadius);

            using (new EditorGUI.DisabledScope(vehicleRoot == null || wheels.Length == 0))
            {
                if(GUILayout.Button("Generate Animation")) { GenerateAnimation(); }
            }
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if(vehicleRoot == null) { return; }

            foreach(Transform wheel in wheels)
            {
                WheelGUIUtility.DrawWheelMeshRollingPreviewHandles(wheel, wheelRadius, vehicleRoot);
            }
        }

        private void GenerateAnimation()
        {
            float metersPerWheelRevolution = Mathf.PI * 2f * wheelRadius;

            float animationLength = 1f / (191f / 3.6f / metersPerWheelRevolution);

            AnimationCurve curve = AnimationCurve.Linear(0f, 0f, animationLength, 360f);

            AnimationClip animation = new AnimationClip();

            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(animation);

            settings.loopTime = true;

            AnimationUtility.SetAnimationClipSettings(animation, settings);

            foreach(Transform wheel in wheels)
            {
                animation.SetCurve(AnimationUtility.CalculateTransformPath(wheel, vehicleRoot), typeof(Transform), "localEulerAnglesRaw.x", curve);
            }

            try
            {
                string path = EditorUtility.SaveFilePanel("Save animation", "Assets", "WheelRoll", "anim");

                if (string.IsNullOrEmpty(path))
                {
                    Debug.LogError($"[<color=purple>{nameof(WheelRollingAnimationGenerator)}</color>]: Invalid path!");

                    return;
                }

                string projectPath = Application.dataPath;

                projectPath = projectPath.Substring(0, projectPath.Length - 6);

                path = path.Replace(projectPath, string.Empty);

                VehicleAnimationAssetGenerator.GenerateAnimationAsset(animation, path);
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void GenerateAnimation(Transform root, Transform[] wheels, float wheelRadius, string path = null, AnimationClip originalClip = null)
        {
            float metersPerWheelRevolution = Mathf.PI * 2f * wheelRadius;

            float animationLength = 1f / (191f / 3.6f / metersPerWheelRevolution);

            AnimationCurve curve = AnimationCurve.Linear(0f, 0f, animationLength, 360f);

            AnimationClip animation = originalClip ?? new AnimationClip() { name = "Steering" };

            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(animation);

            settings.loopTime = true;

            AnimationUtility.SetAnimationClipSettings(animation, settings);

            foreach (Transform wheel in wheels)
            {
                animation.SetCurve(AnimationUtility.CalculateTransformPath(wheel, root), typeof(Transform), "localEulerAnglesRaw.x", curve);
            }

            try
            {
                if (path == null) { path = EditorUtility.SaveFilePanel("Save animation", "Assets", "WheelRoll", "anim"); }

                if (string.IsNullOrEmpty(path))
                {
                    Debug.LogError($"[<color=purple>{nameof(WheelRollingAnimationGenerator)}</color>]: Invalid path!");

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

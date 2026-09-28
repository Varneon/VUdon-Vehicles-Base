using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Varneon.VUdon.VehiclesBase.Editor.Utilities;

namespace Varneon.VUdon.VehiclesBase.Editor.AssetGeneration
{
    public static class LandVehicleAnimatorConstructors
    {
        private static (LandVehicleAnimationType, AnimatorControllerParameterType)[] ParametersLite() => new (LandVehicleAnimationType, AnimatorControllerParameterType)[]
        {
            (LandVehicleAnimationType.AbsoluteSpeed, AnimatorControllerParameterType.Float),
            (LandVehicleAnimationType.Brake, AnimatorControllerParameterType.Float),
            (LandVehicleAnimationType.InReverse, AnimatorControllerParameterType.Bool),
            (LandVehicleAnimationType.RemoteSignedSpeed, AnimatorControllerParameterType.Float),
            (LandVehicleAnimationType.RPM, AnimatorControllerParameterType.Float),
            (LandVehicleAnimationType.Steering, AnimatorControllerParameterType.Float),
            (LandVehicleAnimationType.Throttle, AnimatorControllerParameterType.Float)
        };

        private static (LandVehicleAnimationType, AnimatorState)[] LayersLite() => new (LandVehicleAnimationType, AnimatorState)[]
        {
            (LandVehicleAnimationType.AbsoluteSpeed, new AnimatorState(){ name = "AbsoluteSpeed", speed = 0f, timeParameterActive = true, timeParameter = "AbsoluteSpeed" }),
            (LandVehicleAnimationType.Brake, new AnimatorState(){ name = "Brake", speed = 0f, timeParameterActive = true, timeParameter = "Brake" }),
            (LandVehicleAnimationType.InReverse, new AnimatorState(){ name = "InReverse", speed = 0f, mirrorParameterActive = true, mirrorParameter = "InReverse" }),
            (LandVehicleAnimationType.RemoteSignedSpeed, new AnimatorState(){ name = "RemoteSignedSpeed", speed = 1f, speedParameterActive = true, speedParameter = "RemoteSignedSpeed" }),
            (LandVehicleAnimationType.RPM, new AnimatorState(){ name = "RPM", speed = 0f, timeParameterActive = true, timeParameter = "RPM" }),
            (LandVehicleAnimationType.Steering, new AnimatorState(){ name = "Steering", speed = 0f, timeParameterActive = true, timeParameter = "Steering" }),
            (LandVehicleAnimationType.Throttle, new AnimatorState(){ name = "Throttle", speed = 0f, timeParameterActive = true, timeParameter = "Throttle" })
        };

        public static LandVehicleAnimatorManifest CreateLiteLandVehicleAnimator(string path, LandVehicleDescriptor descriptor)
        {
            string directory = PathUtility.ConvertToRelativePath(Path.GetDirectoryName(path));

            path = PathUtility.ConvertToRelativePath(path);

            string name = Path.GetFileNameWithoutExtension(path);

            LandVehicleAnimationDescriptor[] animationDescriptors = descriptor.GetComponentsInChildren<LandVehicleAnimationDescriptor>(true);

            AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);

            bool controllerExists = controller;

            if (!controllerExists) { controller = AnimatorController.CreateAnimatorControllerAtPath(path); }

            LandVehicleAnimatorManifest manifest = new LandVehicleAnimatorManifest(descriptor, directory, name, controller);

            controller.parameters = ParametersLite().Select(p => new AnimatorControllerParameter() { name = p.Item1.ToString(), type = p.Item2 }).ToArray();

            controller.parameters.FirstOrDefault(p => p.name.Equals("Steering")).defaultFloat = 0.5f;

            Type audioSourceType = typeof(AudioSource);

            foreach ((LandVehicleAnimationType, AnimatorState) layerInfo in LayersLite())
            {
                LandVehicleAnimationType animationType = layerInfo.Item1;

                string layerName = animationType.ToString();

                AnimationClip clip = LandVehicleAnimationConstructors.Animation(animationType);

                manifest.Animations[animationType] = clip;

                int existingLayerIndex = Array.FindIndex(controller.layers, l => l.name.Equals(layerName));
                if (existingLayerIndex >= 0) { continue; }

                AnimatorStateMachine stateMachine = new AnimatorStateMachine() { name = layerName, hideFlags = HideFlags.HideInHierarchy };

                AnimatorControllerLayer layer = new AnimatorControllerLayer() { name = layerName, defaultWeight = 1f, stateMachine = stateMachine };

                AssetDatabase.AddObjectToAsset(layer.stateMachine, path);

                AnimatorState state = layerInfo.Item2;

                state.motion = clip;

                AssetDatabase.AddObjectToAsset(state, path);

                if (animationDescriptors != null)
                {
                    foreach (LandVehicleAnimationDescriptor animationDescriptor in animationDescriptors.Where(d => d.AnimationType == animationType))
                    {
                        LandVehicleAnimationUtility.BindAnimationDescriptor(ref clip, animationDescriptor);
                    }
                }

                VehicleAnimationAssetGenerator.GenerateAnimationAsset(clip, PathUtility.ConvertToRelativePath(Path.Combine(Path.GetDirectoryName(path), $"{clip.name}.anim")), true);

                layer.stateMachine.AddState(state, new Vector3(425f, 120f, 0f));

                controller.AddLayer(layer);
            }

            #region Base Layer Cleanup
            AnimatorControllerLayer firstLayer = controller.layers[0];

            // If the controller's first layer has default name and is empty, remove it
            if (firstLayer.name.Equals("Base Layer") && firstLayer.stateMachine.states.Length == 0)
            {
                controller.RemoveLayer(0);
            }
            #endregion

            return manifest;
        }
    }
}

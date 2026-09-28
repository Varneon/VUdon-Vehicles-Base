using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    public static class VehicleAnimationAssetGenerator
    {
        public static bool TryGetAnimationClip(string path, out AnimationClip clip)
        {
            if (string.IsNullOrEmpty(path)) { throw new ArgumentException(nameof(path), "Invalid path"); }

            // Try to load existing animation clip from the path
            clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

            return clip;
        }

        public static void GenerateAnimationAsset(AnimationClip animation, string path, bool skipAssetPostProcessing = false)
        {
            if (string.IsNullOrEmpty(path)) { throw new ArgumentException(nameof(path), "Invalid path"); }

            // Try to load existing animation clip from the path
            AnimationClip existingClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

            string fileName = Path.GetFileNameWithoutExtension(path);

            if(animation.name != fileName)
            {
                Debug.LogWarningFormat("[VehicleAnimationAssetGenerator] Animation clip's name doesn't match the save path! Applying name to ensure functionality in Unity... <color=#888>({0})</color>", path);

                animation.name = fileName;
            }

            if (existingClip != null)
            {
                EditorUtility.CopySerialized(animation, existingClip);
            }
            else
            {
                AssetDatabase.CreateAsset(animation, path);
            }

            if (!skipAssetPostProcessing)
            {
                AssetDatabase.SaveAssets();

                AssetDatabase.Refresh();

                EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<AnimationClip>(path));
            }
        }

        public static void GenerateDefaultLandVehicleAnimationAsset(LandVehicleAnimationType animationType, string directory)
        {

        }
    }
}

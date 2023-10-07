using System;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor.Utilities
{
    /// <summary>
    /// Utility for Bounds
    /// </summary>
    public static class BoundsUtility
    {
        public static Bounds CalculateRendererBounds(Transform root, bool ignoreWithoutMaterials = false) => CalculateRendererBounds(root, Physics.AllLayers, ignoreWithoutMaterials);

        public static Bounds CalculateRendererBounds(Transform root, LayerMask layers, bool ignoreWithoutMaterials = false)
        {
            // Renderer.localBounds isn't available in 2019.4, this is why calculating the local bounds of the MeshRenderer requires the root to be moved to the scene's origin

            Vector3 originalPosition = root.position;
            Quaternion originalRotation = root.rotation;

            root.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            bool initialized = false;

            Bounds bounds = default;

            foreach (MeshRenderer renderer in root.GetComponentsInChildren<MeshRenderer>(true))
            {
                if(!IsLayerDefined(layers, renderer.gameObject.layer)) { continue; }

                if(ignoreWithoutMaterials && renderer.sharedMaterials.Length == 0) { continue; }

                if(initialized)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
                else
                {
                    bounds = new Bounds(renderer.bounds.center, renderer.bounds.size);

                    initialized = true;
                }
            }

            foreach(SkinnedMeshRenderer renderer in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                if (!IsLayerDefined(layers, renderer.gameObject.layer)) { continue; }

                if (ignoreWithoutMaterials && renderer.sharedMaterials.Length == 0) { continue; }

                if (initialized)
                {
                    bounds.Encapsulate(renderer.localBounds);
                }
                else
                {
                    bounds = new Bounds(renderer.localBounds.center, renderer.localBounds.size);

                    initialized = true;
                }
            }

            root.SetPositionAndRotation(originalPosition, originalRotation);

            return bounds;
        }

        public static Bounds CalculateHierarchyColliderBounds(Transform root, bool ignoreTriggers = true) => CalculateHierarchyColliderBounds(root, Physics.AllLayers, ignoreTriggers);

        public static Bounds CalculateHierarchyColliderBounds(Transform root, LayerMask layers, bool ignoreTriggers = true)
        {
            Bounds bounds = new Bounds();

            foreach (Collider collider in root.GetComponentsInChildren<Collider>())
            {
                if(!IsLayerDefined(layers, collider.gameObject.layer)) { continue; }

                if(ignoreTriggers && collider.isTrigger) { continue; }

                Type colliderType = collider.GetType();

                if (colliderType.Equals(typeof(BoxCollider)))
                {
                    BoxCollider boxCollider = (BoxCollider)collider;

                    bounds.Encapsulate(new Bounds(boxCollider.center, boxCollider.size));
                }
                else if (colliderType.Equals(typeof(CapsuleCollider)))
                {
                    CapsuleCollider capsuleCollider = (CapsuleCollider)collider;

                    float height = capsuleCollider.height;

                    float radius = capsuleCollider.radius;

                    Vector3 size = new Vector3(
                        capsuleCollider.direction == 0 ? height : radius,
                        capsuleCollider.direction == 1 ? height : radius,
                        capsuleCollider.direction == 2 ? height : radius
                        );

                    bounds.Encapsulate(new Bounds(capsuleCollider.center, size));
                }
                else if (colliderType.Equals(typeof(SphereCollider)))
                {
                    SphereCollider sphereCollider = (SphereCollider)collider;

                    float radius = sphereCollider.radius;

                    bounds.Encapsulate(new Bounds(sphereCollider.center, new Vector3(radius, radius, radius)));
                }
                else if (colliderType.Equals(typeof(MeshCollider)))
                {
                    MeshCollider meshCollider = (MeshCollider)collider;

                    bounds.Encapsulate(meshCollider.sharedMesh.bounds);
                }
            }
            return bounds;
        }

        private static bool IsLayerDefined(LayerMask mask, int layer) => (mask & (1 << layer)) != 0;
    }
}

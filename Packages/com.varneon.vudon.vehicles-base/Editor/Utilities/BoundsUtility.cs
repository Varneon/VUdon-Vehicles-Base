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
            bool initialized = false;

            Bounds bounds = default;

            Matrix4x4 worldToRootMatrix = root.worldToLocalMatrix;

            foreach (MeshRenderer renderer in root.GetComponentsInChildren<MeshRenderer>())
            {
                if(!IsLayerDefined(layers, renderer.gameObject.layer)) { continue; }

                if(ignoreWithoutMaterials && renderer.sharedMaterials.Length == 0) { continue; }

                Bounds localBounds = renderer.localBounds;

                Transform rendererTransform = renderer.transform;

                Matrix4x4 rendererToWorldMatrix = rendererTransform.localToWorldMatrix;

                Matrix4x4 matrix = Matrix4x4.TRS(rendererTransform.position - root.position, Quaternion.Inverse(root.rotation) * rendererTransform.rotation, rendererTransform.lossyScale);

                if(initialized)
                {
                    EncapsulatePoints(ref bounds, GetBoundsCorners(localBounds, rendererToWorldMatrix, worldToRootMatrix));
                }
                else
                {
                    bounds = GetPointsBounds(GetBoundsCorners(localBounds, rendererToWorldMatrix, worldToRootMatrix));

                    initialized = true;
                }
            }

            foreach(SkinnedMeshRenderer renderer in root.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (!IsLayerDefined(layers, renderer.gameObject.layer)) { continue; }

                if (ignoreWithoutMaterials && renderer.sharedMaterials.Length == 0) { continue; }

                Bounds localBounds = renderer.localBounds;

                if (initialized)
                {
                    bounds.Encapsulate(localBounds);
                }
                else
                {
                    bounds = new Bounds(localBounds.center, localBounds.size);

                    initialized = true;
                }
            }

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

        public static Bounds GetPointsBounds(params Vector3[] points)
        {
            Bounds bounds = new Bounds(points[0], Vector3.zero);

            EncapsulatePoints(ref bounds, points);

            return bounds;
        }

        public static void EncapsulatePoints(ref Bounds bounds, params Vector3[] points)
        {
            foreach(Vector3 point in points)
            {
                bounds.Encapsulate(point);
            }
        }

        public static Vector3[] GetBoundsCorners(Bounds bounds, Matrix4x4 matrix, Matrix4x4 matrix2)
        {
            Vector3 c = bounds.center;
            Vector3 e = bounds.extents;

            float x = e.x;
            float y = e.y;
            float z = e.z;
            float xn = -x;
            float yn = -y;
            float zn = -z;

            return new Vector3[]
            {
                matrix2.MultiplyPoint(matrix.MultiplyPoint(c + e)),
                matrix2.MultiplyPoint(matrix.MultiplyPoint(c + new Vector3(xn, y, z))),
                matrix2.MultiplyPoint(matrix.MultiplyPoint(c + new Vector3(xn, yn, z))),
                matrix2.MultiplyPoint(matrix.MultiplyPoint(c + new Vector3(xn, yn, zn))),
                matrix2.MultiplyPoint(matrix.MultiplyPoint(c + new Vector3(x, yn, zn))),
                matrix2.MultiplyPoint(matrix.MultiplyPoint(c + new Vector3(x, y, zn))),
                matrix2.MultiplyPoint(matrix.MultiplyPoint(c + new Vector3(xn, y, zn))),
                matrix2.MultiplyPoint(matrix.MultiplyPoint(c + new Vector3(x, yn, z)))
            };
        }

        public static Vector3[] GetBoundsCorners(Bounds bounds, Matrix4x4 matrix)
        {
            Vector3 c = bounds.center;
            Vector3 e = bounds.extents;

            float x = e.x;
            float y = e.y;
            float z = e.z;
            float xn = -x;
            float yn = -y;
            float zn = -z;

            return new Vector3[]
            {
                matrix.MultiplyPoint(c + e),
                matrix.MultiplyPoint(c + new Vector3(xn, y, z)),
                matrix.MultiplyPoint(c + new Vector3(xn, yn, z)),
                matrix.MultiplyPoint(c + new Vector3(xn, yn, zn)),
                matrix.MultiplyPoint(c + new Vector3(x, yn, zn)),
                matrix.MultiplyPoint(c + new Vector3(x, y, zn)),
                matrix.MultiplyPoint(c + new Vector3(xn, y, zn)),
                matrix.MultiplyPoint(c + new Vector3(x, yn, z))
            };
        }
    }
}

#if !COMPILER_UDONSHARP
using UnityEngine;
using Varneon.VUdon.VehiclesBase.Abstract;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Utility script for visualizing the orientation of generated <see cref="TrailRenderer"/>s used to draw skidmarks and treadmarks
    /// </summary>
    [AddComponentMenu("")]
    [DisallowMultipleComponent]
    [ExcludeFromPreset]
    [RequireComponent(typeof(TrailRenderer))]
    public sealed class LandVehicleSkidmarkVisualizer : LandVehicleObjectDescriptor
    {
        private TrailRenderer trailRenderer;

        private void OnValidate()
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }

        private void OnDrawGizmosSelected()
        {
            using (new VSDK.GizmosDrawingScope(Color.cyan, transform.localToWorldMatrix))
            {
                float width = trailRenderer.widthMultiplier * trailRenderer.startWidth;

                float extX = width / 2f;
                float extY = 0.5f;

                Gizmos.DrawLine(new Vector3(-extX, -extY, 0f), new Vector3(-extX, extY, 0f));
                Gizmos.DrawLine(new Vector3(extX, -extY, 0f), new Vector3(extX, extY, 0f));
            }
        }
    }
}
#endif

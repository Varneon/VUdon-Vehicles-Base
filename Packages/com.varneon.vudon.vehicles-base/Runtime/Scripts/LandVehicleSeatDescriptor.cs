using UnityEngine;
using Varneon.VSDK;
using Varneon.VUdon.VehiclesBase.Abstract;
using Varneon.VUdon.VehiclesBase.Enums;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining a seat on a vehicle, mostly a direct copy of <see cref="Seats.Abstract.Seat"/>
    /// </summary>
    /// <remarks>
    /// Due to the complexity of the calibration rig it is recommended to only use the automatic seat generation tools provided in <see cref="LandVehicleDescriptor"/>
    /// </remarks>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Seat Descriptor")]
    [DisallowMultipleComponent]
    public class LandVehicleSeatDescriptor : LandVehicleObjectDescriptor
    {
        public RuntimeAnimatorController AnimatorController;

        public SeatCalibrationMethod CalibrationMethod = SeatCalibrationMethod.Head;

        public Transform HeadCalibrationPoint;

        public Transform HipsCalibrationPoint;

        public Transform EnterTransform;

        public Transform ExitTransform;

#if !COMPILER_UDONSHARP
        private void OnDrawGizmosSelected()
        {
            if (HeadCalibrationPoint)
            {
                using (new GizmosDrawingScope(HeadCalibrationPoint.localToWorldMatrix))
                {
                    Gizmos.DrawFrustum(Vector3.zero, 60f, 1f, 0.05f, 16f / 9f);
                }
            }

            if (EnterTransform)
            {
                using (new GizmosDrawingScope(EnterTransform.localToWorldMatrix))
                {
                    GizmosUtilities.DrawAxisPreviewInline(0.2f, 0.5f, 1f);
                }
            }

            if (ExitTransform)
            {
                using (new GizmosDrawingScope(ExitTransform.localToWorldMatrix))
                {
                    GizmosUtilities.DrawAxisPreviewInline(0.2f, 0.5f, 1f);
                }
            }
        }
#endif
    }
}

using UdonSharp;
using Varneon.VUdon.VehiclesBase.Abstract;
using Varneon.VUdon.VisibilitySensors.Abstract;
using UnityEngine;
using VRC.Udon;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Visibility sensor for vehicles
    /// </summary>
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class VehicleVisibilitySensor : VisibilitySensor
    {
        /// <summary>
        /// Root vehicle which this sensor is attached to
        /// </summary>
        [SerializeField, HideInInspector]
        internal UdonBehaviour vehicle;

        public override void _onBecameVisible()
        {
            vehicle.SendCustomEvent(nameof(Vehicle.SetVehicleStateVisible));
        }

        public override void _onBecameInvisible()
        {
            vehicle.SendCustomEvent(nameof(Vehicle.SetVehicleStateInvisible));
        }
    }
}

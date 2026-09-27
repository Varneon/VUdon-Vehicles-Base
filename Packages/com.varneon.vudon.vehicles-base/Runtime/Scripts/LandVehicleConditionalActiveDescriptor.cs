using Varneon.VUdon.VehiclesBase.Abstract;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase
{
    /// <summary>
    /// Descriptor for defining under which conditions should the GameObject this component is attached to be active
    /// </summary>
    [AddComponentMenu(VehicleConstants.DESCRIPTOR_COMPONENT_ROOT_PATH + "Land Vehicle Conditional Active Descriptor")]
    [DisallowMultipleComponent]
    [ExcludeFromPreset]
    public class LandVehicleConditionalActiveDescriptor : LandVehicleObjectDescriptor
    {
        public enum FeatureType
        {
            [Tooltip("Enabled when the engine is running")]
            EngineRunning,
            [Tooltip("Enabled when the local player is the driver")]
            Driver,
            [Tooltip("Enabled for all players sitting in the vehicle")]
            Seated,
            [Tooltip("Enabled for the owner of the vehicle")]
            Owner,
            [Tooltip("Enabled when the vehicle's transmission is in reverse")]
            InReverse
        }

        [Tooltip("When should this GameObject be active")]
        public FeatureType Type;

        [Tooltip("Should this GameObject be active when the condition above is true")]
        public bool ActiveOnCondition = true;

        public void Set(FeatureType type, bool activeOnCondition)
        {
            Type = type;
            ActiveOnCondition = activeOnCondition;
        }

        /// <summary>
        /// Applies the initial active state by negating the desired active state on condition
        /// </summary>
        internal void Initialize()
        {
            gameObject.SetActive(!ActiveOnCondition);
        }
    }
}

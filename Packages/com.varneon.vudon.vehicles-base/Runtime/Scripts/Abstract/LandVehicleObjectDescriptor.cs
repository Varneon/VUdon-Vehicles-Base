using UnityEngine;
using Varneon.VSDK.Interfaces;

namespace Varneon.VUdon.VehiclesBase.Abstract
{
    /// <summary>
    /// Base class for all editor-only descriptors that define a certain object or feature on vehicles
    /// </summary>
    public abstract class LandVehicleObjectDescriptor : MonoBehaviour, IDestroyOnBuild { }
}

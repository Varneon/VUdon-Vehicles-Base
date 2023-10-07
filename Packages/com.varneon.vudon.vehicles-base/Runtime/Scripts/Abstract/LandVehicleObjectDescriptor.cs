using UnityEngine;
using Varneon.VUdon.VehiclesBase.Interfaces;

namespace Varneon.VUdon.VehiclesBase.Abstract
{
    [DisallowMultipleComponent]
    public abstract class LandVehicleObjectDescriptor : MonoBehaviour, IDestroyOnBuild { }
}

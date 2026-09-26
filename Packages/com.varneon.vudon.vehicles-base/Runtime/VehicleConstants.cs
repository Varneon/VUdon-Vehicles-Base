namespace Varneon.VUdon.VehiclesBase
{
    public static class VehicleConstants
    {
        /// <summary>
        /// Multiplier for converting m/s to km/h
        /// </summary>
        public const float METERS_PER_SECOND_TO_KPH = 3.6f;

        /// <summary>
        /// Multiplier for converting rad/s to rpm
        /// </summary>
        public const float RADIANS_PER_SECOND_TO_RPM = 9.5492965964254f;

        /// <summary>
        /// Multiplier for converting L/100km to mpg
        /// </summary>
        public const float LITRE_PER_100_KM_TO_MPG = 235.214583f;

        /// <summary>
        /// Multiplier for converting foot-pounds to Newton-meters
        /// </summary>
        public const float FT_LB_TO_NM = 1.3558179483f;

        /// <summary>
        /// Multiplier for converting miles per hour to kilometers per hour
        /// </summary>
        public const float MPH_TO_KPH = 1.60934f;

        public const float
            DEFAULT_MAX_AUDIBLE_VEHICLE_RANGE = 200f,
            DEFAULT_MAX_AUDIBLE_VEHICLE_RANGE_SQR = 40000f; // TODO: Parametrize

        public const float
            VELOCITY_STATIONARY_THRESHOLD = 0.25f,
            VELOCITY_STATIONARY_THRESHOLD_SQRT = 0.0625f,
            WHEELS_EFFECTIVE_CONTACT_FORCE_THRESHOLD = 0.25f,
            AUTO_RECOVERY_INTERVAL = 1f,
            REFLECTION_PROBE_RERENDER_DISTANCE_THRESHOLD_SQR = 0.25f,
            REFLECTION_PROBE_RERENDER_INTERVAL_THRESHOLD = 1f,
            AUTO_RESPAWN_DELAY_SECONDS = 300f, // For now the respawn has been set to 5 minutes, revisit later to consider adjusting
            AUTO_RESPAWN_DISTANCE_THRESHOLD_SQR = 10000f; // Respawn vehicles if they are beyond 100 meters from the player

        public const int
            VEHICLE_SUBSTEPS_DRIVER = 5, // Default: 10
            VEHICLE_SUBSTEPS_REMOTE = 1; // Default: 2

        /// <summary>
        /// Parameters names for vehicle animators
        /// </summary>
        public const string
            ANIM_PARAM_NAME_THROTTLE = "Throttle",
            ANIM_PARAM_NAME_BRAKE = "Brake",
            ANIM_PARAM_NAME_CLUTCH = "Clutch",
            ANIM_PARAM_NAME_STEERING = "Steering",
            ANIM_PARAM_NAME_INREVERSE = "InReverse",
            ANIM_PARAM_NAME_RPM = "RPM",
            ANIM_PARAM_NAME_ABSOLUTE_SPEED = "AbsoluteSpeed",
            ANIM_PARAM_NAME_SIGNED_SPEED = "SignedSpeed",
            ANIM_PARAM_NAME_HANDBRAKE = "Handbrake";

        public const string
            INPUT_AXIS_HORIZONTAL = "Horizontal",
            INPUT_AXIS_VERTICAL = "Vertical",
            INPUT_AXIS_OCULUS_GEARVR_LTHUMBSTICKX = "Oculus_GearVR_LThumbstickX",
            INPUT_AXIS_OCULUS_GEARVR_RTHUMBSTICKX = "Oculus_GearVR_RThumbstickX",
            INPUT_AXIS_OCULUS_GEARVR_RTHUMBSTICKY = "Oculus_GearVR_RThumbstickY",
            INPUT_AXIS_OCULUS_CROSSPLATFORM_SECONDARYINDEXTRIGGER = "Oculus_CrossPlatform_SecondaryIndexTrigger",
            INPUT_AXIS_OCULUS_CROSSPLATFORM_PRIMARYINDEXTRIGGER = "Oculus_CrossPlatform_PrimaryIndexTrigger";

        public const string
            DESCRIPTOR_COMPONENT_ROOT_PATH = "VUdon/Vehicles/Descriptors/";
    }
}

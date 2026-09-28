using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using Varneon.VUdon.VehiclesBase.Editor.Utilities;

namespace Varneon.VUdon.VehiclesBase.Editor
{
    /// <summary>
    /// Class for watching the addition of <see cref="LandVehicleDescriptor"/>, <see cref="LandVehicleWheelDescriptor"/> and <see cref="LandVehicleSteeringWheelDescriptor"/> in order to trigger automatic setup workflow
    /// </summary>
    [InitializeOnLoad]
    public static class ComponentAdditionWatcher
    {
        static ComponentAdditionWatcher()
        {
            ObjectFactory.componentWasAdded -= OnComponentAdded;
            ObjectFactory.componentWasAdded += OnComponentAdded;
        }

        private static void OnComponentAdded(Component component)
        {
            Type type = component.GetType();

            if (type == typeof(LandVehicleDescriptor))
            {
                Transform transform = component.transform;

                LandVehicleDescriptor descriptor = (LandVehicleDescriptor)component;

                descriptor.CenterOfMass = new Vector3(0f, 0.4f, 0f);

                Vector3 steeringWheelPosition = new Vector3(-0.45f, 0.85f, 0.5f);

                if (HierarchyUtility.TryGetWheelTransforms(transform, out Transform[] wheels))
                {
                    StringBuilder messageBuilder = new StringBuilder("Following wheels were found in the hierarchy:\n\n");

                    foreach (Transform wheel in wheels)
                    {
                        messageBuilder.AppendLine(AnimationUtility.CalculateTransformPath(wheel, transform));
                    }

                    messageBuilder.AppendLine("\nWould you like to assing wheel descriptors to the found wheels above?");

                    string message = messageBuilder.ToString();

                    if (EditorUtility.DisplayDialog("Vehicle Setup Wizard", message, "Yes", "No"))
                    {
                        foreach (Transform wheel in wheels)
                        {
                            Undo.AddComponent<LandVehicleWheelDescriptor>(wheel.gameObject);
                        }
                    }

                    if (HierarchyUtility.TryGetWheelHubTransforms(transform, out Transform[] wheelHubs) && wheels.Length == wheelHubs.Length)
                    {
                        messageBuilder = new StringBuilder("Following wheel hubs were found in the hierarchy:\n\n");

                        foreach (Transform wheelHub in wheelHubs)
                        {
                            messageBuilder.AppendLine(AnimationUtility.CalculateTransformPath(wheelHub, transform));
                        }

                        messageBuilder.AppendLine("\nWould you like to assing these wheel hubs to the descriptors?");

                        message = messageBuilder.ToString();

                        if (EditorUtility.DisplayDialog("Vehicle Setup Wizard", message, "Yes", "No"))
                        {
                            for(int i = 0; i < wheels.Length; i++)
                            {
                                wheels[i].GetComponent<LandVehicleWheelDescriptor>().WheelHub = wheelHubs[i];
                            }
                        }
                    }
                }

                if (HierarchyUtility.TryGetSteeringWheelTransform(transform, out Transform steeringWheel))
                {
                    StringBuilder messageBuilder = new StringBuilder("Steering wheel was found in the hierarchy:\n\n");

                    messageBuilder.AppendLine(AnimationUtility.CalculateTransformPath(steeringWheel, transform));

                    messageBuilder.AppendLine("\nWould you like to assing steering wheel descriptor to the found steering wheel above?");

                    string message = messageBuilder.ToString();

                    if (EditorUtility.DisplayDialog("Vehicle Setup Wizard", message, "Yes", "No"))
                    {
                        steeringWheelPosition = transform.InverseTransformPoint(steeringWheel.position);

                        Undo.AddComponent<LandVehicleSteeringWheelDescriptor>(steeringWheel.gameObject);
                    }
                }

                descriptor.SeatRows = new LandVehicleDescriptor.SeatRowData[]{ new LandVehicleDescriptor.SeatRowData(2, steeringWheelPosition - new Vector3(0f, 1f, 0.4f)) };
            }
            if(type == typeof(LandVehicleWheelDescriptor))
            {
                Transform transform = component.transform;

                Bounds bounds = BoundsUtility.CalculateRendererBounds(transform, true);

                LandVehicleWheelDescriptor descriptor = (LandVehicleWheelDescriptor)component;

                Vector3 size = bounds.size;

                descriptor.Radius = (size.y + size.z) / 4f;

                descriptor.Width = size.x;

                float horizontalOffset = bounds.center.x;

                descriptor.ColliderOffset.x = horizontalOffset;

                descriptor.HorizontalOffset = horizontalOffset;
            }
            if(type == typeof(LandVehicleSteeringWheelDescriptor))
            {
                Transform transform = component.transform;

                Bounds bounds = BoundsUtility.CalculateRendererBounds(transform, true);

                LandVehicleSteeringWheelDescriptor descriptor = (LandVehicleSteeringWheelDescriptor)component;

                Vector3 size = bounds.size;

                descriptor.Radius = Mathf.Max(size.x, size.y) / 2f - descriptor.Thickness;

                descriptor.HighlightRendererReference = descriptor.GetComponentInChildren<MeshRenderer>();
            }
        }
    }
}

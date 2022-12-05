using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.DataPresets.Editor
{
    [CustomEditor(typeof(CarSpecSheet))]
    public class CarSpecSheetEditor : UnityEditor.Editor
    {
        private CarSpecSheet specSheet;

        private CarSpecSheet.CarSpecSheetData specSheetData;

        private AnimationCurve engineTorqueCurve;

        private static readonly GUIContent
            NameFieldContent = new GUIContent("Name", "Name of the car preset"),
            DescriptionFieldContent = new GUIContent("Description", "Description of the car preset"),
            MakeFieldContent = new GUIContent("Make", "Make of the car"),
            ModelFieldContent = new GUIContent("Model", "Model of the car"),
            YearFieldContent = new GUIContent("Year", "Production year of the car"),
            WeightFieldContent = new GUIContent("Weight", "Default operational weight of the car"),
            TopSpeedFieldContent = new GUIContent("Top Speed", "Top speed of the car (km/h)"),
            IdleRPMFieldContent = new GUIContent("Idle RPM", "Idle RPM of the engine"),
            MaxRPMFieldContent = new GUIContent("Max RPM", "Max RPM of the engine"),
            MaxEngineTorqueFieldContent = new GUIContent("Max Engine Torque", "Maximum torque of the engine in Nm"),
            EngineTorqueCurveFieldContent = new GUIContent("Engine Torque Curve", "Normalized torque curve of the engine"),
            GearCountFieldContent = new GUIContent("Gear Count", "Number of forward gears"),
            ReverseGearRatioFieldContent = new GUIContent("Reverse Gear Ratio", "Reverse gear's ratio"),
            FinalDriveRatioFieldContent = new GUIContent("Final Drive Ratio", "Final drive's ratio");

        private bool fieldsDirty;

        private bool viewRawJson;

        private void OnEnable()
        {
            specSheet = (CarSpecSheet)target;

            specSheetData = specSheet.Data;

            if (specSheetData.GearRatios == null) { specSheetData.GearRatios = new float[specSheetData.GearCount]; }

            if(specSheetData.EngineTorqueCurveKeyframes == null || specSheetData.EngineTorqueCurveKeyframes.Length == 0)
            {
                engineTorqueCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
            }
            else
            {
                engineTorqueCurve = new AnimationCurve(specSheetData.EngineTorqueCurveKeyframes);
            }
        }

        public override void OnInspectorGUI()
        {
            using (var scope = new EditorGUI.ChangeCheckScope())
            {
                specSheetData.Name = EditorGUILayout.TextField(NameFieldContent, specSheetData.Name);

                specSheetData.Description = EditorGUILayout.TextField(DescriptionFieldContent, specSheetData.Description);

                specSheetData.Make = EditorGUILayout.TextField(MakeFieldContent, specSheetData.Make);

                specSheetData.Model = EditorGUILayout.TextField(ModelFieldContent, specSheetData.Model);

                specSheetData.Year = EditorGUILayout.IntField(YearFieldContent, specSheetData.Year);

                specSheetData.Weight = EditorGUILayout.FloatField(WeightFieldContent, specSheetData.Weight);

                specSheetData.TopSpeed = EditorGUILayout.FloatField(TopSpeedFieldContent, specSheetData.TopSpeed);

                specSheetData.IdleRPM = EditorGUILayout.FloatField(IdleRPMFieldContent, specSheetData.IdleRPM);

                specSheetData.MaxRPM = EditorGUILayout.FloatField(MaxRPMFieldContent, specSheetData.MaxRPM);

                specSheetData.MaxEngineTorque = EditorGUILayout.FloatField(MaxEngineTorqueFieldContent, specSheetData.MaxEngineTorque);

                using (var engineTorqueCurveChangedScope = new EditorGUI.ChangeCheckScope())
                {
                    engineTorqueCurve = EditorGUILayout.CurveField(EngineTorqueCurveFieldContent, engineTorqueCurve, GUILayout.Height(96));

                    if (engineTorqueCurveChangedScope.changed)
                    {
                        specSheetData.EngineTorqueCurveKeyframes = engineTorqueCurve.keys;
                    }
                }

                using (var gearCountChangedScope = new EditorGUI.ChangeCheckScope())
                {
                    specSheetData.GearCount = EditorGUILayout.IntField(GearCountFieldContent, specSheetData.GearCount);

                    if (gearCountChangedScope.changed)
                    {
                        System.Array.Resize(ref specSheetData.GearRatios, specSheetData.GearCount);
                    }
                }

                for(int i = 0; i < specSheetData.GearCount; i++)
                {
                    specSheetData.GearRatios[i] = EditorGUILayout.Slider($"Gear {i + 1} Ratio", specSheetData.GearRatios[i], 0.1f, 10f);
                }

                specSheetData.ReverseGearRatio = EditorGUILayout.FloatField(ReverseGearRatioFieldContent, specSheetData.ReverseGearRatio);

                specSheetData.FinalDriveRatio = EditorGUILayout.FloatField(FinalDriveRatioFieldContent, specSheetData.FinalDriveRatio);

                if (scope.changed)
                {
                    fieldsDirty = true;
                }
            }

            if(viewRawJson = EditorGUILayout.Foldout(viewRawJson, "Raw Json", true))
            {
                EditorGUILayout.TextArea(specSheet.RawJsonData);
            }

            using (new EditorGUI.DisabledScope(!fieldsDirty))
            {
                if(GUILayout.Button("Save Spec Sheet", GUILayout.Height(32f)))
                {
                    specSheet.Data = specSheetData;

                    AssetDatabase.SaveAssets();

                    fieldsDirty = false;
                }
            }

            using (new GUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(fieldsDirty))
                {
                    if (EditorGUILayout.DropdownButton(new GUIContent("Copy", "Copy the raw JSON data"), FocusType.Passive))
                    {
                        GenericMenu menu = new GenericMenu();

                        menu.AddItem(new GUIContent("Copy raw JSON", "Copies the raw JSON to your clipboard"), false, () => EditorGUIUtility.systemCopyBuffer = specSheet.RawJsonData);

                        menu.AddItem(new GUIContent("Copy raw JSON with code block formatting", "Copies the raw JSON to your clipboard with code block formatting (useful for displaying data in e.g. Discord, Markdown, etc.)"), false, () => EditorGUIUtility.systemCopyBuffer = string.Format("```json\n{0}\n```", specSheet.RawJsonData));

                        menu.ShowAsContext();
                    }
                }
            }
        }
    }
}

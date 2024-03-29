﻿using System.IO;
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
            TransmissionTypeFieldContent = new GUIContent("Transmission Type", "Type of the transmission"),
            GearCountFieldContent = new GUIContent("Gear Count", "Number of forward gears"),
            ReverseGearRatioFieldContent = new GUIContent("Reverse Gear Ratio", "Reverse gear's ratio"),
            FinalDriveRatioFieldContent = new GUIContent("Final Drive Ratio", "Final drive's ratio");

        private bool fieldsDirty;

        private bool viewRawJson;

        private void OnEnable()
        {
            specSheet = (CarSpecSheet)target;

            LoadSpecSheet();
        }

        private void OnDestroy()
        {
            if(specSheet == null) { return; }

            specSheet.Data = specSheetData;

            EditorUtility.SetDirty(specSheet);
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

                specSheetData.TransmissionType = (TransmissionType)EditorGUILayout.EnumPopup(TransmissionTypeFieldContent, specSheetData.TransmissionType);

                using (var gearCountChangedScope = new EditorGUI.ChangeCheckScope())
                {
                    specSheetData.GearCount = EditorGUILayout.DelayedIntField(GearCountFieldContent, specSheetData.GearCount);

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
                    else if (EditorGUILayout.DropdownButton(new GUIContent("Export", "Export the Car Spec Sheet"), FocusType.Passive))
                    {
                        GenericMenu menu = new GenericMenu();

                        menu.AddItem(new GUIContent("Export CarSpecSheet as Unitypackage", "Exports the CarSpecSheet ScriptableObject as a Unitypackage"), false, () => ExportAsUnitypackage());

                        menu.AddItem(new GUIContent("Save raw JSON to file", "Saves the raw JSON data from the spec sheet to a file"), false, () => SaveRawJSONToFile());

                        menu.ShowAsContext();
                    }
                }
                if (EditorGUILayout.DropdownButton(new GUIContent("Import", "Import CarSpecSheet from external data"), FocusType.Passive))
                {
                    GenericMenu menu = new GenericMenu();

                    menu.AddItem(new GUIContent("Import from JSON file", "Imports a spec sheet from JSON file"), false, () => ImportSpecSheetFromJSON());

                    if(CarSpecSheet.IsJSONValidCarSpecSheetData(EditorGUIUtility.systemCopyBuffer))
                    {
                        menu.AddItem(new GUIContent("Import from JSON in clipboard", "Imports a spec sheet from JSON"), false, () => ImportSpecSheetFromJSON(EditorGUIUtility.systemCopyBuffer));
                    }
                    else
                    {
                        menu.AddDisabledItem(new GUIContent("Import from JSON in clipboard", "Imports a spec sheet from JSON"));
                    }

                    menu.ShowAsContext();
                }
            }
        }

        private void LoadSpecSheet()
        {
            specSheetData = specSheet.Data;

            if (specSheetData.GearRatios == null) { specSheetData.GearRatios = new float[specSheetData.GearCount]; }

            if (specSheetData.EngineTorqueCurveKeyframes == null || specSheetData.EngineTorqueCurveKeyframes.Length == 0)
            {
                engineTorqueCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));

                specSheetData.EngineTorqueCurveKeyframes = engineTorqueCurve.keys;
            }
            else
            {
                engineTorqueCurve = new AnimationCurve(specSheetData.EngineTorqueCurveKeyframes);
            }
        }

        private void ImportSpecSheetFromJSON()
        {
            string path = EditorUtility.OpenFilePanel("Import CarSpecSheet from JSON", "", "json");

            if(string.IsNullOrEmpty(path) || !File.Exists(path)) { return; }

            string json;

            using (StreamReader reader = new StreamReader(path))
            {
                json = reader.ReadToEnd();
            }

            ImportSpecSheetFromJSON(json);
        }

        private void ImportSpecSheetFromJSON(string json)
        {
            specSheet.Data = CarSpecSheet.FromJSON(json);

            LoadSpecSheet();
        }

        private void ExportAsUnitypackage()
        {
            string path = EditorUtility.SaveFilePanel("Save CarSpecSheet as Unitypackage", "", string.Format("CarSpecSheet_{0}", specSheet.name), "unitypackage");

            if (string.IsNullOrEmpty(path)) { return; }

            AssetDatabase.ExportPackage(AssetDatabase.GetAssetPath(specSheet), path, ExportPackageOptions.Interactive);

            AssetDatabase.Refresh();
        }

        private void SaveRawJSONToFile()
        {
            string path = EditorUtility.SaveFilePanel("Save CarSpecSheet data as JSON", "", string.Format("CarSpecSheet_{0}", specSheet.name), "json");

            if (string.IsNullOrEmpty(path)) { return; }

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(specSheet.RawJsonData);
            }

            AssetDatabase.Refresh();
        }
    }
}

using UnityEditor;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.DataPresets.Editor
{
    [CustomEditor(typeof(EngineAudioPreset))]
    public class EngineAudioPresetEditor : UnityEditor.Editor
    {
        private bool showFields;

        public override void OnInspectorGUI()
        {
            if(showFields = GUILayout.Toggle(showFields, "Show Fields"))
            {
                base.OnInspectorGUI();
            }
        }
    }
}

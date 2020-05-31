using UnityEditor;
using UnityEngine;

namespace Path_Generator.Editor
{
    [CustomEditor (typeof (GeneratePath))]
    public class GeneratePathEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            GeneratePath pathGen = (GeneratePath)target;
            DrawDefaultInspector();
            if (GUILayout.Button("Generate"))
            {
                pathGen.Generate();
            }
        }
    }
}

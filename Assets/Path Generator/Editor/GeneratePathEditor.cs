using System;
using UnityEditor;
using UnityEngine;

namespace Path_Generator.Editors
{
    [CustomEditor(typeof(GeneratePath))]
    public class GeneratePathEditor : Editor
    {
        private GeneratePath _target;

        public void OnEnable()
        {
            _target = target as GeneratePath;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Generate"))
            {
                _target.Generate();
            }

            DrawDefaultInspector();
        }
    }
}
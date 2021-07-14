using System;
using UnityEngine;

namespace Path_Generator
{
    public class GeneratePath : MonoBehaviour
    {
        // public Polygon shape = new Polygon();
        public SuperShape shape = new SuperShape();

        public void Generate()
        {
            GameObject parent = new GameObject("Path");
            parent.AddComponent<Path>();
        
            Vector3[] path = shape.GenerateShape();
            for (int i=0; i < path.Length; i++)
            {
                GameObject pointObj = new GameObject("Point (" + i + ")");
                pointObj.transform.SetParent(parent.transform);
                pointObj.transform.position = path[i];
            }
        }

        private void OnDrawGizmos()
        {
            try
            {
                Vector3[] path = shape.GenerateShape();
                if (path.Length == 0) return;
                
                Vector3 startPosition = path[0];
                Vector3 prevPosition = startPosition;

                foreach (Vector3 position in path)
                {
                    // position = Vector3.Scale(position, off);
                    Gizmos.DrawLine(prevPosition, position);
                    Gizmos.DrawSphere(position, .03f);
                    prevPosition = position;
                }
                Gizmos.DrawLine(startPosition, prevPosition);
            }
            catch (Exception)
            {
                // ignored
            }
        }   
    }
}

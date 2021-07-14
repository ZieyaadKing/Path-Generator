using System;
using UnityEngine;

namespace Path_Generator
{
    public abstract class Shape
    {
        public int numberOfPoints = 1;
        public float radius = 1;
        protected Vector3 position = Vector3.zero;

        public abstract Vector3[] GenerateShape();

        public void SetPosition(Vector3 pos)
        {
            position = pos;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

    }

    [Serializable]
    public class Polygon : Shape
    {
        public override Vector3[] GenerateShape()
        {
            radius = Mathf.Abs(radius);
            numberOfPoints = Mathf.Abs(numberOfPoints);

            Vector3[] shape = new Vector3[numberOfPoints];

            float angle = 0;
            float step = (2 * Mathf.PI) / numberOfPoints;

            for (int i=0; i < numberOfPoints; i++)
            {
                shape[i] = new Vector3
                (
                    Mathf.Cos(angle) * Mathf.Rad2Deg, 
                    0, 
                    Mathf.Sin(angle) * Mathf.Rad2Deg
                ) * radius;
                shape[i] += position;
                angle += step;
            }
            return shape;
        }
    }

    [Serializable]
    public class SuperShape : Shape
    {
        public float a = 1;
        public float b = 1;
        public float m1 = 1;
        public float m2 = 1;
        public float n1 = 1;
        public float n2 = 1;
        public float n3 = 1;
    
        public override Vector3[] GenerateShape()
        {
            float increment = (2 * Mathf.PI / numberOfPoints);
            Vector3[] shape = new Vector3[numberOfPoints];
            int i = 0;

            for (float angle = 0; angle < 2 * Mathf.PI; angle += increment)
            {
                float part1 = (1 / a) * Mathf.Cos(angle * (m1 / 4));
                part1 = Mathf.Pow(Mathf.Abs(part1), n2);

                float part2 = (1 / b) * Mathf.Sin(angle * (m2 / 4));
                part2 = Mathf.Pow(Mathf.Abs(part2), n3);

                float part3 = n1 * Mathf.Sqrt(part1 + part2);
                float r = ((1 / part3) == 0) ? 0 : 1 / part3;

                float x = Mathf.Cos(angle);
                float z = Mathf.Sin(angle);
                shape[i] = new Vector3(x, 0, z) * radius * r;
                // angle += increment;
                i++;
            }
            return shape;
        }
    }

    [Serializable]
    public class Spiral : Shape 
    {
        public int numberOfSpirals = 1;

        public override Vector3[] GenerateShape()
        {
            Vector3[] shape = new Vector3[numberOfPoints];
            float angle = 0;
            float step = (numberOfSpirals * 2 * Mathf.PI / numberOfPoints);
            float rotation = radius;

            for (int i=0; i < numberOfPoints; i++)
            {
                float x = Mathf.Cos(angle);
                float z = Mathf.Sin(angle);
                shape[i] = new Vector3(x, 0, z) * rotation * angle;
            
                angle += step;
                rotation += step;
            }

            return shape;
        }
    }
}
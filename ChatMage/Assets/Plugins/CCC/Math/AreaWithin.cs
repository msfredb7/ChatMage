using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCC.Math
{
    public class AreaWithin
    {
        /// <summary>
        /// Trouve l'aire dans un ensemble de points
        /// </summary>
        /// <param name="points">Ensemble de points QUI DOIVENT ÊTRE ORDONNÉS</param>
        /// <param name="sensHoraire">Partant du points 0 vers n, est-ce que la forme est tracé dans le sens horaire ?</param>
        /// <returns></returns>
        static public float GetAreaWithin(Vector2[] points, bool sensHoraire)
        {
            if (points.Length == 3)
                return GetAreaWithinP(points[0], points[1], points[2]);

            if (points.Length < 3)
                return 0;

            /////////////////////////////////////////
            //Shape intérieur
            /////////////////////////////////////////
            Vector2[] interiorShape = new Vector2[Mathf.CeilToInt((float)points.Length / 2f)];

            int u = 0;
            for (int i = 0; i < points.Length; i += 2)
            {
                interiorShape[u] = points[i];
                u++;
            }

            float interiorArea = GetAreaWithin(interiorShape, sensHoraire);


            /////////////////////////////////////////
            //Les triangles extérieur
            /////////////////////////////////////////

            float totalArea = interiorArea;

            int lengthPair = points.Length;
            if (lengthPair % 2 != 0)
                lengthPair--;

            for (int i = 0; i < lengthPair; i += 2)
            {
                Vector2 pointA = points[i];
                Vector2 pointB = points[i + 1];
                Vector2 pointC = points[(i + 2) % points.Length];

                bool areaAAjouter = (IsLeft(pointA, pointC, pointB) == sensHoraire);

                float triangleArea = GetAreaWithinP(pointA, pointB, pointC);

                if (areaAAjouter)
                    totalArea += triangleArea;
                else
                    totalArea -= triangleArea;
            }

            /////////////////////////////////////////
            //Retour
            /////////////////////////////////////////

            return totalArea;
        }

        static public bool IsLeft(Vector2 start, Vector2 end, Vector2 point)
        {
            return ((end.x - start.x) * (point.y - start.y) - (end.y - start.y) * (point.x - start.x)) > 0;
        }

        static public float GetAreaWithinV(Vector2 vectorA, Vector2 vectorB, Vector2 vectorC)
        {
            //A² = (2ab + 2bc + 2ca – a² – b² – c²)/16
            float a = vectorA.sqrMagnitude;
            float b = vectorB.sqrMagnitude;
            float c = vectorC.sqrMagnitude;

            float areaSqr = (2 * ((a * b) + (b * c) + (c * a)) - (a * a) - (b * b) - (c * c)) / 16;

            return Mathf.Sqrt(areaSqr);
        }
        static public float GetAreaWithinP(Vector2 pointA, Vector2 pointB, Vector2 pointC)
        {
            return GetAreaWithinV(pointC - pointA, pointB - pointA, pointC - pointB);
        }
    }
}

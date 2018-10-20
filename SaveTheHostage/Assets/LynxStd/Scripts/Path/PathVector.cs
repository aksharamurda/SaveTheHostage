using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaStd
{
    public class PathVector
    {

        public static float Distance(Vector3 a, Vector3 b)
        {
            return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
        }

        public static Vector3 VectorTo(Vector3 fromVect, Vector3 toVect)
        {
            Vector3 v = toVect - fromVect;
            v.y = 0;
            return v;
        }
    }
}

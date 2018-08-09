using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class VectorExtensions
{
    public static float[] ConvertToFloatArray(this Vector3 vec)
    {
        int size = 3;
        float[] r = new float[size];
        for (int i = 0; i < size; i++)
        {
            r[i] = vec[i];
        }
        return r;
    }

    public static int[] ConvertToIntArray(this Vector3 vec)
    {
        var arr = vec.ConvertToFloatArray();
        return System.Array.ConvertAll(arr, item => (int)item);
    }

    #region Extract Axis
    public static float[] XY(this Vector3 vec)
    {
        int size = 2;
        float[] r = new float[size];
        r[0] = vec.x;
        r[1] = vec.y;
        return r;
    }

    public static float[] XZ(this Vector3 vec)
    {
        int size = 2;
        float[] r = new float[size];
        r[0] = vec.x;
        r[1] = vec.z;
        return r;
    }

    public static float[] YZ(this Vector3 vec)
    {
        int size = 2;
        float[] r = new float[size];
        r[0] = vec.y;
        r[1] = vec.z;
        return r;
    }
    #endregion

}

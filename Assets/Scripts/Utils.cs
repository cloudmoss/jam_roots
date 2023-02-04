using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    // Rotate a vector by an angle in degrees
    public static Vector2 RotateVector(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }

    // Convert vector3 to vector2int
    public static Vector2Int ToVector2Int(this Vector3 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    // Convert vector2 to vector2int
    public static Vector2Int ToVector2Int(this Vector2 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    // Convert vector2int to vector2
    public static Vector2 ToVector2(this Vector2Int v)
    {
        return new Vector2(v.x, v.y);
    }
}

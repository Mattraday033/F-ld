using System;
using UnityEngine;

//Unity ships Vector2Int and Vector3Int but no integer four-component vector, so this fills the gap.
//Selector packs its tile bounds into one of these where (x: upper, y: right, z: left, w: lower).
[Serializable]
public struct Vector4Int
{
    public int x;
    public int y;
    public int z;
    public int w;

    public Vector4Int(int x, int y, int z, int w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public static Vector4Int zero => new Vector4Int(0, 0, 0, 0);

    public static Vector4Int operator +(Vector4Int a, Vector4Int b)
    {
        return new Vector4Int(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
    }

    public static Vector4Int operator -(Vector4Int a, Vector4Int b)
    {
        return new Vector4Int(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
    }

    public static bool operator ==(Vector4Int a, Vector4Int b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
    }

    public static bool operator !=(Vector4Int a, Vector4Int b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        return obj is Vector4Int other && this == other;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 31 + x;
        hash = hash * 31 + y;
        hash = hash * 31 + z;
        hash = hash * 31 + w;
        return hash;
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ", " + z + ", " + w + ")";
    }
}

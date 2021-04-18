using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin<T>
{
    private HashSet<T> objects;
    private Vector3 position; // front left bottom corner
    private float edgeLen;

    public Bin(Vector3 p, float e)
    {
        position = p;
        edgeLen = e;
    }

    public void Add(T newObj)
    {
        objects.Add(newObj);
    }

    public void Remove(T deadObj)
    {
        objects.Remove(deadObj);
    }

    public HashSet<T> GetObjects()
    {
        return objects;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public float GetEdgeLength()
    {
        return edgeLen;
    }

    public bool Fits(Vector3 pos)
    {
        if (position.x <= pos.x && pos.x < position.x + edgeLen &&
            position.y <= pos.y && pos.y < position.y + edgeLen &&
            position.z <= pos.z && pos.z < position.z + edgeLen)
        {
            return true;
        }
        return false;
    }

    public void Draw()
    {
        Gizmos.color = Color.magenta;
        float half = edgeLen / 2;
        Gizmos.DrawWireCube(position + new Vector3(half,half,half),
                            Vector3.one * edgeLen);
    }
}

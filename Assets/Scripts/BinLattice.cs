using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinLattice<T>
{
    private Bin<T>[] bins;
    private Vector3 origin;
    private float edgeLen;
    private int numBinsInOneRow;
    private int numBinsInOneLayer;
    private float binEdgeLen;

    private int depth;

    public BinLattice(Vector3 origin, float edgeLen, int numBinsInOneRow)
    {
        this.origin = origin;
        this.edgeLen = edgeLen;
        this.numBinsInOneRow = numBinsInOneRow;
        numBinsInOneLayer = numBinsInOneRow * numBinsInOneRow;
        bins = new Bin<T>[(int)Mathf.Pow(this.numBinsInOneRow, 3f)];
        binEdgeLen = this.edgeLen / this.numBinsInOneRow;
        float offset = this.edgeLen / 2;

        for (int y = 0; y < this.numBinsInOneRow; y++)
        {
            for (int z = 0; z < this.numBinsInOneRow; z++)
            {
                for (int x = 0; x < this.numBinsInOneRow; x++)
                {
                    Bin<T> bin = new Bin<T>(new Vector3(x * binEdgeLen - offset,
                                                y * binEdgeLen - offset,
                                                z * binEdgeLen - offset),
                                                binEdgeLen);
                    bins[x +
                         y * numBinsInOneLayer +
                         z * this.numBinsInOneRow] = bin;
                }
            }
        }
    }

    /*
     * Checks if the postion is within the boundary of the bin lattice
     */
    public bool InRange(Vector3 pos)
    {
        pos -= origin;
        if (0 <= pos.x && pos.x < edgeLen &&
            0 <= pos.y && pos.y < edgeLen &&
            0 <= pos.z && pos.z < edgeLen)
        {
            return true;
        }
        return false;
    }

    public bool InRange(int index)
    {
        return index >= 0 && index < bins.Length;
    }

    /*
     * Calculates index of the bin that contains this position
     */
    public int GetBinIndex(Vector3 pos)
    {
        if (!InRange(pos))
            return -1;

        pos -= origin;
        int x = (int)(pos.x / binEdgeLen);
        int y = (int)(pos.y / binEdgeLen);
        int z = (int)(pos.z / binEdgeLen);

        return x + y * numBinsInOneLayer + z * numBinsInOneRow;
    }

    public void Add(Vector3 pos, T obj)
    {
        int index = GetBinIndex(pos);
        if (index < 0)
            return;
        bins[index].Add(obj);
    }

    public void Add(int index, T obj)
    {
        if (index < 0 || index >= bins.Length)
        {
            throw new System.InvalidOperationException("index " + index +
                                                        " is out of boundary");
        }
        bins[index].Add(obj);
    }

    public void Remove(Vector3 pos, T obj)
    {
        int index = GetBinIndex(pos);
        if (index < 0)
            return;
        bins[index].Remove(obj);
    }

    public void Remove(int index, T obj)
    {
        if (index < 0 || index >= bins.Length)
        {
            throw new System.InvalidOperationException("index " + index +
                                                        " is out of boundary");
        }
        bins[index].Remove(obj);
    }

    public bool Contains(Vector3 pos, T obj)
    {
        int index = GetBinIndex(pos);
        if (index < 0)
            return false;
        return bins[index].Contains(obj);
    }

    public bool Contains(int index, T obj)
    {
        if (index < 0 || index >= bins.Length)
        {
            throw new System.InvalidOperationException("index " + index +
                                                        " is out of boundary");
        }
        return bins[index].Contains(obj);
    }

    public void Clear()
    {
        foreach (Bin<T> b in bins)
        {
            b.Clear();
        }
    }

    public List<T> QuerySphere(Vector3 center, float radius)
    {
        return QueryCube(center, 2*radius);
    }

    public List<T> QueryCube(Vector3 center, float eLen)
    {
        List<T> res = new List<T>();
        center -= origin;
        float half = eLen / 2;
        int minX = (int) Mathf.Max(0f, (center.x - half) / binEdgeLen);
        int maxX = (int) Mathf.Min(numBinsInOneRow - 1,
                                   (center.x + half) / binEdgeLen);
        int minY = (int) Mathf.Max(0f, (center.y - half) / binEdgeLen);
        int maxY = (int) Mathf.Min(numBinsInOneRow - 1,
                                   (center.y + half) / binEdgeLen);
        int minZ = (int) Mathf.Max(0f, (center.z - half) / binEdgeLen);
        int maxZ = (int) Mathf.Min(numBinsInOneRow - 1,
                                   (center.z + half) / binEdgeLen);

        for (int y = minY; y <= maxY; y++)
        {
            for (int z = minZ; z <= maxZ; z++) 
            {
                for (int x = minX; x <= maxX; x++)
                {
                    int index = x + y * numBinsInOneLayer + z * numBinsInOneRow;
                    res.AddRange(bins[index].GetObjects());
                }
            }
        }

        return res;
    }

    public void Draw()
    {
        for (int y = 0; y < numBinsInOneRow; y++)
        {
            for (int z = 0; z < numBinsInOneRow; z++)
            {
                for (int x = 0; x < numBinsInOneRow; x++)
                {
                    bins[x +
                         y * numBinsInOneLayer +
                         z * numBinsInOneRow].Draw();
                }
            }
        }
    }
}


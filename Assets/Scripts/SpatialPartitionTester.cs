using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialPartitionTester : MonoBehaviour
{
    private BinLattice<float> bl;
    float xRange = 200f;
    float yRange = 300f;
    float zRange = 900f;

    // Start is called before the first frame update
    void Start()
    {
        bl = new BinLattice<float>(Vector3.zero, 500f, 10);
        
        // initialize elements
        for (float x = 11f; x < xRange; x+=7f)
        {
            for (float y = 3f; y < yRange; y += 9f)
            {
                for (float z = 25f; z < zRange; z += 13f)
                {
                    Vector3 pos = new Vector3(x/y, z/y, z/7f);
                    int index = bl.GetBinIndex(pos);
                    if (index >= 0)
                    {
                        bl.Add(index, pos.magnitude);
                    }
                }
            }
        }

        // check if each element is in the bin
        for (float x = 11f; x < xRange; x += 7f)
        {
            for (float y = 3f; y < yRange; y += 9f)
            {
                for (float z = 25f; z < zRange; z += 13f)
                {
                    Vector3 pos = new Vector3(x / y, z / y, z / 7f);
                    int index = bl.GetBinIndex(pos);
                    if (index >= 0)
                    {
                        if (!bl.Contains(index, pos.magnitude))
                        {
                            Debug.Log(pos.magnitude + " should be in bin " +
                                "index " + index);
                            throw new System.Exception("test failed");
                        }
                        // Debug.Log(pos.magnitude + " looks good");
                    }
                }
            }
        }

        bl = new BinLattice<float>(Vector3.zero, 15f, 3);
        int count = 0;
        for (int y = 0; y < 15; y+=5) 
        {
            for (int z = 0; z < 15; z += 5)
            {
                for (int x = 0; x < 15; x += 5)
                {
                    bl.Add(new Vector3(x + Random.Range(0,1f),
                                       y + Random.Range(0, 1f),
                                       z + Random.Range(0, 1f)), count++);
                }
            }
        }
        
        List<float> list = bl.QueryCube(new Vector3(7.5f, 7.5f, 0), 10f);
        Debug.Log("list length " + list.Count); // expect 18
        Debug.Log(System.String.Join(", ", list));
        list = bl.QueryCube(new Vector3(7.5f, 7.5f, 0), 9f);
        Debug.Log("list length " + list.Count); // expect 9
        Debug.Log(System.String.Join(", ", list));
        list = bl.QueryCube(new Vector3(0.5f, 0.7f, 4.99f), 9f);
        Debug.Log("list length " + list.Count); // expect 4
        Debug.Log(System.String.Join(", ", list));
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnDrawGizmos()
    {
        if (bl == null)
        {
            return;
        }
        bl.Draw();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialPartitionTester : MonoBehaviour
{
    private BinLattice<int> bl;

    // Start is called before the first frame update
    void Start()
    {
        bl = new BinLattice<int>(Vector3.zero, 50f, 10);
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

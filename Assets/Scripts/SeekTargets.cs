using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTargets : MonoBehaviour
{
    public List<Vector3> seekTargets;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        GenerateSeekTargets();
    }

    void GenerateSeekTargets()
    {
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                seekTargets.Add(new Vector3(i * 15, 5, j * 15));
            }
        }
    }
}

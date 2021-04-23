using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeObstacles : MonoBehaviour
{
    /*
    private GameObject cube1;
    public Texture m_MetallicTexture, m_Normal;
    Renderer m_Renderer;
    */
    private int cubeSpacing = 30; // cube spacing in multiple of default unit distance, must be int
    private int numCubesOnRow = 20; // total number of cubes will be n^3

    public void createCubes()
    {
        /*
        Texture diffuse = (Texture)Resources.Load("pattern 01/diffuse");
        Material mat = (Material)Resources.Load("pattern 01/Metal pattern 01");
        mat.SetTexture("_BumpMap", (Texture)Resources.Load("pattern 01/normal"));
        */
        GameObject prefab = Resources.Load<GameObject>("Prefabs/MetallicCube");
        int offset = -1 * cubeSpacing * numCubesOnRow / 2;

        prefab.transform.Rotate(45, 0, 45);
        for (int i = 0; i < numCubesOnRow; i++)
        {
            for (int j = 0; j < numCubesOnRow; j++)
            {
                for (int k = 0; k < numCubesOnRow; k++)
                {
                    Instantiate(prefab, new Vector3(i * cubeSpacing + offset, j * cubeSpacing + offset, k * cubeSpacing + offset), Quaternion.Euler(new Vector3(45, 0, 45)));
                    /*
                    cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube1.transform.position = Vector3.zero;
                    cube1.transform.localScale = new Vector3(5, 5, 5);
                    cube1.transform.Rotate(45, 0, 45);
                    cube1.GetComponent<Renderer>().material = mat;
                    cube1.GetComponent<Renderer>().material.mainTexture = diffuse;
                    cube1.AddComponent<Rigidbody>(); // needed for collision detection
                    cube1.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable
                    */
                }
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        createCubes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


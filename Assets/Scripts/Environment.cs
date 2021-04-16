using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Environment
{
    public GameObject cube;

    public void initEnvironment()
    {
        cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cube.transform.localScale = new Vector3(200, 200, 25);
        cube.transform.position = new Vector3(0, 0, -50.0f);
        cube.tag = "static";
        Rigidbody c;
        GameObject newinstan = GameObject.Instantiate(cube, Vector3.zero, Quaternion.identity) as GameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        initEnvironment();
    }
}

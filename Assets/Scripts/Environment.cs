using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Environment : MonoBehaviour
{
    public GameObject wall;


    public void createWall()
    {
        wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall.transform.position = new Vector3(0, 0, -50);
        wall.transform.localScale = new Vector3(200, 200, 25);
        wall.tag = "static"; // label wall so that boids will be able to identify as something to avoid
        wall.AddComponent<Rigidbody>(); // needed for collision detection
        wall.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable
    }

    // Start is called before the first frame update
    void Start()
    {
        createWall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


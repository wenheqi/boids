using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Environment : MonoBehaviour
{
    public GameObject ceiling, floor, wall1, wall2, wall3, wall4;

    public void createWall()
    {
        wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall1.transform.position = new Vector3(100, 0, 0);
        wall1.transform.localScale = new Vector3(25, 200, 200);
        wall1.tag = "static"; // label wall so that boids will be able to identify as something to avoid
        wall1.AddComponent<Rigidbody>(); // needed for collision detection
        wall1.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall2.transform.position = new Vector3(-100, 0, 0);
        wall2.transform.localScale = new Vector3(25, 200, 200);
        wall2.tag = "static"; // label wall so that boids will be able to identify as something to avoid
        wall2.AddComponent<Rigidbody>(); // needed for collision detection
        wall2.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall3.transform.position = new Vector3(0, 0, 100);
        wall3.transform.localScale = new Vector3(200, 200, 25);
        wall3.tag = "static"; // label wall so that boids will be able to identify as something to avoid
        wall3.AddComponent<Rigidbody>(); // needed for collision detection
        wall3.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall4.transform.position = new Vector3(0, 0, -100);
        wall4.transform.localScale = new Vector3(200, 200, 25);
        wall4.tag = "static"; // label wall so that boids will be able to identify as something to avoid
        wall4.AddComponent<Rigidbody>(); // needed for collision detection
        wall4.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        ceiling.transform.position = new Vector3(0, 100, 0);
        ceiling.transform.localScale = new Vector3(200, 25, 200);
        ceiling.tag = "static"; // label ceiling so that boids will be able to identify as something to avoid
        ceiling.AddComponent<Rigidbody>(); // needed for collision detection
        ceiling.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        floor.transform.position = new Vector3(0, -100, 0);
        floor.transform.localScale = new Vector3(200, 25, 200);
        floor.tag = "static"; // label floor so that boids will be able to identify as something to avoid
        floor.AddComponent<Rigidbody>(); // needed for collision detection
        floor.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Environment : MonoBehaviour
{
    private GameObject ceiling, floor, wall1, wall2, wall3, wall4;

    public void createWall()
    {
        Color c;
        wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall1.transform.position = new Vector3(50, 0, 0);
        wall1.transform.localScale = new Vector3(1, 100, 100);
        //wall1.tag = "static"; // label wall so that boids will be able to identify as something to avoid
        wall1.name = "static"; // label wall so that boids will be able to identify as something to avoid
        c = wall1.GetComponent<Renderer>().material.color;
        c.a = 0.5f;
        wall1.GetComponent<Renderer>().material.color = c;
        wall1.AddComponent<Rigidbody>(); // needed for collision detection
        wall1.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall2.transform.position = new Vector3(-50, 0, 0);
        wall2.transform.localScale = new Vector3(1, 100, 100);
        wall2.name = "static"; // label wall so that boids will be able to identify as something to avoid
        c = wall2.GetComponent<Renderer>().material.color;
        c.a = 0.5f;
        wall2.GetComponent<Renderer>().material.color = c;
        wall2.AddComponent<Rigidbody>(); // needed for collision detection
        wall2.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall3.transform.position = new Vector3(0, 0, 50);
        wall3.transform.localScale = new Vector3(100, 100, 1);
        wall3.name = "static"; // label wall so that boids will be able to identify as something to avoid
        c = wall3.GetComponent<Renderer>().material.color;
        c.a = 0.5f;
        wall3.GetComponent<Renderer>().material.color = c;
        wall3.AddComponent<Rigidbody>(); // needed for collision detection
        wall3.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall4.transform.position = new Vector3(0, 0, -50);
        wall4.transform.localScale = new Vector3(100, 100, 1);
        wall4.name = "static"; // label wall so that boids will be able to identify as something to avoid
        c = wall4.GetComponent<Renderer>().material.color;
        c.a = 0.5f;
        wall4.GetComponent<Renderer>().material.color = c;
        wall4.AddComponent<Rigidbody>(); // needed for collision detection
        wall4.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        ceiling.transform.position = new Vector3(0, 50, 0);
        ceiling.transform.localScale = new Vector3(100, 1, 100);
        ceiling.name = "static"; // label ceiling so that boids will be able to identify as something to avoid
        c = ceiling.GetComponent<Renderer>().material.color;
        c.a = 0.5f;
        ceiling.GetComponent<Renderer>().material.color = c;
        ceiling.AddComponent<Rigidbody>(); // needed for collision detection
        ceiling.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable

        floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        floor.transform.position = new Vector3(0, -50, 0);
        floor.transform.localScale = new Vector3(100, 1, 100);
        floor.name = "static"; // label floor so that boids will be able to identify as something to avoid
        c = floor.GetComponent<Renderer>().material.color;
        c.a = 0.5f;
        floor.GetComponent<Renderer>().material.color = c;
        ceiling.AddComponent<Rigidbody>(); // needed for collision detection
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


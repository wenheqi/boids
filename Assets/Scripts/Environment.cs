using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Environment : MonoBehaviour
{
    private GameObject ceiling, floor, wall1, wall2, wall3, wall4;

    public void createWall()
    {
        wall1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall1.transform.position = new Vector3(50, 0, 0);
        wall1.transform.localScale = new Vector3(1, 100, 100);
        //wall1.tag = "static"; // label wall so that boids will be able to identify as something to avoid
        wall1.name = "static"; // label wall so that boids will be able to identify as something to avoid
        wall1.AddComponent<Rigidbody>(); // needed for collision detection
        wall1.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable
        wall1.layer = 3; // set layer so that boids only perform OA with static obstacles

        wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall2.transform.position = new Vector3(-50, 0, 0);
        wall2.transform.localScale = new Vector3(1, 100, 100);
        wall2.name = "static"; // label wall so that boids will be able to identify as something to avoid
        wall2.AddComponent<Rigidbody>(); // needed for collision detection
        wall2.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable
        wall2.layer = 3; // set layer so that boids only perform OA with static obstacles

        wall3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall3.transform.position = new Vector3(0, 0, 50);
        wall3.transform.localScale = new Vector3(100, 100, 1);
        wall3.name = "static"; // label wall so that boids will be able to identify as something to avoid
        wall3.AddComponent<Rigidbody>(); // needed for collision detection
        wall3.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable
        wall3.layer = 3; // set layer so that boids only perform OA with static obstacles

        wall4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        wall4.transform.position = new Vector3(0, 0, -50);
        wall4.transform.localScale = new Vector3(100, 100, 1);
        wall4.name = "static"; // label wall so that boids will be able to identify as something to avoid
        wall4.AddComponent<Rigidbody>(); // needed for collision detection
        wall4.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable
        wall4.layer = 3; // set layer so that boids only perform OA with static obstacles

        ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        ceiling.transform.position = new Vector3(0, 50, 0);
        ceiling.transform.localScale = new Vector3(100, 1, 100);
        ceiling.name = "static"; // label ceiling so that boids will be able to identify as something to avoid
        ceiling.AddComponent<Rigidbody>(); // needed for collision detection
        ceiling.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable
        ceiling.layer = 3; // set layer so that boids only perform OA with static obstacles


        floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // set wall position and size
        floor.transform.position = new Vector3(0, -50, 0);
        floor.transform.localScale = new Vector3(100, 1, 100);
        floor.name = "static"; // label floor so that boids will be able to identify as something to avoid
        ceiling.AddComponent<Rigidbody>(); // needed for collision detection
        floor.AddComponent<Rigidbody>(); // needed for collision detection
        floor.GetComponent<Rigidbody>().isKinematic = true; // makes object unmovable
        floor.layer = 3; // set layer so that boids only perform OA with static obstacles
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


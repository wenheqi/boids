using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Boid boid;
    private List<Boid> boids = new List<Boid>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            // generate a random position
            Vector3 position = new Vector3(
                    Random.Range(-5f, 5f),
                    Random.Range(-5f, 5f),
                    Random.Range(-5f, 5f)
                );
            boids.Add(Instantiate(boid, position, Random.rotation));
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<Transform> transformList = new List<Transform>();

        // make a copy of the transforms of each boid
        foreach (Boid b in boids)
        {
            transformList.Add(b.transform);
        }

        foreach(Boid b in boids)
        {
            //b.Move();
            b.MoveInFlock(transformList);
        }
    }
}

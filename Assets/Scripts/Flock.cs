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
        for (int i = 0; i < 200; i++)
        {
            // generate a random position
            Vector3 position = new Vector3(
                    Random.Range(0.0f, 50.0f),
                    Random.Range(0.0f, 50.0f),
                    Random.Range(0.0f, 50.0f)
                );
            Boid b = Instantiate(boid, position, Random.rotation);
            //b.AlignmentEnabled = true;
            b.CohesionEnabled = true;
            //b.SeparationEnabled = true;
            boids.Add(b);
        }
    }

    // Update is called once per frame
    void Update()
    {
        List<BoidProperty> transformList = new List<BoidProperty>();

        // take a screenshot of each boid's position, rotation, etc.
        foreach (Boid b in boids)
        {
            transformList.Add(new BoidProperty(b));
        }

        foreach(Boid b in boids)
        {
            b.MoveInFlock(transformList);
        }
    }
}

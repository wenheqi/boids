using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    private List<Boid> boids = new List<Boid>();
    private float lowerSpawnBound = -25.0f;
    private float upperSpawnBound = 25.0f;

    // Start is called before the first frame update
    void Start()
    {
        Boid prefab = Resources.Load<Boid>("Prefabs/Fish");

        for (int i = 0; i < 200; i++)
        {
            // generate a random position
            Vector3 position = new Vector3(
                    Random.Range(lowerSpawnBound, upperSpawnBound),
                    Random.Range(lowerSpawnBound, upperSpawnBound),
                    Random.Range(lowerSpawnBound, upperSpawnBound)
                );
            Boid b = Boid.Create(prefab, position, Random.rotation);
            b.AvoidanceEnabled = true;
            b.AlignmentEnabled = true;
            b.CohesionEnabled = true;
            b.SeparationEnabled = true;
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

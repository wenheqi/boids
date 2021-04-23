using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    private List<Boid> boids = new List<Boid>();
    public Boid boid;

    // Start is called before the first frame update
    void Start()
    {
        //Boid prefab = Resources.Load<Boid>("Prefabs/Spaceship");

        for (int i = 0; i < 300; i++)
        {
            // generate a random position
            Vector3 position = new Vector3(
                    Random.Range(-2840.0f, -2790.0f),
                    Random.Range(-500.0f, -450.0f),
                    Random.Range(3700.0f, 3750.0f)
                );
            //Boid b = Boid.Create(prefab, position, Random.rotation);
            Boid b = Instantiate(boid, position, Random.rotation);
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

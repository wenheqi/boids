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
            boids.Add(Instantiate(boid, position, Random.rotation));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //List<BoidProperty> transformList = new List<BoidProperty>();

        // take a screenshot of each boid's position, rotation, etc.
        //foreach (Boid b in boids)
        //{
            //transformList.Add(new BoidProperty(b));
        //}

        foreach(Boid b in boids)
        {
            b.MoveInFlock(b.nearbyBoids);
        }
    }

    public Boid getClosestBoid(Boid sourceBoid)
    {
        float closestDistance = 500;
        Boid closestBoid = null;
        for (int i = 0; i < boids.Count; i++)
        {
            float dist = Vector3.Distance(boids[i].transform.position, sourceBoid.transform.position);
            if (dist < closestDistance && boids[i] != sourceBoid)
            {
                closestDistance = dist;
                closestBoid = boids[i];
            }
        }
        return closestBoid;
    }
}

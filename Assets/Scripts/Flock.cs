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
        
    }

    public Boid getClosestBoid()
    {
        float closestDistance = 500;
        Boid closestBoid = null;
        for (int i = 0; i < boids.Count; i++)
        {
            float dist = Vector3.Distance(boids[i].transform.position, this.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestBoid = boids[i];
            }
        }
        return closestBoid;
    }
}

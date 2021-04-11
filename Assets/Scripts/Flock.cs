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
        for (int i = 0; i < 5; i++)
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
        for (int i = 0; i < boids.Count; i++)
        {
            Boid closestBoid = getClosestBoid(boids[i]);
            Vector3 averageVelocity = (closestBoid.velocity + boids[i].velocity) * 0.5f;
            //Debug.Log("Source boid: " + boids[i].velocity + " Closest boid: " + closestBoid.velocity);

            // change boid's velocity based on the average of its velocity and the nearest boid's velocity
            boids[i].velocity = averageVelocity;
            
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

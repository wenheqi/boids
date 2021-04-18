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
        for (int i = 0; i < 20; i++)
        {
            // generate a random position
            Vector3 position = new Vector3(
                    Random.Range(0.0f, 50.0f),
                    Random.Range(0.0f, 50.0f),
                    Random.Range(0.0f, 50.0f)
                );
            Boid thisBoid = Instantiate(boid, position, Random.rotation);
            thisBoid.gameObject.name = i.ToString();
            boids.Add(thisBoid);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //List<BoidProperty> transformList = new List<BoidProperty>();
        List<GameObject> transformList = new List<GameObject>();

        // take a screenshot of each boid's position, rotation, etc.
        foreach (Boid b in boids)
        {
            GameObject boidClone = Instantiate(b.gameObject);
            transformList.Add(boidClone);
        }

        foreach(Boid b in boids)
        {
            b.MoveInFlock(transformList);
        }
    }

    /*
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
    */
}

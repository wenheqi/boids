using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Boid boid;
    private List<Boid> boids = new List<Boid>();
    // private int numBoids = 200;
    // global array of boids for current frame
    // public BoidProperty[] allBoid;
    //public int[] allBoid = new int[numBoids];
    public List<BoidProperty> allBoidPropertiesPerFrame = new List<BoidProperty>();

    // Start is called before the first frame update
    void Start()
    {
        // note: change number of boids to spawn in AllBoids.cs
        for (int i = 0; i < AllBoids.numBoids; i++)
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
        //List<GameObject> transformList = new List<GameObject>();

        // take a screenshot of each boid's position, rotation, etc.
        int i = 0;
        foreach (Boid b in boids)
        {
            AllBoids.allBoids[i] = new BoidProperty(b);
            //transformList.Add(new BoidProperty(b));
            ++i;
        }

        foreach(Boid b in boids)
        {
            b.MoveInFlock();
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

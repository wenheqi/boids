using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockSeekTester : MonoBehaviour
{
    public Boid boid;
    private List<Boid> boids;
    private BinLattice<BoidProperty> boidsDb;
    private float binEdgeLen = 500f;
    private int numOfBinsPerRow = 50;
    private int spawnSpread = 50;
    // Start is called before the first frame update
    void Start()
    {
        boids = new List<Boid>();

        for (int i = 0; i < 1000; i++)
        {
            float lowerBound = binEdgeLen / 2.0f - (spawnSpread / 2.0f);
            float upperBound = binEdgeLen / 2.0f + (spawnSpread / 2.0f);
            // generate a random position
            Vector3 position = new Vector3(
                    Random.Range(lowerBound, upperBound),
                    Random.Range(lowerBound, upperBound),
                    Random.Range(lowerBound, upperBound)
                );
            boids.Add(Instantiate(boid, position, Random.rotation));
        }
        Debug.Log("num of boids: " + boids.Count);
    }

    // Update is called once per frame
    void Update()
    {
        boidsDb = new BinLattice<BoidProperty>(
                                                Vector3.zero,
                                                binEdgeLen,
                                                numOfBinsPerRow);


        // take a screenshot of each boid's position, rotation, etc.
        foreach (Boid b in boids)
        {
            boidsDb.Add(b.transform.position, new BoidProperty(b));
        }

        foreach (Boid b in boids)
        {
            List<BoidProperty> neighbours = boidsDb.QuerySphere(
                b.transform.position,
                b.GetPerceptionDistance());
            b.MoveInFlock(neighbours);
        }
    }

    private void OnDrawGizmos()
    {
        if (boidsDb == null)
            return;
        // draw bins
        boidsDb.Draw();
    }
}

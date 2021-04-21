using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceTester : MonoBehaviour
{
    public Boid boid;
    private List<Boid> boids;
    private BinLattice<BoidProperty> boidsDb;
    // Start is called before the first frame update
    void Start()
    {
        boids = new List<Boid>();

        for (int i = 0; i < 2000; i++)
        {
            // generate a random position
            Vector3 position = new Vector3(
                    Random.Range(0.0f, 50.0f),
                    Random.Range(0.0f, 50.0f),
                    Random.Range(0.0f, 50.0f)
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
                                                50f,
                                                10);


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
            //b.Teleport();
        }
    }

    private void OnDrawGizmos()
    {
        if (boidsDb == null)
            return;
        // draw bins
        // boidsDb.Draw();
    }
}

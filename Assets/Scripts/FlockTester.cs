using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockTester : MonoBehaviour
{
    public Boid boid;
    private List<Boid> boids;
    private BinLattice<BoidProperty> boidsDb;
    // Start is called before the first frame update
    void Start()
    {
        boids = new List<Boid>();

        Boid prefab = Resources.Load<Boid>("Prefabs/Fish");
        for (int i = 0; i < 1000; i++)
        {
            // generate a random position
            Vector3 position = new Vector3(
                    Random.Range(0.0f, 50.0f),
                    Random.Range(0.0f, 50.0f),
                    Random.Range(0.0f, 50.0f)
                );
            Boid b = Boid.Create(prefab, position, Random.rotation);
            b.AlignmentEnabled = true;
            b.CohesionEnabled = true;
            b.SeparationEnabled = true;
            b.GoalSeekingEnabled = false;
            boids.Add(b);
            //boids.Add(Instantiate(boid, position, Random.rotation));
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockTester : MonoBehaviour
{
    public Boid boid;
    private List<Boid> boids;
    private BinLattice<Vector3> boidsDb;
    public List<Vector3> seekTargets;
    public List<Vector3> sphereTargets;
    // Start is called before the first frame update
    void Start()
    {
        boids = new List<Boid>();
        sphereTargets = GenerateSpherePoints();

        foreach (Vector3 target in sphereTargets)
        {
            GameObject x = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x.transform.position = target;
        }
        Boid prefab = Resources.Load<Boid>("Prefabs/Fish");

        for (int i = 0; i < 500; i++)
        {
            // generate a random position
            //Vector3 position = new Vector3(
                    //Random.Range(-25.0f, 25.0f),
                    //Random.Range(-25.0f, 25.0f),
                    //Random.Range(-25.0f, 25.0f)
                //);
            Vector3 position = new Vector3(
                    Random.Range(-130.0f, -100.0f),
                    Random.Range(-15.0f, 15.0f),
                    Random.Range(-15.0f, 15.0f)
                );
            //Boid b = Boid.Create(prefab, position, Random.rotation);
            Boid b = Boid.Create(prefab, position, Quaternion.LookRotation(new Vector3(1,0,0)));
            b.AlignmentEnabled = true;
            b.CohesionEnabled = true;
            b.SeparationEnabled = true;
            b.GoalSeekingEnabled = true;
            b.CreateGoalList(sphereTargets);
            boids.Add(b);
            //boids.Add(Instantiate(boid, position, Random.rotation));
        }
        Debug.Log("num of boids: " + boids.Count);
    }

    // Update is called once per frame
    void Update()
    {
        List<BoidProperty> transformList = new List<BoidProperty>();
        boidsDb = new BinLattice<Vector3>(
                                                Vector3.zero,
                                                120f,
                                                20);

        foreach (Vector3 s in sphereTargets)
        {
            boidsDb.Add(s, s);
        }
        // take a screenshot of each boid's position, rotation, etc.
        foreach (Boid b in boids)
        {
            transformList.Add(new BoidProperty(b));
        }

        /*
        foreach(Boid b in boids)
        {
            b.MoveInFlock(transformList);
        }
        */

        /*
        // take a screenshot of each boid's position, rotation, etc.
        foreach (Boid b in boids)
        {
            boidsDb.Add(b.transform.position, new BoidProperty(b));
        }
        */

        foreach (Boid b in boids)
        {
            List<Vector3> nearestSeekObjects = boidsDb.QuerySphere(
                b.transform.position,
                b.GetPerceptionDistance());
            //Debug.Log("num nearby objects " + nearestSeekObjects.Count);
            b.MoveInFlock(transformList, nearestSeekObjects);
        }
    }

    /*
    private void OnDrawGizmos()
    {
        if (boidsDb == null)
            return;
        // draw bins
        boidsDb.Draw();
    }
    */

    List<Vector3> GenerateSpherePoints()
    {
        float sphereRadius = 50.0f;
        List<Vector3> generatedPoints = new List<Vector3>();
        for (int i = 0; i < 1000; i++)
        {
            Vector3 newP = new Vector3(GenerateGaussianRandom(1.0f, 10.0f), GenerateGaussianRandom(1.0f, 10.0f), GenerateGaussianRandom(1.0f, 10.0f));
            newP = Vector3.Normalize(newP);
            newP *= sphereRadius;
            generatedPoints.Add(newP);
        }
        return generatedPoints;
    }

    float GenerateGaussianRandom(float mu, float sigma)
    {
        float rand1 = Random.Range(0.0f, 1.0f);
        float rand2 = Random.Range(0.0f, 1.0f);

        float n = Mathf.Sqrt(-2.0f * Mathf.Log(rand1)) * Mathf.Cos((2.0f * Mathf.PI) * rand2);

        return (mu + sigma * n);
    }
}

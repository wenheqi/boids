using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    private List<Boid> boids = new List<Boid>();
    public List<Vector3> seekTargets;
    public List<Vector3> sphereTargets;
    SphereCollider m_Collider;

    // Start is called before the first frame update
    void Start()
    {
        sphereTargets = GenerateSpherePoints();
        /*
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = Vector3.zero;
        sphere.transform.localScale = new Vector3(100, 100, 100);
        sphere.AddComponent<Rigidbody>();
        sphere.GetComponent<Rigidbody>().isKinematic = true;
        m_Collider = sphere.GetComponent<SphereCollider>();
        m_Collider.radius = 110f;
        */

        //GenerateSeekTargets();
        //foreach (Vector3 target in seekTargets)
        foreach (Vector3 target in sphereTargets)
        {
            GameObject x = GameObject.CreatePrimitive(PrimitiveType.Cube);
            x.transform.position = target;
        }
        Boid prefab = Resources.Load<Boid>("Prefabs/Fish");

        for (int i = 0; i < 2; i++)
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
            b.GoalSeekingEnabled = true;
            //b.CreateGoalList(seekTargets);
            b.CreateGoalList(sphereTargets);
            //b.Goal = new Vector3(-10f, 10f, -10f);
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

    void GenerateSeekTargets()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                seekTargets.Add(new Vector3(i * 10, 5, j * 10));
            }
        }
    }

    List<Vector3> GenerateSpherePoints()
    {
        float sphereRadius = 100.0f;
        List<Vector3> generatedPoints = new List<Vector3>();
        for (int i = 0; i < 500; i++)
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

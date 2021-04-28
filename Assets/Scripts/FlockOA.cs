using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockOA : MonoBehaviour
{
    [Range(0.1f, 10f)]
    public float boundingRadius = 0.33f;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 100f)]
    public float maxForce = 100f;
    public bool alignmentEnabled = true;
    [Range(1f, 100f)]
    public float alignmentDist = 2f;
    [Range(1f, 100f)]
    public float alignmentStrength = 5f;
    [Range(1f, 180f)]
    public float alignmentAngle = 180f;
    public bool cohesionEnabled = true;
    [Range(1f, 100f)]
    public float cohesionDist = 2f;
    [Range(1f, 100f)]
    public float cohesionStrength = 5f;
    [Range(1f, 180f)]
    public float cohesionAngle = 180f;
    public bool separationEnabled = true;
    [Range(1f, 100f)]
    public float separationDist = 2f;
    [Range(1f, 100f)]
    public float separationStrength = 5f;
    [Range(1f, 180f)]
    public float separationAngle = 180f;
    public bool avoidEnabled = true;
    [Range(1f, 100f)]
    public float avoidDist = 3f;
    [Range(1f, 100f)]
    public float avoidStrength = 35f;
    [Range(1f, 180f)]
    public float avoidAngle = 180f;
    public bool goalSeekingEnabled = true;
    public Vector3 goal = Vector3.zero;
    [Range(1f, 100f)]
    public float goalSeekingStrength = 18.0f;

    private List<Boid> boids = new List<Boid>();

    // Start is called before the first frame update
    void Start()
    {
        Boid prefab = Resources.Load<Boid>("Prefabs/Fish");

        for (int i = 0; i < 100; i++)
        {
            // generate a random position
            Vector3 position = new Vector3(
                    Random.Range(-3f, 3f),
                    Random.Range(3f, 5f),
                    Random.Range(-3f, 3f)
                );
            Boid b = Boid.Create(prefab, position, Random.rotation);
            b.gameObject.layer = 7; // set to boids layer
            b.BoundingRadius = boundingRadius;
            b.MaxSpeed = maxSpeed;
            b.MaxForce = maxForce;
            b.AlignmentEnabled = alignmentEnabled;
            b.AlignmentDist = alignmentDist;
            b.AlignmentStrength = alignmentStrength;
            b.AlignmentAngle = alignmentAngle;
            b.CohesionEnabled = cohesionEnabled;
            b.CohesionDist = cohesionDist;
            b.CohesionStrength = cohesionStrength;
            b.CohesionAngle = cohesionAngle;
            b.SeparationEnabled = separationEnabled;
            b.SeparationDist = separationDist;
            b.SeparationStrength = separationStrength;
            b.SeparationAngle = separationAngle;
            b.AvoidEnabled = avoidEnabled;
            b.AvoidDist = avoidDist;
            b.AvoidStrength = avoidStrength;
            b.AvoidAngle = avoidAngle;
            b.GoalSeekingEnabled = goalSeekingEnabled;
            b.Goal = goal;
            b.GoalSeekingStrength = goalSeekingStrength;
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
            b.BoundingRadius = boundingRadius;
            b.MaxSpeed = maxSpeed;
            b.MaxForce = maxForce;
            b.AlignmentEnabled = alignmentEnabled;
            b.AlignmentDist = alignmentDist;
            b.AlignmentStrength = alignmentStrength;
            b.AlignmentAngle = alignmentAngle;
            b.CohesionEnabled = cohesionEnabled;
            b.CohesionDist = cohesionDist;
            b.CohesionStrength = cohesionStrength;
            b.CohesionAngle = cohesionAngle;
            b.SeparationEnabled = separationEnabled;
            b.SeparationDist = separationDist;
            b.SeparationStrength = separationStrength;
            b.SeparationAngle = separationAngle;
            b.AvoidEnabled = avoidEnabled;
            b.AvoidDist = avoidDist;
            b.AvoidStrength = avoidStrength;
            b.AvoidAngle = avoidAngle;
            b.GoalSeekingEnabled = goalSeekingEnabled;
            b.Goal = goal;
            b.GoalSeekingStrength = goalSeekingStrength;
            transformList.Add(new BoidProperty(b));
        }

        foreach (Boid b in boids)
        {
            b.MoveInFlock(transformList);
        }
    }
}

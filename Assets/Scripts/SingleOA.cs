using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleOA : MonoBehaviour
{
    private Boid b;
    [Range(1f, 100f)]
    public float maxSpeed = 5f;
    [Range(1f, 100f)]
    public float maxForce = 100f;
    public bool alignmentEnabled = true;
    [Range(1f, 100f)]
    public float alignmentDist = 2f;
    [Range(1f, 100f)]
    public float alignmentStrength = 1f;
    [Range(1f, 180f)]
    public float alignmentAngle = 180f;
    public bool cohesionEnabled = true;
    [Range(1f, 100f)]
    public float cohesionDist = 1f;
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
    public float avoidDist = 2f;
    [Range(1f, 100f)]
    public float avoidStrength = 20f;
    [Range(1f, 180f)]
    public float avoidAngle = 180f;
    // Start is called before the first frame update
    void Start()
    {
        Boid prefab = Resources.Load<Boid>("Prefabs/Fish");
        b = Boid.Create(prefab, new Vector3(0f,4f,0f), Random.rotation);
        b.gameObject.layer = 7; // set to boids layer
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
    }

    // Update is called once per frame
    void Update()
    {
        b.gameObject.layer = 7; // set to boids layer
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
        b.SteerToAvoidObstacle();
        Debug.Log(transform.TransformDirection(transform.up));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private static float MAX_SPEED = 2f;
    [SerializeField]
    public Vector3 velocity; // world space velocity
    public List<GameObject> nearbyBoids; // list of other boids that are within boidDetectionRadius
    //SphereCollider boidCollider;
    private float boidDetectionRadius = 2.5f;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.ClampMagnitude(
                new Vector3(
                    Random.Range(-MAX_SPEED, MAX_SPEED),
                    Random.Range(-MAX_SPEED, MAX_SPEED),
                    Random.Range(-MAX_SPEED, MAX_SPEED)
                ),
                MAX_SPEED
            );
        transform.rotation = Quaternion.LookRotation(velocity);
        this.gameObject.AddComponent<Rigidbody>(); // rigidbody is needed for collisions
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // isKinematic set to true ignores physics when collisions happen

        // a second collision sphere can be made is needed
        //boidCollider = GetComponent<SphereCollider>();
        //this.boidCollider = this.gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        //this.boidCollider.center = this.transform.position;
        //this.boidCollider.radius = boidDetectionRadius;
        //this.boidCollider.isTrigger = true;

        SphereCollider a = GetComponent<SphereCollider>();
        a.isTrigger = true;
        a.center = this.transform.position;
        a.radius = boidDetectionRadius;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(velocity);
        this.nearbyBoids.Sort(byDist);
    }

    private void OnTriggerEnter(Collider collision)
    {
        nearbyBoids.Add(collision.gameObject);
    }

    private void OnTriggerExit(Collider collision)
    {
        nearbyBoids.Remove(collision.gameObject);
    }

    private int byDist(GameObject a, GameObject b)
    {
        float distToA = Vector3.Distance(transform.position, a.transform.position);
        float distToB = Vector3.Distance(transform.position, b.transform.position);
        return distToA.CompareTo(distToB);
    }
}

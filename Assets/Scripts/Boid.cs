using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private static float MAX_SPEED = 9f;
    private static float MAX_FORCE = 27f;
    [SerializeField]
    private Vector3 velocity; // world space velocity
    private float mass;

    private float alignmentDist;
    private float cohesionDist;
    private float separationDist;

    private float alignmentForceCoef;
    private float cohesionForceCoef;
    private float separationForceCoef;
    public List<GameObject> nearbyBoids; // list of other boids that are within boidDetectionRadius
    //BoxCollider boxCollider;
    private float boidDetectionRadius = 20.0f;
    public Rigidbody rb;

    // constants for box collider
    private Vector3 boxSize = new Vector3(5, 5, 25);

    // Start is called before the first frame update
    void Start()
    {
        mass = 1f;
        alignmentDist = 7.5f;
        cohesionDist = 9.0f;
        // Note: if separation distance is too small, two fish will kiss
        // each other repeatedly
        separationDist = 5.0f;

        alignmentForceCoef = 8.0f;
        cohesionForceCoef = 8.0f;
        separationForceCoef = 12.0f;

        velocity = Random.onUnitSphere * MAX_SPEED;
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }

        this.gameObject.AddComponent<Rigidbody>(); // rigidbody is needed for collisions
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // isKinematic set to true ignores physics when collisions happen

        // box collision zone in front of boid used for obstacle avoidance (not using cylinder because it's not part of the library)
        //BoxCollider bc = GetComponent<BoxCollider>();
        BoxCollider bc = this.gameObject.AddComponent(typeof(BoxCollider)) as BoxCollider;
        bc.isTrigger = true;
        bc.center = this.transform.position;
        bc.size = boxSize;

        //this.boidCollider = this.gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        //this.boidCollider.center = this.transform.position;
        //this.boidCollider.radius = boidDetectionRadius;
        //this.boidCollider.isTrigger = true;

        //SphereCollider a = GetComponent<SphereCollider>();
        //a.isTrigger = true;
        //a.center = this.transform.position;
        //a.radius = boidDetectionRadius;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    /*
     * Move forward based on current velocity
     */
    public void Move()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    /*
     * Move based on Craig Reynolds' three rules
     */
    public void MoveInFlock(List<GameObject> flock)
    {
        // steering velocity of alignemnt, cohesion and separation
        Vector3 steeringVelocity = Vector3.zero;
        // steering force of alignemnt, cohesion and separation
        Vector3 steeringForce = Vector3.zero;

        // desired alignment steering velocity/direction
        Vector3 alignmentV = Align(flock);
        Vector3 alignmentD = alignmentV;
        alignmentD.Normalize();

        // desired cohesion steering velocity/direction
        Vector3 cohesionV = Cohere(flock);
        Vector3 cohesionD = cohesionV;
        cohesionD.Normalize();

        // desired separation steering velocity/direction
        Vector3 separationV = Separate(flock);
        Vector3 separationD = separationV;
        separationD.Normalize();

        steeringVelocity += alignmentV;
        steeringForce += alignmentForceCoef * alignmentD;
        steeringVelocity += cohesionV;
        steeringForce += cohesionForceCoef * cohesionD;
        steeringVelocity += separationV;
        steeringForce += separationForceCoef * separationD;

        // steering_force = truncate (steering_direction, max_force)
        steeringForce = Vector3.ClampMagnitude(steeringForce, MAX_FORCE);
        steeringVelocity = Vector3.ClampMagnitude(steeringVelocity, MAX_SPEED);
        // acceleration = steering_force / mass
        Vector3 acceleration = steeringForce / mass;
        // avoid overshooting
        Vector3 deltaVelocity = Vector3.ClampMagnitude(
            acceleration * Time.deltaTime, steeringVelocity.magnitude);
        // velocity = truncate(velocity + acceleration, max_speed)
        velocity = Vector3.ClampMagnitude(velocity + deltaVelocity, MAX_SPEED);

        Move();
    }

    /*
     * Cohesion: steer to move toward the average position of local flockmates
     * 
     * Reference:
     * Reynolds, C. W. (1999) Steering Behaviors For Autonomous Characters, in 
     * the proceedings of Game Developers Conference 1999 held in San Jose, 
     * California. Miller Freeman Game Group, San Francisco, California. Pages 
     * 763-782.
     * 
     * TODO: current implementation has O(N^2) complexity as each boid has to
     * loop through the whole flock to calculate the centroid. Need to optimize
     * the algorithm.
     * 
     * Parameters:
     *  List<BoidProperty> flockmates: a list of properties of all boids 
     *  currently in the space
     * Return:
     *  the difference between desired velocity and the object’s current 
     *  velocity
     */
    private Vector3 Cohere(List<GameObject> flockmates)
    {
        Vector3 steering = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        int numFlockmates = 0;

        foreach(GameObject flockmate in flockmates)
        {

            // exclude boid itself from blockmates
            if (flockmate.transform.position != transform.position &&
                Vector3.Distance(this.transform.position,
                flockmate.transform.position) <= cohesionDist)
            {
                // positon is in world space
                avgPosition += flockmate.transform.position;
                numFlockmates++;
            }
        }

        if (numFlockmates > 0)
        {
            avgPosition /= numFlockmates;
            // desired_velocity = normalize (position - target) * max_speed
            // Note: the paper seems to be wrong about the vector calculation
            // should be target - position instead of position - target
            Vector3 desiredVelocity = avgPosition - transform.position;
            desiredVelocity.Normalize();
            desiredVelocity *= MAX_SPEED;
            // steering = desired_velocity - velocity
            steering = desiredVelocity - velocity;
        }
        
        return steering;
    }

    private Vector3 Separate(List<GameObject> flockmates)
    {
        Vector3 steering = Vector3.zero;
        Vector3 flee = Vector3.zero;
        int numFlockmates = 0;

        foreach (GameObject flockmate in flockmates)
        {
            // exclude boid itself from blockmates
            if (flockmate.transform.position != transform.position &&
                Vector3.Distance(this.transform.position,
                flockmate.transform.position) <= separationDist)
            {
                if (Vector3.Distance(transform.position, flockmate.transform.position) <
                    separationDist)
                {
                    // calculate how far and in what direction the boid wants to
                    // flee
                    Vector3 offset = transform.position - flockmate.transform.position;
                    offset /= offset.sqrMagnitude;
                    flee += offset;
                    numFlockmates++;
                }
            }
        }

        if (numFlockmates > 0)
        {
            flee /= numFlockmates;
            flee.Normalize();
            steering = flee * MAX_SPEED - velocity;
        }

        return steering;
    }

    /*
     * Alignment: steer towards the average heading of local flockmates
     * 
     * Reference:
     * Reynolds, C. W. (1999) Steering Behaviors For Autonomous Characters, in 
     * the proceedings of Game Developers Conference 1999 held in San Jose, 
     * California. Miller Freeman Game Group, San Francisco, California. Pages 
     * 763-782.
     * 
     * Note: in Craig's implementation of alignment, cohesion and separation in
     * OpenSteer library, the return of his functions is actually a normalized
     * steering direction vector. After get the direction vector, he multiplies
     * each vector with a coefficient and sum them to get a steering force.
     * Finally he uses this force (truncated if necessary) to calculate the new
     * acceleration, velocity and position.
     * I metion this mainly because in the paper above, the term used is 
     * "steering vector" and in seek behavior it was explicitly mentioned that 
     * "The steering vector is the difference between this desired velocity and 
     * the character’s current velocity", which looks really confusing to me.
     * 
     * Parameters:
     *  List<BoidProperty> flockmates: a list of properties of all boids 
     *  currently in the space
     * Return:
     *  the difference between this desired velocity and the character’s current
     *  velocity
     */
    private Vector3 Align(List<GameObject> flockmates)
    {
        Vector3 steering = Vector3.zero;
        Vector3 avgForward = Vector3.zero;
        int numFlockmates = 0;

        foreach (GameObject flockmate in flockmates)
        {
            // exclude boid itself from flockmates
            if (transform.position != flockmate.transform.position &&
                Vector3.Distance(this.transform.position,
                flockmate.transform.position) <= alignmentDist)
            {
                avgForward += flockmate.transform.forward;
                numFlockmates++;
            }
        }

        if (numFlockmates > 0)
        {
            avgForward /= numFlockmates;
            avgForward.Normalize();
            steering = avgForward * MAX_SPEED - velocity;
        }

        return steering;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetType() == typeof(BoxCollider) && collision.gameObject.name == "static")
        {
            // detected avoidance collision
        }
        nearbyBoids.Add(collision.gameObject);
    }

    private void OnTriggerExit(Collider collision)
    {
        nearbyBoids.Remove(collision.gameObject);
    }

    /*
    private int byDist(GameObject a, GameObject b)
    {
        float distToA = Vector3.Distance(transform.position, a.transform.position);
        float distToB = Vector3.Distance(transform.position, b.transform.position);
        return distToA.CompareTo(distToB);
    }
    */
}

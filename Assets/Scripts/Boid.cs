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

    //private float alignmentDist;
    //private float cohesionDist;
    private float separationDist;

    private float alignmentForceCoef;
    private float cohesionForceCoef;
    private float separationForceCoef;

    public List<GameObject> nearbyObjects; // list of nearby objects that should be avoided
    public List<int> nearbyBoids; // list of other boids that are within boidDetectionRadius
    //BoxCollider boxCollider;
    private float boidDetectionRadius = 8.0f;
    public Rigidbody rb;

    private Vector3 avoidVector;
    private Vector3 flockVector;

    // weights for combining flock and avoid vectors [0,1], sum = 1, higher is more weight
    private float flockVectorWeight = 0.2f;
    private float avoidVectorWeight = 0.8f;

    // constants for box collider
    private Vector3 boxSize = new Vector3(2, 2, 10);

    // Start is called before the first frame update
    void Start()
    {
        mass = 1f;
        //alignmentDist = 7.5f;
        //cohesionDist = 9.0f;
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

        SphereCollider a = GetComponent<SphereCollider>();
        a.isTrigger = true;
        a.center = this.transform.position;
        a.radius = boidDetectionRadius;
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
    public void MoveInFlock()
    {
        if (nearbyObjects.Count > 0)
        {
            getRaycastVector();
            getFlockVector(true);
            //if (avoidVector != Vector3.zero)
            //{
                //velocity = avoidVectorWeight * avoidVector + flockVectorWeight * flockVector; 
            //}

            // combine the results of avoid and flock vector (weighted)
            //RaycastHit hit;
            //if (Physics.Raycast(transform.position, transform.forward, out hit))
            //{
                //Vector3 normalizedHitNormal = Vector3.Normalize(hit.normal);
                //Vector3 avoidVelocityVector = velocity - 2 * Vector3.Dot(velocity, normalizedHitNormal) * normalizedHitNormal;
                //avoidVector = Avoid(avoidVelocityVector);
            //}
            //else
            //{
                //avoidVector = Vector3.zero;
            //}
        }
        else {
            getFlockVector(false);
        }

        Move();
    }

    public void getFlockVector(bool isAvoid)
    {
        // steering velocity of alignemnt, cohesion and separation
        Vector3 steeringVelocity = Vector3.zero;
        // steering force of alignemnt, cohesion and separation
        Vector3 steeringForce = Vector3.zero;

        // desired alignment steering velocity/direction
        Vector3 alignmentV = Align();
        Vector3 alignmentD = alignmentV;
        alignmentD.Normalize();

        // desired cohesion steering velocity/direction
        Vector3 cohesionV = Cohere();
        Vector3 cohesionD = cohesionV;
        cohesionD.Normalize();

        // desired separation steering velocity/direction
        Vector3 separationV = Separate();
        Vector3 separationD = separationV;
        separationD.Normalize();

        steeringVelocity += alignmentV;
        steeringForce += alignmentForceCoef * alignmentD;
        steeringVelocity += cohesionV;
        steeringForce += cohesionForceCoef * cohesionD;
        steeringVelocity += separationV;
        steeringForce += separationForceCoef * separationD;

        // modify steering vector if boid detects an object in front of it
        if (isAvoid && avoidVector != Vector3.zero)
        {
            steeringForce = flockVectorWeight * steeringForce + avoidVectorWeight * avoidVector; 
        }
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
        // set minimum velocity, because otherwise boids tend to stop and spin
        if (velocity.sqrMagnitude < 3.0f)
        {
            velocity = velocity.normalized * 3.0f;
        }
        
    }

    public void getRaycastVector()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            // normalized vector reflecting from hit surface
            Vector3 normalizedHitNormal = Vector3.Normalize(hit.normal);
            // reflected ray vector from the boid vector around the normal surface vector
            avoidVector = velocity - 2 * Vector3.Dot(velocity, normalizedHitNormal) * normalizedHitNormal;
        }
        else
        {
            avoidVector = Vector3.zero;
        }
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
    private Vector3 Cohere()
    {
        Vector3 steering = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;

        foreach (int i in nearbyBoids)
        { 
            avgPosition += AllBoids.allBoids[i].position;
        }

        if (nearbyBoids.Count > 0)
        {
            avgPosition /= nearbyBoids.Count;
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

    private Vector3 Separate()
    {
        Vector3 steering = Vector3.zero;
        Vector3 flee = Vector3.zero;

        foreach (int i in nearbyBoids)
        {
            if (Vector3.Distance(transform.position, AllBoids.allBoids[i].position) <
                separationDist)
            {
                // calculate how far and in what direction the boid wants to
                // flee
                Vector3 offset = transform.position - AllBoids.allBoids[i].position;
                offset /= offset.sqrMagnitude;
                flee += offset;
            }
        }

        if (nearbyBoids.Count > 0)
        {
            flee /= nearbyBoids.Count;
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
    private Vector3 Align()
    {
        Vector3 steering = Vector3.zero;
        Vector3 avgForward = Vector3.zero;

        foreach (int i in nearbyBoids)
        {
            avgForward += AllBoids.allBoids[i].forward;
        }

        if (nearbyBoids.Count > 0)
        {
            avgForward /= nearbyBoids.Count;
            avgForward.Normalize();
            steering = avgForward * MAX_SPEED - velocity;
        }

        return steering;
    }

    /*
     * Returns the desired steering velocity
     */
    private Vector3 SteerAway(GameObject target)
    {
        Vector3 desiredD = target.transform.position - transform.position;
        desiredD.Normalize();
        Vector3 desiredV = desiredD * MAX_SPEED;
        return desiredV - velocity;
    }

    //public void Avoid(GameObject target)
    //public Vector3 Avoid(Vector3 target)
    //{
        //Vector3 steeringV = SteerAway(target); // steering velocity
        //Vector3 steeringD = steeringV; // steering direction
        //steeringD.Normalize();
        // big coef makes it easier to steer to target
        //float seekForceCoef = 5.0f;
        //Vector3 acceleration = seekForceCoef * steeringD / mass;
        //Vector3 deltaV = Vector3.ClampMagnitude(
        //acceleration * Time.deltaTime, steeringV.magnitude);
        //velocity = target;
        //velocity = Vector3.ClampMagnitude(velocity + deltaV, MAX_SPEED);

        //velocity = target;
        //Move();
    //}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetType() == typeof(SphereCollider))
        {
            // detected nearby boid
            nearbyBoids.Add(System.Convert.ToInt32(collision.gameObject.name));
        }
        //else if (collision.GetType() == typeof(BoxCollider) && collision.gameObject.tag == "static")
        else if (collision.GetType() == typeof(BoxCollider) && collision.gameObject.name == "static")
        //else if (collision.GetType() == typeof(BoxCollider))
        {
            // detected collision with static object
            nearbyObjects.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.GetType() == typeof(SphereCollider))
        {
            nearbyBoids.Remove(System.Convert.ToInt32(collision.gameObject.name));
        }
        //else if (collision.GetType() == typeof(BoxCollider))
        else if (collision.GetType() == typeof(BoxCollider) && collision.gameObject.name == "static")
        {
            nearbyObjects.Remove(collision.gameObject);
        }
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

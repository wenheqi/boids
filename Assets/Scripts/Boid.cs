using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private static float MAX_SPEED = 10f;
    private static float MAX_FORCE = 7f;
    [SerializeField]
    private Vector3 velocity; // world space velocity
    private float mass;
    private float perceptionDist;
    private float separationDist;
    private float weightCohesion;
    private float weightSeparation;

    // Start is called before the first frame update
    void Start()
    {
        mass = 1f;
        perceptionDist = 3f;
        // Note: if separation distance is too small, two fish will kiss
        // each other repeatedly
        separationDist = 1.5f;
        weightCohesion = 0.2f;
        weightSeparation = 0.8f;
        velocity = Random.onUnitSphere * MAX_SPEED;
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
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
    public void MoveInFlock(List<BoidProperty> flock)
    {
        Vector3 steeringVelocity = Vector3.zero;

        steeringVelocity += weightCohesion * Cohere(flock);
        steeringVelocity += weightSeparation * Separate(flock);

        Vector3 steeringDir = steeringVelocity;
        steeringDir.Normalize();
        // steering_force = truncate (steering_direction, max_force)
        Vector3 steeringForce = steeringDir * MAX_FORCE;
        // acceleration = steering_force / mass
        Vector3 acceleration = steeringForce / mass;
        // velocity = truncate(velocity + acceleration, max_speed)
        Vector3 deltaVelocity = acceleration * Time.deltaTime;
        // considering the boid may not need max_speed to steer, take the min
        // velocity
        velocity = Vector3.ClampMagnitude(
            velocity +
            (deltaVelocity.sqrMagnitude <= steeringVelocity.sqrMagnitude ?
             deltaVelocity : steeringVelocity), MAX_SPEED);

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
    private Vector3 Cohere(List<BoidProperty> flockmates)
    {
        Vector3 steering = Vector3.zero;
        Vector3 centroid = Vector3.zero;
        int numFlockmates = 0;

        foreach(BoidProperty flockmate in flockmates)
        {
            // exclude boid itself from blockmates
            if (flockmate.position != transform.position &&
                Vector3.Distance(this.transform.position,
                flockmate.position) <= perceptionDist)
            {
                // positon is in world space
                centroid += flockmate.position;
                numFlockmates++;
            }
        }

        if (numFlockmates > 0)
        {
            centroid /= numFlockmates;
            // desired_velocity = normalize (position - target) * max_speed
            // Note: the paper seems to be wrong about the vector calculation
            // should be target - position instead of position - target
            Vector3 desiredVelocity = centroid - transform.position;
            desiredVelocity.Normalize();
            desiredVelocity *= MAX_SPEED;
            // steering = desired_velocity - velocity
            steering = desiredVelocity - velocity;
        }
        
        return steering;
    }

    private Vector3 Separate(List<BoidProperty> flockmates)
    {
        Vector3 steering = Vector3.zero;
        Vector3 flee = Vector3.zero;
        int numFlockmates = 0;

        foreach (BoidProperty flockmate in flockmates)
        {
            // exclude boid itself from blockmates
            if (flockmate.position != transform.position &&
                Vector3.Distance(this.transform.position,
                flockmate.position) <= perceptionDist)
            {
                if (Vector3.Distance(transform.position, flockmate.position) <
                    separationDist)
                {
                    // calculate how far and in what direction the boid wants to
                    // flee
                    Vector3 runaway = transform.position - flockmate.position;
                    runaway.Normalize();
                    runaway *= 1 / separationDist;
                    flee += runaway;
                    numFlockmates++;
                }
            }
        }

        if (numFlockmates > 0)
        {
            flee.Normalize();
            steering = flee * MAX_SPEED - velocity;
        }

        return steering;
    }
}

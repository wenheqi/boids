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
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    /*
     * Returns the desired steering velocity
     */
    private Vector3 SteerToSeek(GameObject target)
    {
        Vector3 desiredD = target.transform.position - transform.position;
        desiredD.Normalize();
        Vector3 desiredV = desiredD * MAX_SPEED;
        return desiredV - velocity;
    }

    public void Seek(GameObject target)
    {
        Vector3 steeringV = SteerToSeek(target); // steering velocity
        Vector3 steeringD = steeringV; // steering direction
        steeringD.Normalize();
        // big coef makes it easier to steer to target
        float seekForceCoef = 5.0f;
        Vector3 acceleration = seekForceCoef * steeringD / mass;
        Vector3 deltaV = Vector3.ClampMagnitude(
            acceleration * Time.deltaTime, steeringV.magnitude);
        velocity = Vector3.ClampMagnitude(velocity + deltaV, MAX_SPEED);
        Move();
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
    private Vector3 Cohere(List<BoidProperty> flockmates)
    {
        Vector3 steering = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        int numFlockmates = 0;

        foreach(BoidProperty flockmate in flockmates)
        {
            // exclude boid itself from blockmates
            if (flockmate.position != transform.position &&
                Vector3.Distance(this.transform.position,
                flockmate.position) <= cohesionDist)
            {
                // positon is in world space
                avgPosition += flockmate.position;
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
                flockmate.position) <= separationDist)
            {
                if (Vector3.Distance(transform.position, flockmate.position) <
                    separationDist)
                {
                    // calculate how far and in what direction the boid wants to
                    // flee
                    Vector3 offset = transform.position - flockmate.position;
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
    private Vector3 Align(List<BoidProperty> flockmates)
    {
        Vector3 steering = Vector3.zero;
        Vector3 avgForward = Vector3.zero;
        int numFlockmates = 0;

        foreach (BoidProperty flockmate in flockmates)
        {
            // exclude boid itself from flockmates
            if (transform.position != flockmate.position &&
                Vector3.Distance(this.transform.position,
                flockmate.position) <= alignmentDist)
            {
                avgForward += flockmate.forward;
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
}

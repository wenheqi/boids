using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private float mass = 1;
    private float maxSpeed = 9f;
    private float maxForce = 27f;
    private Vector3 velocity = Vector3.forward; // world space velocity

    private bool avoidanceEnabled = false;
    private float avoidanceStrength = 25f;
    private float avoidanceDist = 15f; // obstacle avoidance detection distance

    private bool alignmentEnabled = false;
    private float alignmentDist = 7.5f;
    private float alignmentAngle = 180f; // in degrees
    private float alignmentStrength = 8.0f;

    private bool cohesionEnabled = false;
    private float cohesionDist = 9.0f;
    private float cohesionAngle = 180f; // in degrees
    private float cohesionStrength = 8.0f;

    private bool separationEnabled = false;
    private float separationDist = 5.0f;
    private float separationAngle = 180f; // in degrees
    private float separationStrength = 12.0f;


    public static Boid Create(
        string prefabPath,
        Vector3 position,
        Quaternion rotation)
    {
        Boid prefab = Resources.Load<Boid>(prefabPath);
        return Instantiate(prefab, position, rotation);
    }

    public static Boid Create(
        Boid prefab,
        Vector3 position,
        Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }

    public float Mass
    {
        get
        {
            return mass;
        }
        set
        {
            if (value <= 0)
            {
                mass = 1; // avoid dividing by zero
                return;
            }
            mass = value;
        }
    }

    public float MaxSpeed
    {
        get
        {
            return maxSpeed;
        }
        set
        {
            if (value < 0)
            {
                maxSpeed = 0;
                return;
            }
            maxSpeed = value;
        }
    }

    public float MaxForce
    {
        get
        {
            return maxForce;
        }
        set
        {
            if (value < 0)
            {
                maxForce = 0;
                return;
            }
            maxForce = value;
        }
    }

    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }
        set
        {
            velocity = Vector3.ClampMagnitude(value, maxSpeed);
        }
    }

    public bool AvoidanceEnabled
    {
        get
        {
            return avoidanceEnabled;
        }
        set
        {
            avoidanceEnabled = value;
        }
    }

    public float AvoidanceDist
    {
        get
        {
            return avoidanceDist;
        }
        set
        {
            if (value < 0)
            {
                avoidanceDist = 0;
                return;
            }
            avoidanceDist = value;
        }
    }

    public float AvoidanceStrength
    {
        get
        {
            return avoidanceStrength;
        }
        set
        {
            if (value < 0)
            {
                avoidanceStrength = 0;
                return;
            }
            avoidanceStrength = value;
        }
    }

    public bool AlignmentEnabled
    {
        get
        {
            return alignmentEnabled;
        }
        set
        {
            alignmentEnabled = value;
        }
    }

    public float AlignmentDist
    {
        get
        {
            return alignmentDist;
        }
        set
        {
            if (value < 0)
            {
                alignmentDist = 0;
                return;
            }
            alignmentDist = value;
        }
    }

    public float AlignmentStrength
    {
        get
        {
            return alignmentStrength;
        }
        set
        {
            if (value < 0)
            {
                alignmentStrength = 0;
                return;
            }
            alignmentStrength = value;
        }
    }

    public bool CohesionEnabled
    {
        get
        {
            return cohesionEnabled;
        }
        set
        {
            cohesionEnabled = value;
        }
    }

    public float CohesionDist
    {
        get
        {
            return cohesionDist;
        }
        set
        {
            if (value < 0)
            {
                cohesionDist = 0;
                return;
            }
            cohesionDist = value;
        }
    }

    public float CohesionStrength
    {
        get
        {
            return cohesionStrength;
        }
        set
        {
            if (value < 0)
            {
                cohesionStrength = 0;
                return;
            }
            cohesionStrength = value;
        }
    }

    public bool SeparationEnabled
    {
        get
        {
            return separationEnabled;
        }
        set
        {
            separationEnabled = value;
        }
    }

    public float SeparationDist
    {
        get
        {
            return separationDist;
        }
        set
        {
            if (value < 0)
            {
                separationDist = 0;
                return;
            }
            separationDist = value;
        }
    }

    public float SeparationStrength
    {
        get
        {
            return separationStrength;
        }
        set
        {
            if (value < 0)
            {
                separationStrength = 0;
                return;
            }
            separationStrength = value;
        }
    }

    public float PerceptionDist
    {
        get
        {
            return Mathf.Max(
                Mathf.Max(alignmentDist, cohesionDist),
                separationDist
                );
        }
    }

    public void Move()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    private void Steer(Vector3 steeringV, float forceCoef)
    {
        Vector3 steeringD = steeringV; // steering direction
        steeringD.Normalize();

        Vector3 acceleration = Vector3.ClampMagnitude(
            steeringD * forceCoef, maxForce) / mass;

        Vector3 deltaV = Vector3.ClampMagnitude(
            acceleration * Time.deltaTime, steeringV.magnitude);

        velocity = Vector3.ClampMagnitude(velocity + deltaV, maxSpeed);

        Move();
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
     *  List<BoidProperty> flockmates: a list of properties of nearby boids 
     * Return:
     *  the difference between this desired velocity and the character’s current
     *  velocity
     */
    public Vector3 Align(List<BoidProperty> flockmates)
    {
        Vector3 steering = Vector3.zero;
        Vector3 avgForward = Vector3.zero;
        int numFlockmates = 0;

        foreach (BoidProperty flockmate in flockmates)
        {
            // exclude boid itself from flockmates
            if (transform.position != flockmate.position &&
                Vector3.Distance(this.transform.position,
                flockmate.position) <= alignmentDist &&
                Vector3.Angle(transform.forward,
                    flockmate.position - transform.position
                ) <= alignmentAngle)
            {
                avgForward += flockmate.forward;
                numFlockmates++;
            }
        }

        if (numFlockmates > 0)
        {
            avgForward /= numFlockmates;
            avgForward.Normalize();
            steering = avgForward * maxSpeed - velocity;
        }

        return steering;
    }

    private void SteerToAlign(List<BoidProperty> flockmates)
    {
        Steer(Align(flockmates), alignmentStrength);
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
     *  List<BoidProperty> flockmates: a list of properties of nearby boids 
     * Return:
     *  the difference between desired velocity and the object’s current 
     *  velocity
     */
    private Vector3 Cohere(List<BoidProperty> flockmates)
    {
        Vector3 steering = Vector3.zero;
        Vector3 avgPosition = Vector3.zero;
        int numFlockmates = 0;

        foreach (BoidProperty flockmate in flockmates)
        {
            // exclude boid itself from blockmates
            if (flockmate.position != transform.position &&
                Vector3.Distance(this.transform.position,
                flockmate.position) <= cohesionDist &&
                Vector3.Angle(transform.forward,
                    flockmate.position - transform.position
                ) <= cohesionAngle)
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
            desiredVelocity *= maxSpeed;
            steering = desiredVelocity - velocity;
        }

        return steering;
    }

    public void SteerToCohere(List<BoidProperty> flockmates)
    {
        Steer(Cohere(flockmates), cohesionStrength);
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
                flockmate.position) <= separationDist &&
                Vector3.Angle(transform.forward,
                    flockmate.position - transform.position
                ) <= separationAngle)
            {
                // calculate how far and in what direction the boid wants to
                // flee
                Vector3 offset = transform.position - flockmate.position;
                offset /= offset.sqrMagnitude;
                flee += offset;
                numFlockmates++;
            }
        }

        if (numFlockmates > 0)
        {
            flee /= numFlockmates;
            flee.Normalize();
            steering = flee * maxSpeed - velocity;
        }

        return steering;
    }

    public void SteerToSeparate(List<BoidProperty> flockmates)
    {
        Steer(Separate(flockmates), separationStrength);
    }

    // Start is called before the first frame update
    void Start()
    {
        // initialize velocity with initial orientation if set
        if (transform.forward != Vector3.zero)
        {
            velocity = transform.forward * maxSpeed;
        }
        // otherwise generate a random velocity and head towards it
        else
        {
            velocity = Random.onUnitSphere * maxSpeed;
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
        // avoidance vector not encapsulated due to needing in calculating delta velocity
        Vector3 avoidanceV = Vector3.zero;

        // desired avoidance steering velocity/direction
        if (avoidanceEnabled)
        {
            avoidanceV = AvoidObstacle();
            Vector3 avoidanceD = avoidanceV;
            avoidanceD.Normalize();
            steeringVelocity += avoidanceV;
            steeringForce += avoidanceStrength * avoidanceD;
        }

        // desired separation steering velocity/direction
        if (steeringForce.magnitude < maxForce && separationEnabled)
        {
            Vector3 separationV = Separate(flock);
            Vector3 separationD = separationV;
            separationD.Normalize();
            steeringVelocity += separationV;
            steeringForce += separationStrength * separationD;
        }

        // desired alignment steering velocity/direction
        if (steeringForce.magnitude < maxForce && alignmentEnabled)
        {
            Vector3 alignmentV = Align(flock);
            Vector3 alignmentD = alignmentV;
            alignmentD.Normalize();
            steeringVelocity += alignmentV;
            steeringForce += alignmentStrength * alignmentD;
        }
        
        // desired cohesion steering velocity/direction
        if (steeringForce.magnitude < maxForce && cohesionEnabled)
        {
            Vector3 cohesionV = Cohere(flock);
            Vector3 cohesionD = cohesionV;
            cohesionD.Normalize();
            steeringVelocity += cohesionV;
            steeringForce += cohesionStrength * cohesionD;
        }
        

        // steering_force = truncate (steering_direction, max_force)
        steeringForce = Vector3.ClampMagnitude(steeringForce, maxForce);
        steeringVelocity = Vector3.ClampMagnitude(steeringVelocity, maxSpeed);
        if (steeringForce == Vector3.zero)
        {
            // boid forward direction remains the same, speed up to
            // reach max speed limit
            steeringForce = transform.forward * maxForce * 0.05f;
            steeringVelocity = transform.forward * maxSpeed;
        }
        // acceleration = steering_force / mass
        Vector3 acceleration = steeringForce / mass;
        // avoid overshooting
        Vector3 deltaVelocity = Vector3.zero;

        deltaVelocity = Vector3.ClampMagnitude(
            acceleration * Time.deltaTime, steeringVelocity.magnitude);

        velocity = Vector3.ClampMagnitude(velocity + deltaVelocity, maxSpeed);

        // normalize to effective speed range of boids
        //float fractionOfMaxVelocity = velocity.magnitude / maxSpeed;
        float normVelocity = (velocity.magnitude - 4.0f) / (6.0f - 4.0f);
        if (normVelocity < 0)
        {
            normVelocity = 0;
        }

        // set color of trail based on max velocity (higher speed = brighter)
        // changes boid body color based on current velocity (more teal = faster, more red = slower)
        GetComponent<Renderer>().material.SetColor("_Color", new Color(0.7f, normVelocity, normVelocity, 1));

        // changes trail color in same manner
        GetComponent<TrailRenderer>().material.SetColor("_Color", new Color(0.7f, normVelocity, normVelocity, 1));


        Move();
    }

    public Vector3 AvoidObstacle()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Vector3 steering = Vector3.zero;
        if (Physics.Raycast(ray, out hit, avoidanceDist))
        {
            if (hit.collider.gameObject.layer == 3) // only consider static object collisions, ignore other boids
            {
                Debug.Log(hit.collider.name);
                if (hit.normal == -transform.forward)
                {
                    steering = transform.right * maxSpeed;
                }
                else
                {
                    // normalized vector reflecting from hit surface
                    steering = Vector3.Normalize(hit.normal); // using normal surface vector to change acceleration vector

                    // reflected vector implementation
                    //Vector3 normalizedHitNormal = Vector3.Normalize(hit.normal);
                    // reflected ray vector from the boid vector around the normal surface vector
                    //steering = velocity - 2 * Vector3.Dot(velocity, normalizedHitNormal) * normalizedHitNormal;
                    //steering = Vector3.Normalize(steering) * maxSpeed;
                }
            }
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
        Vector3 desiredV = desiredD * maxSpeed;
        return desiredV - velocity;
    }
}

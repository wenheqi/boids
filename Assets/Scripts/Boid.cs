using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private float mass = 1;
    private float maxSpeed = 20f;
    private float maxForce = 100f;
    private Vector3 velocity = Vector3.forward; // world space velocity

    private bool alignmentEnabled = false;
    private float alignmentDist = 7.5f;
    private float alignmentAngle = 180f; // in degrees
    private float alignmentStrength = 15.0f;

    private bool cohesionEnabled = false;
    private float cohesionDist = 9.0f;
    private float cohesionAngle = 180f; // in degrees
    private float cohesionStrength = 15.0f;

    private bool separationEnabled = false;
    private float separationDist = 5.0f;
    private float separationAngle = 180f; // in degrees
    private float separationStrength = 20.0f;

    private Vector3 goal = Vector3.zero;
    private bool goalSeekingEnabled = false;
    private float goalSeekingStrength = 18.0f;

    private bool bankingEnabled = false;
    private float liftStregth = 10.0f;

    private bool avoidEnabled = false;
    private float avoidDist = 5.0f;
    private float avoidAngle = 180f; // angle boid can escape, in degree
    private float avoidStrength = 15f;
    private int numAvoidRays = 20;
    private float fov = 3f; // in degree

    private int layerMask = 1 << 6;

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

    public float AlignmentAngle
    {
        get
        {
            return alignmentAngle;
        }
        set
        {
            if (value < 0)
            {
                alignmentAngle = 0;
                return;
            }
            if (value > 180)
            {
                alignmentAngle = 180;
                return;
            }
            alignmentAngle = value;
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

    public float CohesionAngle
    {
        get
        {
            return cohesionAngle;
        }
        set
        {
            if (value < 0)
            {
                cohesionAngle = 0;
                return;
            }
            if (value > 180)
            {
                cohesionAngle = 180;
                return;
            }
            cohesionAngle = value;
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

    public float SeparationAngle
    {
        get
        {
            return separationAngle;
        }
        set
        {
            if (value < 0)
            {
                separationAngle = 0;
                return;
            }
            if (value > 180)
            {
                separationAngle = 180;
                return;
            }
            separationAngle = value;
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

    public Vector3 Goal
    {
        get
        {
            return goal;
        }
        set
        {
            goal = value;
        }
    }

    public bool GoalSeekingEnabled
    {
        get
        {
            return goalSeekingEnabled;
        }
        set
        {
            goalSeekingEnabled = value;
        }
    }

    public bool BankingEnabled
    {
        get
        {
            return bankingEnabled;
        }
        set
        {
            bankingEnabled = value;
        }
    }

    public float LiftStrength
    {
        get
        {
            return liftStregth;
        }
        set
        {
            liftStregth = value;
        }
    }

    public bool AvoidEnabled
    {
        get
        {
            return avoidEnabled;
        }
        set
        {
            avoidEnabled = value;
        }
    }

    public float AvoidDist
    {
        get
        {
            return avoidDist;
        }
        set
        {
            avoidDist = value;
        }
    }

    public float AvoidAngle
    {
        get
        {
            return avoidAngle;
        }
        set
        {
            if (value < 0)
            {
                avoidAngle = 0;
                return;
            }
            if (value > 180)
            {
                avoidAngle = 180;
                return;
            }
            avoidAngle = value;
        }
    }

    public float AvoidStrength
    {
        get
        {
            return avoidStrength;
        }
        set
        {
            avoidStrength = value;
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
        Vector3 steeringD = Vector3.Normalize(steeringV); // steering direction

        Vector3 steeringF = Vector3.ClampMagnitude(
            steeringD * forceCoef, maxForce);

        Vector3 acceleration = steeringF / mass;

        Vector3 deltaV = Vector3.ClampMagnitude(
            acceleration * Time.deltaTime, steeringV.magnitude);

        //Velocity = velocity + deltaV;
        velocity = Vector3.ClampMagnitude(velocity + deltaV, maxSpeed);

        // set new position
        transform.Translate(velocity * Time.deltaTime, Space.World);
        // set new orientation
        if (velocity != Vector3.zero)
        {
            if (bankingEnabled)
            {
                // velocity is new forward
                Vector3 oldUp = transform.up;
                Vector3 newRight = Vector3.Normalize(
                    Vector3.Cross(velocity, oldUp));
                Vector3 newUp = Vector3.Normalize(
                    Vector3.Cross(newRight, velocity));
                newUp = Vector3.Normalize(
                    newRight * Vector3.Dot(steeringF, newRight) +
                    newUp * (mass * liftStregth));
                transform.rotation = Quaternion.LookRotation(velocity, newUp);
                return;
            }

            // look at the new forward direction
            transform.rotation = Quaternion.LookRotation(velocity);
        }
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
            // steering = desired_velocity - velocity
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

    public Vector3 SeekGoal()
    {
        return (goal - transform.position) * maxSpeed - velocity;
    }

    private void SteerToSeekGoal()
    {
        Steer(SeekGoal(), goalSeekingStrength);
    }

    private Vector3 AvoidAwayFromSurface()
    {
        Vector3 steering = Vector3.zero;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, avoidDist, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            float dist = Vector3.Distance(transform.position, hit.point);
            if (dist == 0)
            {
                return hit.normal * maxSpeed -velocity;
            }
            steering = hit.normal * maxSpeed * avoidDist/dist - velocity;
        }
        return steering;
    }

    private Vector3 AvoidAlongRaycast()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Vector3 steering = Vector3.zero;

        if (isGoingToCollide())
        {
            //int numViewDirections = 300;
            //Vector3[] directions = new Vector3[numViewDirections];

            //float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
            //float angleIncrement = Mathf.PI * 2 * goldenRatio;

            //for (int i = 0; i < numViewDirections; i++)
            //{
            //    float t = (float)i / numViewDirections;
            //    float inclination = Mathf.Acos(1 - 2 * t);
            //    float azimuth = angleIncrement * i;

            //    float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            //    float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            //    float z = Mathf.Cos(inclination);
            //    directions[i] = new Vector3(x, y, z);
            //}

            //Vector3[] rayDirections = directions;

            //for (int i = 0; i < rayDirections.Length; i++)
            //{
            //    Vector3 dir = transform.TransformDirection(rayDirections[i]);
            //    ray = new Ray(transform.position, dir);
            //    if (!Physics.SphereCast(ray, 0.33f, avoidDist, layerMask))
            //    {
            //        return dir * maxSpeed - velocity;
            //    }
            //}

            //return Vector3.zero;


            // find a direction to avoid obstacle
            for (float i = fov / 2 + 5; i <= avoidAngle; i += 5)
            {
                Vector3 newBaseDir = Quaternion.AngleAxis(
                                        i,
                                        transform.up) *
                                     transform.forward;
                Gizmos.color = Color.Lerp(Color.yellow, Color.yellow, i / avoidAngle);
                // rotate this base direction around forward axis for 360 degrees
                for (int j = 0; j < 360; j += 5)
                {
                    Vector3 newDir = Quaternion.AngleAxis(
                                        j,
                                        transform.forward) *
                                     newBaseDir;
                    ray.direction = newDir;
                    if (Physics.SphereCast(transform.position, .33f, newDir, out hit, avoidDist, layerMask))
                    {
                        // still hit
                        Debug.DrawLine(ray.origin, hit.point, Color.red);
                    }
                    else
                    {
                        Debug.DrawLine(ray.origin, ray.origin + ray.direction * avoidDist, Color.green);
                        steering = ray.direction * maxSpeed - velocity;
                        return steering;
                    }
                }
            }
            // can not find a way, try to go back
            // better not to reach here
            return -transform.forward * maxSpeed - velocity;
        }
        // safe
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * avoidDist, Color.green);
        return steering;
    }

    private Vector3 AvoidObstacle()
    {
        return AvoidAlongRaycast();
    }

    public void SteerToAvoidObstacle()
    {
        Vector3 steeringV = AvoidObstacle(); // steering velocity
        // if there is no steering, i.e. boid's direction remains the same
        // try to accelerate until it reaches max speed
        steeringV = steeringV == Vector3.zero ? transform.forward * maxSpeed : steeringV;
        Vector3 steeringD = steeringV; // steering direction
        steeringD.Normalize();
        Vector3 acceleration = avoidStrength * steeringD / mass;
        Vector3 deltaV = Vector3.ClampMagnitude(
            acceleration * Time.deltaTime, steeringV.magnitude);
        velocity = Vector3.ClampMagnitude(velocity + deltaV, maxSpeed);
        Move();
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
        bool avoidingObstacle = false;

        if (avoidEnabled)
        {
            Vector3 avoidV = AvoidObstacle();
            Vector3 avoidD = Vector3.Normalize(avoidV);
            if (avoidV != Vector3.zero)
            {
                avoidingObstacle = true;
                steeringVelocity += avoidV;
                steeringForce += avoidStrength * avoidD;
            }
        }

        if (avoidingObstacle)
        {
            goto Move;
        }

        // desired separation steering velocity/direction
        if (separationEnabled)
        {
            Vector3 separationV = Separate(flock);
            Vector3 separationD = separationV;
            separationD.Normalize();
            steeringVelocity += separationV;
            steeringForce += separationStrength * separationD;
        }

        // desired alignment steering velocity/direction
        if (alignmentEnabled)
        {
            Vector3 alignmentV = Align(flock);
            Vector3 alignmentD = alignmentV;
            alignmentD.Normalize();
            steeringVelocity += alignmentV;
            steeringForce += alignmentStrength * alignmentD;
        }

        // desired cohesion steering velocity/direction
        if (cohesionEnabled)
        {
            Vector3 cohesionV = Cohere(flock);
            Vector3 cohesionD = cohesionV;
            cohesionD.Normalize();
            steeringVelocity += cohesionV;
            steeringForce += cohesionStrength * cohesionD;
        }

        if (goalSeekingEnabled)
        {
            Vector3 goalSeekingV = SeekGoal();
            Vector3 goalSeekingD = goalSeekingV;
            goalSeekingD.Normalize();
            steeringVelocity += goalSeekingV;
            steeringForce += goalSeekingStrength * goalSeekingD;
        }

    Move:
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
        Vector3 deltaVelocity = Vector3.ClampMagnitude(
            acceleration * Time.deltaTime, steeringVelocity.magnitude);
        // velocity = truncate(velocity + acceleration, max_speed)
        velocity = Vector3.ClampMagnitude(velocity + deltaVelocity, maxSpeed);

        // set new position
        transform.Translate(velocity * Time.deltaTime, Space.World);
        // set new orientation
        if (velocity != Vector3.zero)
        {
            if (bankingEnabled)
            {
                // velocity is new forward
                Vector3 oldUp = transform.up;
                Vector3 newRight = Vector3.Normalize(
                    Vector3.Cross(velocity, oldUp));
                Vector3 newUp = Vector3.Normalize(
                    Vector3.Cross(newRight, velocity));
                newUp = Vector3.Normalize(
                    newRight * Vector3.Dot(steeringForce, newRight) +
                    newUp * (mass * liftStregth));
                transform.rotation = Quaternion.LookRotation(velocity, newUp);
                return;
            }

            // look at the new forward direction
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }

    //public void OnDrawGizmos()
    //{
    //    //Gizmos.color = Color.cyan;
    //    //Gizmos.DrawSphere(transform.position, 0.33f);
    //    // 2D
    //    //Gizmos.color = Color.green;
    //    //for (int i = 0; i <= numAvoidRays; i++)
    //    //{
    //    //    Vector3[] v = new Vector3[] { transform.up, transform.right };
    //    //    for (int j = 0; j < v.Length; j++)
    //    //    {
    //    //        for (int k = -1; k <= 1; k++)
    //    //        {
    //    //            if (k == 0) { continue; }
    //    //            // rotate
    //    //            Gizmos.DrawRay(
    //    //                transform.position,
    //    //                Quaternion.AngleAxis(
    //    //                    i * k * avoidAngle / (float)numAvoidRays,
    //    //                    v[j]) *
    //    //                transform.forward);
    //    //        }
    //    //    }
    //    //}

    //    // 3D
    //    Gizmos.color = Color.blue;
    //    for (int i = 5; i <= avoidAngle; i+=5)
    //    {
    //        Vector3 newBaseDir = Quaternion.AngleAxis(
    //                                i,
    //                                transform.up) *
    //                             transform.forward;
    //        Gizmos.color = Color.Lerp(Color.blue, Color.magenta, i / avoidAngle);
    //        // rotate this base direction around forward axis for 360 degrees
    //        for (int j = 0; j < 360; j+=5)
    //        {
    //            Vector3 newDir = Quaternion.AngleAxis(
    //                                j,
    //                                transform.forward) *
    //                             newBaseDir;
    //            Gizmos.DrawRay(
    //                transform.position,
    //                newDir);
    //        }
    //    }
    //}

    private bool isGoingToCollide()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, .33f, transform.forward, out hit, avoidDist, layerMask))
        {
            return true;
        }
        else { }
        return false;

        Ray ray = new Ray(transform.position, transform.forward);
        //RaycastHit hit;

        if (Physics.Raycast(ray, out hit, avoidDist, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);
            return true;
        }

        float halfFov = fov / 2;

        for (float i = 1; i <= halfFov; i++)
        {
            Vector3 newBaseDir = Quaternion.AngleAxis(
                                    i,
                                    transform.up) *
                                 transform.forward;
            Gizmos.color = Color.Lerp(Color.blue, Color.magenta, i / halfFov);
            // rotate this base direction around forward axis for 360 degrees
            for (float j = 0; j < 360; j += 5)
            {
                Vector3 newDir = Quaternion.AngleAxis(
                                    j,
                                    transform.forward) *
                                 newBaseDir;
                ray.direction = newDir;
                if (Physics.Raycast(ray, out hit, avoidDist, layerMask))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    return true;
                }
                else
                {
                    Debug.DrawLine(ray.origin, ray.origin + ray.direction * avoidDist, Color.green);
                }
            }
        }
        return false;
    }
}
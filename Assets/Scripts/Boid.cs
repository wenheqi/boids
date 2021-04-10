using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private static float MAX_SPEED = 1f;
    private static float MAX_FORCE = 2f;
    private static float PERCEPTION_DIST = 1;
    [SerializeField]
    private Vector3 velocity; // world space velocity
    private float mass;
    /*
     * Move forward based on current velocity
     */
    public void Move()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    /*
     * Move based on Craig Reynolds' three rules
     */
    public void MoveInFlock(List<BoidProperty> flock)
    {
        Vector3 steeringDir;
        Vector3 steeringForce;
        Vector3 acceleration;

        steeringDir = this.Cohere(flock);
        steeringDir.Normalize();

        // steering_force = truncate (steering_direction, max_force)
        steeringForce = Vector3.ClampMagnitude(steeringDir * MAX_FORCE, MAX_FORCE);
        // acceleration = steering_force / mass
        acceleration = steeringForce / mass;
        // velocity = truncate(velocity + acceleration, max_speed)
        velocity = Vector3.ClampMagnitude(velocity + acceleration * Time.deltaTime, MAX_SPEED);

        Move();
    }

    /*
     * Flock centering makes a boid want to be near the center of the flock.
     * Because each boid has a localized perception of the world, "center of the
     * flock" actually means the center of the nearby flockmates. Flock 
     * centering causes the boid to fly in a direction that moves it closer to 
     * the centroid of the nearby boids. If a boid is deep inside a flock, the 
     * population density in its neighborhood is roughly homogeneous; the boid 
     * density is approximately the same in all directions. In this case, the 
     * centroid of the neighborhood boids is approximately at the center of the 
     * neighborhood, so the flock centering urge is small. But if a boid is on 
     * the boundary of the flock, its neighboring boids are on one side. The 
     * centroid of the neighborhood boids is displaced from the center of the 
     * neighborhood toward the body of the flock. Here the flock centering urge 
     * is stronger and the flight path will be deflected somewhat toward the 
     * local flock center.
     */
    private Vector3 Cohere(List<BoidProperty> flockmates)
    {
        Vector3 steeringDir;
        Vector3 centroid = Vector3.zero;

        foreach(BoidProperty flockmate in flockmates)
        {
            // exclude boid itself from blockmates
            if (flockmate.position != transform.position &&
                Vector3.Distance(this.transform.position,
                flockmate.position) <= PERCEPTION_DIST)
            {
                // positon is in world space
                centroid += flockmate.position;
            }
        }

        centroid /= flockmates.Count;

        steeringDir = (centroid - transform.position) - velocity;
        steeringDir.Normalize();

        return steeringDir;
    }

    // Start is called before the first frame update
    void Start()
    {
        mass = 1f;
        velocity = Random.onUnitSphere * MAX_SPEED;
        transform.rotation = Quaternion.LookRotation(velocity);
    }
}

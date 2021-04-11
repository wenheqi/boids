using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidProperty
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;

    public BoidProperty(Boid boid)
    {
        position = boid.transform.position;
        rotation = boid.transform.rotation;
        velocity = boid.GetVelocity();
    }
}

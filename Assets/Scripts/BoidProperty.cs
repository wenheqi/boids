using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidProperty
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 forward;

    public BoidProperty(Boid boid)
    {
        position = boid.transform.position;
        rotation = boid.transform.rotation;
        velocity = boid.Velocity;
        forward = boid.transform.forward;
    }

    public override bool Equals(object obj)
    {
        // compare an instance with itself
        if (System.Object.ReferenceEquals(obj, this))
            return true;

        BoidProperty other = obj as BoidProperty;

        if (System.Object.ReferenceEquals(null, other))
            return false;

        if (position.Equals(other.position) &&
            rotation.Equals(other.rotation) &&
            velocity.Equals(other.velocity) &&
            forward.Equals(other.forward))
        {
            return true;
        }
        return false;
    }

    public override int GetHashCode()
    {
        long hash = position.GetHashCode() +
                    rotation.GetHashCode() +
                    velocity.GetHashCode() +
                    forward.GetHashCode();
        return hash > Int32.MaxValue ?
               Convert.ToInt32(hash % Int32.MaxValue) :
               Convert.ToInt32(hash);
    }
}

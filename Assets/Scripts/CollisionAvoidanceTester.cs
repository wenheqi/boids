using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceTester : MonoBehaviour
{
    public Boid boid;
    // Start is called before the first frame update
    void Start()
    {
        //Vector3 v = Random.onUnitSphere;
        //boid = Instantiate(boid, v, Quaternion.LookRotation(v));
        Vector3 position = new Vector3(2f,2f,2f);
        boid = Instantiate(boid, position, Random.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        boid.AvoidObstacle();
    }
}

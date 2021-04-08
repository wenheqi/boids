using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [SerializeField]
    // world space velocity
    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(0f, 0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        // Translate work on local axis
        transform.Translate(transform.InverseTransformDirection(velocity) * Time.deltaTime, Space.Self);
        transform.rotation = Quaternion.LookRotation(velocity);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    private static float MAX_SPEED = 2f;
    [SerializeField]
    public Vector3 velocity; // world space velocity

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.ClampMagnitude(
                new Vector3(
                    Random.Range(-MAX_SPEED, MAX_SPEED),
                    Random.Range(-MAX_SPEED, MAX_SPEED),
                    Random.Range(-MAX_SPEED, MAX_SPEED)
                ),
                MAX_SPEED
            );
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.LookRotation(velocity);
    }
}

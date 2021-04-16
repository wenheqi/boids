using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeTester : MonoBehaviour
{
    [SerializeField]
    private Boid boid;
    [SerializeField]
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        // generate a random position
        Vector3 position = new Vector3(
                Random.Range(-3.0f, 0.0f),
                Random.Range(0.0f, 3.0f),
                Random.Range(-3.0f, 0.0f)
            );
        boid = Instantiate(boid, position, Quaternion.LookRotation(target.transform.position - position));
    }

    // Update is called once per frame
    void Update()
    {
        boid.Flee(target);
    }
}

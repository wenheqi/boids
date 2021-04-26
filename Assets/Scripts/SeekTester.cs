using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekTester : MonoBehaviour
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
                Random.Range(0.0f, 50.0f),
                Random.Range(0.0f, 50.0f),
                Random.Range(0.0f, 50.0f)
            );
        boid = Instantiate(boid, position, Random.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        boid.SeekGoal();
    }
}

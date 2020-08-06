using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreMe : MonoBehaviour
{
    private Vector3 respawnPoint;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        respawnPoint = transform.position + Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -20)
        {
            transform.position = respawnPoint;
            rb.velocity = Vector3.zero;
        }
    }
}

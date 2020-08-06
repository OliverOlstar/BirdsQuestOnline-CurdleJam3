using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaning : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Vector3 previousVel = new Vector3(0, 0, 0);
    [SerializeField] private float leanFactor = 5;
    [SerializeField] private float leanDampening = 15;

    void FixedUpdate()
    {
        Vector3 accel = Horizontal(rb.velocity) - Horizontal(previousVel);
        Quaternion targetQ = Quaternion.Euler(accel.z * leanFactor, 0, -accel.x * leanFactor);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetQ, Time.fixedDeltaTime * leanDampening);
    }

    private Vector3 Horizontal(Vector3 pVector)
    {
        return new Vector3(pVector.x, 0, pVector.z);
    }
}

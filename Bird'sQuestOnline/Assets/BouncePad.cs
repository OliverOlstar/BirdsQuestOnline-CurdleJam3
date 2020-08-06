using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] private Vector3 force;
    [SerializeField] private float forceMag = 5;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRb = other.GetComponent<Rigidbody>();

        if (otherRb == null)
            otherRb = other.GetComponentInParent<Rigidbody>();

        if (otherRb != null)
            otherRb.velocity = force.normalized * forceMag;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + force);
    }
}

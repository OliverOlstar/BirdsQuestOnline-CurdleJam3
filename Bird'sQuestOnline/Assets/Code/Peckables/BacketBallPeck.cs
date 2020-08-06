using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacketBallPeck : MonoBehaviour, IPeckable
{
    private Rigidbody rb;
    private Collider col;

    [SerializeField] private Vector2 forces = new Vector2(5, 5);

    public int Type()
    {
        return 998;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        PecksList._instance.peckables.Add(ID(), this);
    }

    public Transform Pecked()
    {
        transform.Translate(Vector3.up * 40);

        rb.isKinematic = true;
        col.enabled = false;
        return transform;
    }

    public void Released()
    {
        Vector3 forward = transform.parent.forward;

        rb.isKinematic = false;
        rb.velocity = (forward * forces.x) + (Vector3.up * forces.y);
        
        col.enabled = true;
    }

    public IPeckable GetThis()
    {
        return this;
    }

    private int id;
    private void Awake()
    {
        id = Mathf.RoundToInt((transform.position.x + transform.position.y + transform.position.z) * 1000);
    }
    public int ID()
    {
        return id;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupablePeck : MonoBehaviour, IPeckable
{
    private Rigidbody rb;
    private Collider col;
    [SerializeField] private int type;

    public int Type()
    {
        return type;
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

    public IPeckable GetThis()
    {
        return this;
    }

    public void Released()
    {
        rb.isKinematic = false;
        col.enabled = true;
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
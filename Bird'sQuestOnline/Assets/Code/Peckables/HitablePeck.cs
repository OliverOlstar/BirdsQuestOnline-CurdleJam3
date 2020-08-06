using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitablePeck : MonoBehaviour, IPeckable
{
    [SerializeField] private int type;

    public int Type()
    {
        return type;
    }

    private void Start()
    {
        PecksList._instance.peckables.Add(ID(), this);
    }

    public Transform Pecked()
    {
        return null;
    }

    public IPeckable GetThis()
    {
        return this;
    }

    public void Released()
    {

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

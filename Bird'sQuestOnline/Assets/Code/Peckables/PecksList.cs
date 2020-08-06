using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PecksList : MonoBehaviour
{
    public static PecksList _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_instance != this)
                Destroy(this);
        }
    }

    public Dictionary<int, IPeckable> peckables = new Dictionary<int, IPeckable>();
}

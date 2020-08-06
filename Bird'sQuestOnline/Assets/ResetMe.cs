using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMe : MonoBehaviour
{
    public static ResetMe _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(this);
    }

    public void RespawnMe(Transform pMe)
    {
        pMe.position = transform.position;
    }
}
